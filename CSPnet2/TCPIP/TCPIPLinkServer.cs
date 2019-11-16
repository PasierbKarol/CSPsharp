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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CSPlang;
using CSPnet2.Net2Link;
using CSPnet2.NetNode;

namespace CSPnet2.TCPIP
{
/**
 * Concrete implementation of a LinkServer that listens on a TCP/IP based ServerSocket. For information on LinkServer,
 * see the relevant documentation.
 * <p>
 * It is possible for an advanced user to create this object themselves, although it is not recommended. For example:
 * </p>
 * <p>
 * <code>
 * TCPIPLinkServer serv = new TCPIPLinkServer(address);<br>
 * new ProcessManager(serv).start();
 * </code>
 * </p>
 * <p>
 * This is done automatically during Node initialisation. However, if the machine used has multiple interfaces, this can
 * be used to listen on another interface also.
 * </p>
 * 
 * @see LinkServer
 * @author Kevin Chalmers
 */
    public sealed class TCPIPLinkServer : LinkServer
    {
        /**
         * The ServerSocket that this class wraps around. The process listens on this connection
         */
        //private readonly ServerSocket serv;
        private readonly Socket serv;
        //private readonly TcpListener serv;

        /**
         * The NodeAddress that this LinkServer is listening on. This should be the same as the Node's address.
         */
        readonly TCPIPNodeAddress listeningAddress;

        /**
         * Creates LinkServer by wrapping round an existing ServerSocket. Used internally by JCSP
         * 
         * @param serverSocket
         *            The ServerSocket to create the LinkServer with
         */
        //internal TCPIPLinkServer(ServerSocket serverSocket)
        internal TCPIPLinkServer(Socket serverSocket)
        {
            // We need to set the NodeAddress. Create from ServerSocket address and port
            //TODO double check if thee code below provides the proper values - Karol Pasierb
            //this.listeningAddress = new TCPIPNodeAddress(serverSocket.getInetAddress().getHostAddress(), serverSocket.getLocalPort());
            IPEndPoint a = (IPEndPoint) serverSocket.LocalEndPoint;

            this.listeningAddress = new TCPIPNodeAddress(a.Address.ToString(), a.Port);
            this.serv = serverSocket;
        }

        /**
         * Creates a new TCPIPLinkServer listening on the given address
         * 
         * @param address
         *            The address to listen on for new connections
         * @//throws JCSPNetworkException
         *             Thrown if something goes wrong during the creation of the ServerSocket
         */
        public TCPIPLinkServer(TCPIPNodeAddress address)
            //throws JCSPNetworkException
        {
            try
            {
                // First check if we have an ip address in the string
                if (String.IsNullOrEmpty(address.GetIpAddressAsString()))
                {
                    IPAddress localIPAddresstoUse = GetLocalIPAddress.GetOnlyLocalIPAddress();
                    IPAddress[] localIPAddresses = GetLocalIPAddress.GetAllAddresses();
                    address.setIpAddress(GetLocalIPAddress.ConvertIPAddressToString(localIPAddresstoUse));
                    // Get the local IP addresses
                    //InetAddress[] local = InetAddress.getAllByName(InetAddress.getLocalHost().getHostName());
                    //InetAddress toUse = InetAddress.getLocalHost();


                    //TODO the code below is not updated and not working correctly. 
                    // We basically have four types of addresses to worry about. Loopback (127), link local (169),
                    // local (192) and (possibly) global. Grade each 1, 2, 3, 4 and use highest scoring address. In all
                    // cases use first address of that score.
                    int current = 0;

                    // Loop until we have checked all the addresses
                    for (int i = 0; i < localIPAddresses.Length; i++)
                    {
                        // Ensure we have an IPv4 address
                        //if (local[i] is Inet4Address)
                        if (localIPAddresses[i] is IPAddress)
                        {
                            // Get the first byte of the address
                            //byte first = local[i].getAddress()[0];
                            byte first = localIPAddresses[i].GetAddressBytes()[0];

                            // Now check the value
                            if (first == (byte) 127 && current < 1)
                            {
                                // We have a Loopback address
                                current = 1;
                                // Set the address to use
                                localIPAddresstoUse = localIPAddresses[i];
                            }
                            else if (first == (byte) 169 && current < 2)
                            {
                                // We have a link local address
                                current = 2;
                                // Set the address to use
                                localIPAddresstoUse = localIPAddresses[i];
                            }
                            else if (first == (byte) 192 && current < 3)
                            {
                                // We have a local address
                                current = 3;
                                // Set the address to use
                                localIPAddresstoUse = localIPAddresses[i];
                            }
                            else
                            {
                                // Assume the address is globally accessible and use by default.
                                localIPAddresstoUse = localIPAddresses[i];
                                // Break from the loop
                                break;
                            }
                        }
                    }

                    // Now set the IP address of the address
                    //address.setIpAddress(toUse.getHostAddress());
                    //address.setIpAddress(localIPAddresstoUse.ToString());

                    // Set the address part now, but it may change if we have to get a port number
                    //address.setAddress(address.GetIpAddressAsString() + ":" + address.getPort());
                }

                // Now check if the address has a port number
                if (address.getPort() == 0)
                {
                    // No port number supplied. Get one as we create the ServerSocket
                    //InetAddress socketAddress = InetAddress.getByName(address.GetIpAddressAsString());
                    //SocketAddress socketAddress = new SocketAddress(AddressFamily.InterNetwork);
                    IPEndPoint socketEndPoint =
                        new IPEndPoint(IPAddress.Parse(address.GetIpAddressAsString()),
                            0); //port 0 as in Java's implementation

                    // Create the server socket with a random port
                    //this.serv = new ServerSocket(0, 0, socketAddress);
                    //this.serv = new TcpListener(localIPAddresstoUse.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    this.serv = new Socket(socketEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    this.serv.Bind(socketEndPoint);
                    this.serv.Listen(0);


                    // Assign the port to the address
                    //address.setPort(this.serv.getLocalPort());
                    address.setPort(socketEndPoint.Port);

                    // And set the address
                    address.setAddress(address.GetIpAddressAsString() + ":" + address.getPort());

                    // Set the listening address
                    this.listeningAddress = address;
                }
                else
                {
                    // Create an IP address from the NodeAddress
                    IPEndPoint inetAddress = new IPEndPoint(IPAddress.Parse(address.GetIpAddressAsString()),
                        address.getPort());

                    // Now create the ServerSocket
                    this.serv = new Socket(inetAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    //Bind server to the ip address - Karol Pasierb
                    this.serv.Bind(inetAddress);
                    this.serv.Listen(10); //backlog 10 for the queue  - Karol Pasierb

                    // Set listeningAddress
                    this.listeningAddress = address;
                }
            }
            catch (IOException ioe)
            {
                throw new JCSPNetworkException("Failed to create TCPIPLinkServer on: " + address.getAddress());
            }
        }

        /**
         * The run method for the TCPIPLinkServer process
         */
        public override void run()
        {
            // Log start of Link Server
            Node.logger.log(this.GetType(), "TCPIP Link Server started on " + this.listeningAddress.getAddress());
            try
            {
                // Now we loop until something goes wrong
                while (true)
                {
                    // Receive incoming connection
                    Socket incoming = this.serv.Accept();

                    //IPEndPoint remoteEndPoint = (IPEndPoint) incoming.RemoteEndPoint;
                    //Debug.WriteLine("Accepted connection from {0}:{1}.", remoteEndPoint.Address, remoteEndPoint.Port);

                    // Log
                    Node.logger.log(this.GetType(), "Received new incoming connection");
                    // Set TcpNoDelay
                    incoming.NoDelay = true;

                    // Now we want to receive the connecting Node's NodeID
                    NetworkStream networkStream = new NetworkStream(incoming);

                    // Receive remote NodeID and parse
                    String otherID = new BinaryReader(networkStream).ReadString();
                    NodeID remoteID = NodeID.parse(otherID);

                    // First check we have a tcpip Node connection
                    if (remoteID.getNodeAddress() is TCPIPNodeAddress)
                    {
                        // Create an output stream from the Socket
                        BinaryWriter outStream = new BinaryWriter(networkStream);

                        // Now Log that we have received a connection
                        Node.logger.log(this.GetType(), "Received connection from: " + remoteID.toString());

                        // Check if already connected
                        if (requestLink(remoteID) == null)
                        {
                            // No existing connection to incoming Node exists. Keep connection

                            // Write OK to the connecting Node
                            outStream.Write("OK");
                            outStream.Flush();

                            // Send out our NodeID
                            outStream.Write(Node.getInstance().getNodeID().toString());
                            outStream.Flush();

                            // Create Link, register, and start.
                            TCPIPLink link = new TCPIPLink(incoming, remoteID);
                            registerLink(link);
                            new ProcessManager(link).start();
                        }
                        else
                        {
                            // We already have a connection to the incoming Node

                            // Log failed connection
                            Node.logger.log(this.GetType(), "Connection to " + remoteID
                                                                          + " already exists.  Informing remote Node.");

                            // Write EXISTS to the remote Node
                            outStream.Write("EXISTS");
                            outStream.Flush();

                            // Send out NodeID. We do this so the opposite Node can find its own connection
                            outStream.Write(Node.getInstance().getNodeID().toString());
                            outStream.Flush();

                            // Close socket
                            incoming.Close();
                        }
                    }

                    // Address is not a TCPIP address. Close socket. This will cause an exception on the opposite Node
                    else
                    {
                        incoming.Close();
                    }
                }
            }
            catch (IOException ioe)
            {
                // We can't really recover from this. This may happen if the network connection was lost.
                // Log and fail
                Node.loggerError.log(this.GetType(), "TCPIPLinkServer failed.  " + ioe.Message);
            }
        }
    }
}