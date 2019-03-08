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
using System.Net;
using CSPnet2.Net2Link;
using CSPnet2.NetNode;

namespace CSPnet2.TCPIP
{
/**
 * A concrete implementation of a NodeAddress that is designed for TCP/IP connections.
 * 
 * @see NodeAddress
 * @author Kevin Chalmers
 */
    public sealed class TCPIPNodeAddress : NodeAddress
    {
        /**
         * The SUID for this class
         */
        private static readonly long serialVersionUID = 1L;

        /**
         * The IP address part of the address
         */
        private String ip;
        //private IPAddress ipAddress;

        /**
         * The port part of the address
         */
        private int port;

        /**
         * Creates a new TCPIPNodeAddress from an IP address and port
         * 
         * @param ipAddress
         *            The IP address part of the NodeAddress
         * @param portNumber
         *            The port number part of the NodeAddress
         */


/*        public TCPIPNodeAddress(String ipAddress, EndPoint portNumber)
        {
            this.ip = ipAddress;
            this.port = Int32.Parse(portNumber.AddressFamily.ToString());
            this.protocol = "tcpip";
            this.address = ipAddress + ":" + portNumber;
        }*/

        public TCPIPNodeAddress(String ipAddress, int portNumber)
        {
            this.ip = ipAddress;
            this.port = portNumber;
            this.protocol = "tcpip";
            this.address = ipAddress + ":" + portNumber;
        }

        /**
         * Creates a new TCPIPNodeAddress using the local IP address and a given port number. Allows a
         * 
         * @param portNumber
         *            The port number to use
         */
        public TCPIPNodeAddress(int portNumber)
        {
            this.port = portNumber;
            this.ip = "";
            this.protocol = "tcpip";
        }

        /**
         * Creates a new TCPIPNodeAddress
         */
        public TCPIPNodeAddress()
        {
            this.port = 0;
            this.ip = "";
            this.protocol = "tcpip";
        }

        /**
         * Gets the port number part of this address
         * 
         * @return The port number part of the address
         */
        public /*final*/ int getPort()
        {
            return this.port;
        }

        /**
         * Sets the port part of the address. Used internally in JCSP
         * 
         * @param portNumber
         *            The port number to use
         */
        internal void setPort(int portNumber)
        {
            this.port = portNumber;
        }

        /**
         * Gets the IP address part of the address
         * 
         * @return The IP Address part of the address
         */
        public /*final*/ String GetIpAddressAsString()
        {
            return this.ip;
        }

/*        public /*final#1# IPAddress GetIpAddress()
        {
            return this.ipAddress;
        }*/

        /**
         * Sets the IP address part of the NodeAddress. Used internally in JCSP
         * 
         * @param ipAddr
         *            The IP address to use
         */
        internal void setIpAddress(String ipAddr)
        {
            this.ip = ipAddr;
        }

/*        internal void setIpAddress(IPAddress ipAddr)
        {
            this.ipAddress = ipAddr;
        }*/

        /**
         * Sets the address String. Used internally within JCSP
         * 
         * @param str
         *            The String to set as the address
         */
        internal void setAddress(String str)
        {
            this.address = str;
        }

        /**
         * Creates a new TCPIPLink connected to a Node with this address
         * 
         * @return A new TCPIPLink connected to this address
         * @//throws JCSPNetworkException
         *             Thrown if something goes wrong during the creation of the Link
         */
        internal override Link createLink()
            //throws JCSPNetworkException
        {
            return new TCPIPLink(this);
        }

        /**
         * Creates a new TCPIPLinkServer listening on this address
         * 
         * @return A new TCPIPLinkServer listening on this address
         * @//throws JCSPNetworkException
         *             Thrown if something goes wrong during the creation of the LinkServer
         */
        internal override LinkServer createLinkServer()
            //throws JCSPNetworkException
        {
            return new TCPIPLinkServer(this);
        }

        /**
         * Returns the TCPIPProtocolID
         * 
         * @return TCPIPProtocolID
         */
        internal override ProtocolID getProtocolID()
        {
            return TCPIPProtocolID.getInstance();
        }
    }
}