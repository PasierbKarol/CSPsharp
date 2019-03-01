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
        readonly EndPoint remotEndPoint; //Karol Pasierb
        public static ManualResetEvent allDone = new ManualResetEvent(false); //Karol Pasierb
        private static Socket incoming = null;

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
            //TODO implement later - Karol Pasierb
            //this.listeningAddress = new TCPIPNodeAddress(serverSocket.getInetAddress().getHostAddress(), serverSocket.getLocalPort());
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
                if (address.GetIpAddressAsString().Equals(""))
                {
                    IPAddress localIPAddresstoUse = IPAddressGetterForNET2.GetOnlyLocalIPAddress();
                    IPAddress[] localIPAddresses = IPAddressGetterForNET2.GetAllLocalAddresses();
                    address.setIpAddress(localIPAddresstoUse);
                    // Get the local IP addresses
                    //InetAddress[] local = InetAddress.getAllByName(InetAddress.getLocalHost().getHostName());
                    //InetAddress toUse = InetAddress.getLocalHost();


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
                    address.setIpAddress(localIPAddresstoUse);

                    // Set the address part now, but it may change if we have to get a port number
                    address.setAddress(address.GetIpAddressAsString() + ":" + address.getPort());
                }

                // Now check if the address has a port number
                if (address.getPort() == 0)
                {
                    // No port number supplied. Get one as we create the ServerSocket
                    //InetAddress socketAddress = InetAddress.getByName(address.GetIpAddressAsString());
                    //SocketAddress socketAddress = new SocketAddress(AddressFamily.InterNetwork);
                    IPEndPoint socketEndPoint = new IPEndPoint(IPAddress.Parse(address.GetIpAddressAsString()), 0); //port 0 as in Java's implementation
                    IPAddress serverAddress = IPAddress.Parse(address.GetIpAddressAsString());


                    // Create the server socket with a random port
                    //this.serv = new ServerSocket(0, 0, socketAddress);
                    //this.serv = new TcpListener(localIPAddresstoUse.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    this.serv = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
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
                    //InetAddress inetAddress = InetAddress.getByName(address.GetIpAddressAsString());
                    IPEndPoint inetAddress = new IPEndPoint(IPAddress.Parse(address.GetIpAddressAsString()), address.getPort()); 

                    // Now create the ServerSocket
                    //this.serv = new ServerSocket(address.getPort(), 10, inetAddress);
                    //IPEndPoint ipEndpoint = new IPEndPoint(localIPAddresstoUse, address.getPort());
                    this.serv = new Socket(inetAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    //Bind server to the ip address - Karol Pasierb
                    this.serv.Bind(inetAddress);
                    this.serv.Listen(10); //backlog 10 for the queue  - Karol Pasierb

                    // Set listeningAddress
                    this.listeningAddress = address;
                    this.remotEndPoint = inetAddress;
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
            Node.log.log(this.GetType(), "TCPIP Link Server started on " + this.listeningAddress.getAddress());
            try
            {
                // Now we loop until something goes wrong
                while (true)
                {
                    // Receive incoming connection
                    //Socket incoming = this.serv.accept();
                    Socket incoming = this.serv.Accept();
                    //var ir = new AsyncCallback(AcceptCallback);
                    /*Socket incoming = */ //this.serv.BeginAccept(ir, this.serv );
                    IPEndPoint remoteEndPoint = (IPEndPoint)incoming.RemoteEndPoint;
                    Console.WriteLine("Accepted connection from {0}:{1}.", remoteEndPoint.Address, remoteEndPoint.Port);


                    // Log
                    Node.log.log(this.GetType(), "Received new incoming connection");
                    Console.WriteLine("\n\nReceived new incoming connection");
                    // Set TcpNoDelay
                    //incoming.setTcpNoDelay(true);
                    incoming.NoDelay = true;

                    // Now we want to receive the connecting Node's NodeID
                    //BinaryReader inStream = new BinaryReader(incoming.getInputStream());
                    NetworkStream networkStream = new NetworkStream(incoming);
                    //BinaryReader inStream = new BinaryReader(networkStream).ReadBytes(100);

                    // Receive remote NodeID and parse
                    String otherID = new BinaryReader(networkStream).ReadString(); //https://stackoverflow.com/questions/10810479/what-does-binaryreader-do-if-the-bytes-i-am-reading-arent-present-yet
                    Console.WriteLine("\n\nRead otherID  " + otherID);
                    NodeID remoteID = NodeID.parse(otherID);

                    // First check we have a tcpip Node connection
                    if (remoteID.getNodeAddress() is TCPIPNodeAddress)
                    {
                        // Create an output stream from the Socket
                        //BinaryWriter outStream = new BinaryWriter(incoming.getOutputStream());
                        BinaryWriter outStream = new BinaryWriter(networkStream);

                        // Now Log that we have received a connection
                        Node.log.log(this.GetType(), "Received connection from: " + remoteID.toString());
                        Console.WriteLine("\n\nReceived connection from: " + remoteID.toString());

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
                            Node.log.log(this.GetType(), "Connection to " + remoteID
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
                        incoming.Close();
                }
            }
            catch (IOException ioe)
            {
                // We can't really recover from this. This may happen if the network connection was lost.
                // Log and fail
                Node.err.log(this.GetType(), "TCPIPLinkServer failed.  " + ioe.Message);
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            incoming = handler;


            IPEndPoint remoteEndPoint = (IPEndPoint)incoming.RemoteEndPoint;
            Console.WriteLine("Accepted connection from {0}:{1}.", remoteEndPoint.Address, remoteEndPoint.Port);

            //Socket incoming = this.serv;
            


            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);
                    // Echo the data back to the client.  
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }
}