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
using System.Collections;
using CSPlang;
using CSPnet2.Net2Link;
using CSPnet2.TCPIP;

namespace CSPnet2.NetNode
{
/**
 * This abstract class defines encapsulates the address of a Node within a JCSP networked system. Specific protocols
 * must provide concrete implementations of this class to allow Node initialisation and connection. One concrete example
 * is provided in the jcsp.net2.tcpip package.
 * 
 * @see Node
 * @author Kevin Chalmers (updated from Quickstone Technologies)
 */

    [Serializable]
    public abstract class NodeAddress : IComparable<NodeAddress>
    {
        /**
         * String representing the protocol in used
         */
        protected String protocol;

        /**
         * String representation of the address
         */
        protected String address;

        /**
         * The table of installed protocols on this Node
         */
        private static Hashtable installedProtocols = new Hashtable();

        /**
         * Gets the string representing the protocol
         * 
         * @return The String representation of the protocol part of the NodeAddress
         */
        public String getProtocol()
        {
            return this.protocol;
        }

        /**
         * Gets a string representing the address
         * 
         * @return The String representation of the address part of the NodeAddress
         */
        public String getAddress()
        {
            return this.address;
        }

        /**
         * Converts the NodeAddress into a String. The form is [protocol]\\[address]
         * 
         * @return A String representation of this NodeAddress
         */
        public String toString()
        {
            return this.protocol + "\\\\" + this.address;
        }

        /**
         * Gets the hash code of this object
         * 
         * @return Hashcode for this NodeAddress
         */
        public int hashCode()
        {
            return this.address.GetHashCode();
        }

        /**
         * Checks if this NodeAddress is equal to another
         * 
         * @param obj
         *            The NodeAddress to compare to
         * @return True if object is equal to this NodeAddress, false otherwise
         */
        public Boolean equals( /*final*/ Object obj)
        {
            if (!(obj is NodeAddress))
                return false;
            NodeAddress other = (NodeAddress) obj;
            return (this.protocol.Equals(other.protocol) && this.address.Equals(other.address));
        }

        /**
         * Compares this NodeAddress to another
         * 
         * @param arg0
         *            The NodeAddress to compare to
         * @return 1 if object is greater than this one, 0 if they are equal, -1 otherwise
         */


        /*public int compareTo(/*final#1# Object arg0)
            {
                if (!(arg0 is NodeAddress))
                    return -1;
                NodeAddress other = (NodeAddress)arg0;
                if (!this.protocol.Equals(other.protocol))
                    return this.protocol.CompareTo(other.protocol);
                return this.address.CompareTo(other.address);
            }*/

        /**
         * Creates a Link connected to this address
         * 
         * @return A new Link connected to this address
         * @//throws JCSPNetworkException
         *             If something goes wrong during the creation of the Link
         */
        /// <summary>what this does</summary>
        /// <exception cref="JCSPNetworkException">some scenario</exception>
        internal abstract Link createLink();

        /**
         * Creates a LinkServer listening on this address
         * 
         * @return A new LinkServer listening on this address
         * @//throws JCSPNetworkException
         *             If something goes wrong during the creation of the LinkServer
         */

        /// <summary>what this does</summary>
        /// <exception cref="JCSPNetworkException">some scenario</exception>
        internal abstract LinkServer createLinkServer();

        /**
         * Retrieves the correct protocol handler for the implemented address type. This is used during Node initialisation
         * 
         * @return the ProtocolID for this address type
         */
        internal abstract ProtocolID getProtocolID();

        /**
         * Parses a string representation of a NodeAddress back to its object form
         * 
         * @param str
         *            The string to parse
         * @return A new NodeAddress created from a String form
         * @//throws ArgumentException 
         *             Thrown if the string is not for a recognised protocol.
         */


        public static NodeAddress parse(String str)
            //throws ArgumentException 
        {
            int index = str.IndexOf("\\");
            ProtocolID protocol = (ProtocolID)NodeAddress.installedProtocols[str.Substring(0, index)];
            //if (protocol != null)
            //{
            //    return protocol.parse(str.Substring(index + 4));
            //}

            return protocol.parse(str);

            //String[] strings = str.Split(@"\\\\");

            //if (strings[0].Equals("tcpip", StringComparison.OrdinalIgnoreCase))
            //{
            //    String[] addressStrings = strings[1].Split(":");
            //    return new TCPIPNodeAddress(addressStrings[0], Int32.Parse(addressStrings[1]));
            //}

            throw new ArgumentException("Unknown protocol used for parsing NodeAddress");
        }

        /**
         * Installs a new Protocol on the Node
         * 
         * @param name
         *            Name of the protocol to install
         * @param protocol
         *            ProtocolID installed
         */
        public /*synchronized*/ static void installProtocol(String name, ProtocolID protocol)
        {
            if (!NodeAddress.installedProtocols.ContainsKey(name))
                NodeAddress.installedProtocols.Add(name, protocol);
            // If the protocol is already installed, we do nothing.
        }

        public int CompareTo(NodeAddress other)
        {
            //if (!(arg0 is NodeAddress))
            //    return -1;

            if (!this.protocol.Equals(other.protocol))
                return this.protocol.CompareTo(other.protocol);
            return this.address.CompareTo(other.address);
        }
    }
}