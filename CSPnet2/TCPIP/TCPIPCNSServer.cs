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
using CSPlang;
using CSPnet2.NetNode;

namespace CSPnet2.TCPIP
{

//    import java.net.Inet4Address;
//import java.net.InetAddress;


/**
 * This is the original (now deprecated) server program for use by
 * 
 * @author Kevin Chalmers (updated from Quickstone Technologies)
 * @deprecated Use TCPIPNodeFactory instead
 */
public sealed class TCPIPCNSServer
{
    /**
     * @param args
     * @//throws Exception
     */
    public static void main(String[] args)
        //throws Exception
    {
        // Get the local IP addresses
        //InetAddress[] local = InetAddress.getAllByName(InetAddress.getLocalHost().getHostName());
        InetAddress[] local = InetAddress.getAllByName(InetAddress.getLocalHost().getHostName());
        //InetAddress toUse = InetAddress.getLocalHost();
        InetAddress toUse = InetAddress.getLocalHost();

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

        // Create a local address object
        TCPIPNodeAddress localAddr = new TCPIPNodeAddress(toUse.getHostAddress(), 7890);
        // Initialise the Node
        Node.getInstance().init(localAddr);
        // Start CNS and BNS
        IamCSProcess[] processes = { CNS.CNS.getInstance(), BNS.BNS.getInstance() };
        new CSPParallel(processes).run();
    }
}
}