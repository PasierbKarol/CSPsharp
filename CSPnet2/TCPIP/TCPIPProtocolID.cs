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
using CSPnet2;
using CSPnet2.NetNode;

namespace CSPnet2.TCPIP
{

/**
 * Concrete implementation of a ProtocolID used to parse a string representation of a TCPIPNodeAddress into a
 * TCPIPNodeAddress object.
 * 
 * @author Kevin Chalmers
 */
public sealed class TCPIPProtocolID : ProtocolID
{
    /**
     * Singleton instance of this class
     */
    private static TCPIPProtocolID instance = new TCPIPProtocolID();

    /**
     * Gets the singleton instance of this class
     * 
     * @return A new singleton instance of this class
     */
    public static TCPIPProtocolID getInstance()
    {
        return instance;
    }

    /**
     * Default private constructor
     */
    private TCPIPProtocolID()
    {
        // Empty constructor
    }

    /**
     * Parses a string to recreate a TCPIPNodeAddress object
     * 
     * @param addressString
     *            String representing the address
     * @return A new TCPIPNodeAddress object
     * @//throws ArgumentException 
     *             Thrown if the address is not in a correct form
     */
    internal override NodeAddress parse(String addressString)
       // //throws ArgumentException 
    {
        // Split address into IP and port
        int index = addressString.IndexOf("\\\\");
        String temp = addressString.Substring(index + 2);
        index = temp.IndexOf(":");
        String address = temp.Substring(0, index);
        int port = Int32.Parse(temp.Substring(index + 1, temp.Length));
        return new TCPIPNodeAddress(address, port);
    }

}
}