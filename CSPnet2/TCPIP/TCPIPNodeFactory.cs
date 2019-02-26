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
using CSPlang;
using CSPnet2.NetNode;

namespace CSPnet2.TCPIP
{


/**
 * Used to initialise a Node. This is kept for backward compatibility. See Node for more information.
 * 
 * @see Node
 * @see NodeFactory
 * @deprecated This method of Node initialisation should no longer be used. See Node for more information
 * @author Kevin Chalmers
 */
public sealed class TCPIPNodeFactory : NodeFactory
{

    /**
     * Creates a new TCPIPNodeFactory
     * 
     * @param addr
     *            The address of the CNS / BNS
     */
    public TCPIPNodeFactory(TCPIPNodeAddress addr)
    {
        this.cnsAddress = addr;
    }

    /**
     * Creates a new TCPIPNodeFactory
     * 
     * @param serverIP
     *            The IP address of the CNS / BNS
     */
    public TCPIPNodeFactory(String serverIP)
    {
        this.cnsAddress = new TCPIPNodeAddress(serverIP, 7890);
    }

    /**
     * Initialises the Node, connecting to the CNS / BNS
     * 
     * @param node
     *            The Node to initialise
     * @return A new NodeAddress which the Node is registered at
     * @//throws JCSPNetworkException
     *             Thrown if something goes wrong during the Node initialisation process
     */
    internal override NodeAddress initNode(Node node)
        //throws JCSPNetworkException
    {
        // First install TCPIPProtocolID
        NodeAddress.installProtocol("tcpip", TCPIPProtocolID.getInstance());
        try
        {
            // Get the local IP addresses
            //InetAddress[] local = InetAddress.getAllByName(InetAddress.getLocalHost().getHostName());
            IPAddress[] local = IPAddress.getAllByName(IPAddress.getLocalHost().getHostName());
                //InetAddress toUse = InetAddress.getLocalHost();
            IPAddress toUse = IPAddress.();

            // We basically have four types of addresses to worry about. Loopback (127), link local (169),
            // local (192) and (possibly) global. Grade each 1, 2, 3, 4 and use highest scoring address. In all
            // cases use first address of that score.
            int current = 0;

            // Loop until we have checked all the addresses
            for (int i = 0; i < local.Length; i++)
            {
                // Ensure we have an IPv4 address
                if (local[i] is Inet4Address)
                {
                    // Get the first byte of the address
                    byte first = local[i].getAddress()[0];

                    // Now check the value
                    if (first == (byte)127 && current < 1)
                    {
                        // We have a Loopback address
                        current = 1;
                        // Set the address to use
                        toUse = local[i];
                    }
                    else if (first == (byte)169 && current < 2)
                    {
                        // We have a link local address
                        current = 2;
                        // Set the address to use
                        toUse = local[i];
                    }
                    else if (first == (byte)192 && current < 3)
                    {
                        // We have a local address
                        current = 3;
                        // Set the address to use
                        toUse = local[i];
                    }
                    else
                    {
                        // Assume the address is globally accessible and use by default.
                        toUse = local[i];
                        // Break from the loop
                        break;
                    }
                }
            }

                // Create a new ServerSocket listening on this address
            TcpClient serv = new TcpClient(0, 10, toUse);
            //TcpListener serv = new TcpListener(0, 10, toUse);
            //Socket serv = new Socket(0, 10, toUse);
            //ServerSocket serv = new ServerSocket(0, 10, toUse);

            // Create the local address
            //TCPIPNodeAddress localAddr = new TCPIPNodeAddress(toUse.getHostAddress(), serv.getLocalPort());
            TCPIPNodeAddress localAddr = new TCPIPNodeAddress(toUse.getHostAddress(), serv.Client.LocalEndPoint);

            // Create and start the LinkServer
            TCPIPLinkServer server = new TCPIPLinkServer(serv);
            new ProcessManager(server).start();

            // Return the NodeAddress
            return localAddr;
        }
        catch (UnknownHostException uhe)
        {
            throw new JCSPNetworkException("Failed to start TCPIPLinkServer.  Could not get local IP address.\n"
                                           + uhe.getMessage());
        }
        catch (IOException ioe)
        {
            throw new JCSPNetworkException("Failed to open new Server Socket.\n" + ioe.Message);
        }
    }
}
}