//////////////////////////////////////////////////////////////////////
//                                                                  //
//  JCSP ("CSP for Java") Libraries                                 //
//  Copyright (C) 1996-2018 Peter Welch, Paul Austin and Neil Brown //
//                2001-2004 Quickstone Technologies Limited         //
//                2005-2018 Kevin Chalmers                          //
//                                                                  //
//  You may use this work under the terms of either                 //
//  1. The Apache License, Version 2.0                              //
//  2. or (at your option), the GNU Lesser General Public License,  //
//       version 2.1 or greater.                                    //
//                                                                  //
//  Full licence texts are included in the LICENCE file with        //
//  this library.                                                   //
//                                                                  //
//  Author contacts: P.H.Welch@kent.ac.uk K.Chalmers@napier.ac.uk   //
//                                                                  //
//////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using CSPnet2.Net2Link;
using CSPnet2.NetNode;

namespace CSPnet2.TCPIP
{
    /**
     * A concrete implementation of a Link that operates over a TCP/IP based socket connection. For information on Link, see
     * the relative documentation.
     * <p>
     * It is perfectly possible for a user to create a TCPIPLink without going through the standard LinkFactory approach,
     * although this is not recommended. For example:
     * </p>
     * <p>
     * <code>
     * TCPIPLink link = new TCPIPLink(address);<br>
     * link.connect();
     * link.registerLink();<br>
     * new ProcessManager(link).start();<br>
     * </code>
     * </p>
     * <p>
     * Can be achieved using the LinkFactory:
     * </p>
     * <p>
     * <code>
     * link = LinkFactory.getLink(address);
     * </code>
     * </p>
     * <p>
     * The LinkFactory will create and start the Link automatically if required.
     * </p>
     * 
     * @see Link
     * @see TCPIPNodeAddress
     * @author Kevin Chalmers
     */
    public sealed class TCPIPLink : Link
    {
        /**
         * Defines the size of the buffer to place on the incoming and outgoing streams. This is a rather large size, and
         * for certain implementations, this may be reduced. It is unlikely that any sent object will be this large.
         */
        public static int BUFFER_SIZE = 8192;

        /**
         * Flag to determine whether the Nagle algorithm should be activated. Default is false (off).
         */
        public static Boolean NAGLE = false;

        /**
         * The socket connected to the remote Node.
         */
        private Socket socket;

        /**
         * The address of the remote Node.
         */
        private TCPIPNodeAddress remoteAddress;

        /**
         * Creates a new TCPIPLink
         * 
         * @param address
         *            The address of the remote Node to connect to
         * @//throws JCSPNetworkException
         *             Thrown if something goes wrong during the creation process
         */
        public TCPIPLink(TCPIPNodeAddress address)
        //throws JCSPNetworkException
        {
            try
            {
                // First check if we have an ip address in the string. If not, we assume that this is to be connected
                // to the local machine but to a different JVM
                if (String.IsNullOrEmpty(address.GetIpAddressAsString()))
                {

                    IPAddress[] localIPAddresses = GetLocalIPAddress.GetAllAddresses();
                    IPAddress ipAddresstoUse = GetLocalIPAddress.GetOnlyLocalIPAddress();
                    address.setIpAddress(GetLocalIPAddress.ConvertIPAddressToString(ipAddresstoUse));

                    //TODO the code below is not updated and not working correctly. 
                    // We basically have four types of addresses to worry about. Loopback (127), link local (169),
                    // local (192) and (possibly) global. Grade each 1, 2, 3, 4 and use highest scoring address. In all
                    // cases use first address of that score.
                    int current = 0;

                    // Loop until we have checked all the addresses
                    for (int i = 0; i < localIPAddresses.Length; i++)
                    {
                        // Ensure we have an IPv4 address
                        if (localIPAddresses[i] is IPAddress)
                        {
                            // Get the first byte of the address
                            //byte first = localIPAddresses[i].getAddress()[0];
                            byte first = localIPAddresses[i].GetAddressBytes()[0];

                            // Now check the value
                            if (first == (byte)127 && current < 1)
                            {
                                // We have a Loopback address
                                current = 1;
                                // Set the address to use
                                ipAddresstoUse = localIPAddresses[i];
                            }
                            else if (first == (byte)169 && current < 2)
                            {
                                // We have a link local address
                                current = 2;
                                // Set the address to use
                                ipAddresstoUse = localIPAddresses[i];
                            }
                            else if (first == (byte)192 && current < 3)
                            {
                                // We have a local address
                                current = 3;
                                // Set the address to use
                                ipAddresstoUse = localIPAddresses[i];
                            }
                            else
                            {
                                // Assume the address is globally accessible and use by default.
                                ipAddresstoUse = localIPAddresses[i];
                                // Break from the loop
                                break;
                            }
                        }
                    }

                    // Now set the IP address of the address
                    //address.setIpAddress(ipAddresstoUse.ToString());

                    // Set the address part.
                    //address.setAddress(address.GetIpAddressAsString() + ":" + address.getPort());
                }

                // Connect the socket to the server socket on the remote Node
                IPAddress remoteAddress = IPAddress.Parse(address.GetIpAddressAsString());
                this.socket = new Socket(remoteAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                EndPoint remoteEndPoint = new IPEndPoint(remoteAddress, address.getPort());

                // Set TcpNoDelay. Off should improve performance for smaller packet sizes, which JCSP should have in general
                this.socket.NoDelay = !TCPIPLink.NAGLE;
                this.socket.Connect(remoteEndPoint);
                // Create the input and output streams for the Link
                this.rxStream = new BinaryReader(new NetworkStream(this.socket));
                this.txStream = new BinaryWriter(new NetworkStream(this.socket));

                // Set the remote address
                this.remoteAddress = address;

                // We are not connected, so set connected to false.
                this.connected = false;

                // Log Node connection
                Node.logger.log(this.GetType(), "Link created to " + address.toString());
            }
            catch (IOException ioe)
            {
                // Something went wrong during connection. Log and throw exception
                Node.loggerError.log(this.GetType(), "Failed to create Link to " + address.toString());
                throw new JCSPNetworkException("Failed to create TCPIPLink to: " + address.getAddress());
            }
        }

        /**
         * Creates new TCPIPLink from a Socket. This is used internally by JCSP
         * 
         * @param socket
         *            The socket to create the TCPIPLink with
         * @param nodeID
         *            The NodeID of the remote Node
         * @//throws JCSPNetworkException
         *             Thrown if there is a problem during the connection
         */
        internal TCPIPLink(Socket socket, NodeID nodeID)
        //throws JCSPNetworkException
        {
            try
            {
                this.socket = socket;
                // Set TcpNoDelay
                socket.NoDelay = !TCPIPLink.NAGLE;
                // Set the input and output streams for the Link
                //this.rxStream = new BinaryReader(new BufferedInputStream(socket.getInputStream(), TCPIPLink.BUFFER_SIZE));
                //this.txStream = new BinaryWriter(new BufferedOutputStream(socket.getOutputStream(), TCPIPLink.BUFFER_SIZE));


                //this.rxStream = new BinaryReader(new BufferedInputStream(this.socket.getInputStream(), TCPIPLink.BUFFER_SIZE));
                this.rxStream = new BinaryReader(new NetworkStream(this.socket));
                //this.txStream = new BinaryWriter(new BufferedOutputStream(this.socket.getOutputStream(), TCPIPLink.BUFFER_SIZE));
                this.txStream = new BinaryWriter(new NetworkStream(this.socket));


                this.remoteID = nodeID;
                this.remoteAddress = (TCPIPNodeAddress)this.remoteID.getNodeAddress();
                this.connected = true;
                // Log Link creation and Link connection
                Node.logger.log(this.GetType(), "Link created to " + nodeID.toString());
                Node.logger.log(this.GetType(), "Link to " + nodeID.toString() + " connected");
            }
            catch (IOException ioe)
            {
                // Something went wrong during the creation. Log and throw exception
                Node.loggerError.log(this.GetType(), "Failed to create Link to " + nodeID.toString());
                throw new JCSPNetworkException("Failed to create TCPIPLink to: " +
                                               nodeID.getNodeAddress().getAddress());
            }
        }

        /**
         * Connects the Link to the remote Node. Exchanges the NodeIDs
         * 
         * @return True if the Link successfully connected to the remote Link
         * @//throws JCSPNetworkException
         *             Thrown if something goes wrong during the connection
         */
        public override Boolean connect()
        //throws JCSPNetworkException
        {
            if (this.connected)
                return true;

            // Flag to determine if we are connected at the end of the process.
            Boolean toReturn = false;

            try
            {
                // Write the string representation of our NodeID to the remote Node
                this.txStream.Write(Node.getInstance().getNodeID().toString());
                this.txStream.Flush();

                // Read in the response from the opposite Node
                String response = this.rxStream.ReadString();

                // Either the connection has been accepted (no connection to this Node exists on the opposite Node) or
                // it has not. The opposite Node sends OK in the first instance.
                if (response.Equals("OK", StringComparison.OrdinalIgnoreCase))
                {
                    // The connection is to be kept. Log, and set toReturn to true
                    Node.logger.log(this.GetType(), "Link to " + this.remoteAddress.toString() + " connected");
                    toReturn = true;
                }

                // Read in Remote NodeID as string
                NodeID otherID = NodeID.parse(this.rxStream.ReadString());

                // First check we have a tcpip Node connection. This should always be the case
                if (otherID.getNodeAddress() is TCPIPNodeAddress)
                {
                    // Set address and NodeID. If we are not connected then this NodeID can be used to
                    // get the actual Link from the LinkManager
                    this.remoteAddress = (TCPIPNodeAddress)otherID.getNodeAddress();
                    this.remoteID = otherID;

                    // Set connected to toReturn
                    this.connected = toReturn;
                    return toReturn;
                }

                // We do not have a tcpip? Should never really happen however. Log and throw Exception
                Node.loggerError.log(this.GetType(), "Tried to connect a TCPIPLink to a non TCPIP connection");
                throw new JCSPNetworkException("Tried to connect a TCPIPLink to a non TCPIP connection");
            }
            catch (IOException ioe)
            {
                // Something went wrong during the connection process. Log and throw exception.
                Node.loggerError.log(this.GetType(), "Failed to connect TCPIPLink to: " + this.remoteAddress.getAddress());
                throw new JCSPNetworkException("Failed to connect TCPIPLink to: " + this.remoteAddress.getAddress());
            }
        }

        /**
         * Creates any required resources. For TCP/IP there is none.
         * 
         * @return True if all resources were created OK. Always the case in TCP/IP
         * @//throws JCSPNetworkException
         *             Thrown if anything goes wrong during the creation process.
         */
        protected override Boolean createResources() //TODO what's the point of that?
        //throws JCSPNetworkException
        {
            return true;
        }

        public override void destroyResources()
        {
            try
            {
                // We must ensure only one process can call destroy at any time
                lock (this)
                {
                    // Check that the socket is still in existence
                    if (this.socket != null)
                    {
                        this.txStream.Close();
                        this.rxStream.Close();
                        this.socket.Close();
                        this.socket = null;
                        // Remove the Link from the LinkManager
                        this.lostLink();
                    }
                }
            }
            catch (Exception e)
            {
                // Hopefully nothing bad has happened. If it has, we still need to
                // register the Link as lost
                this.lostLink();
            }
        }

        /**
         * Gets the NodeAddress of the Node that this Link is connected to
         * 
         * @return The NodeAddress of the remotely connected Node
         */
        public NodeAddress getRemoteAddress()
        {
            return this.remoteAddress;
        }
    }
}