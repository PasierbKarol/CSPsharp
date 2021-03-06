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
using System.Runtime.InteropServices;
using CSPlang;


namespace CSPnet2.NetNode
{
    /**
     * This class is used to uniquely identify a Node within the entire JCSP network of Nodes in operation. This is to allow
     * ease to identify individual Nodes when IDs come in, and to quickly find them within tables of other Links to allow
     * usage of existing connections. This is different to a NodeAddress, which is a symbolic name representing a Node, and
     * which therefore may be repeated. The hope here is that by using enough pieces of data the Node should have a unique
     * identification. This is done by gathering the information on the current system time in milliseconds, the current
     * free memory of the JVM, the hash code of a newly created object, the name of the Node, if there is one, and the
     * address of the Node itself. Having this much information should provide us with a unique ID. Other implementations of
     * the protocol can use other means of identifying a Node uniquely, but they must use the same amount of data, e.g.
     * string(number 64 bits) - string(number 64 bits) - string (number 32 bits) - string - address string, when
     * communicating with another JCSP Node for the sake of compatibility.
     * 
     * @author Kevin Chalmers
     */

    [Serializable]
    public sealed class NodeID : IComparable<NodeID>
    {
        /**
         * The SUID of this class
         */
        private static readonly long serialVersionUID = 1L;

        /**
         * Current time in milliseconds
         */
        private readonly long time;

        /**
         * Current amount of free memory to the JVM
         */
        private readonly long mem;

        /**
         * Hash code of a object
         */
        private readonly int hashCode;

        /**
         * Name of the Node
         */
        private readonly String name;

        /**
         * Address of the Node
         */
        private readonly NodeAddress address;

        /**
         * Constructor taking the name and the address of the Node
         * 
         * @param nodeName
         *            Symbolic name of the Node
         * @param nodeAddress
         *            Symbolic address of the Node
         */
        internal NodeID(String nodeName, NodeAddress nodeAddress)
        {
            this.time = CSTimer.CurrentTimeMillis();
            //this.mem = System.Runtime.GetRuntime().freeMemory();
            //this.mem = GC.GetTotalMemory(true);
            this.mem = Process.GetCurrentProcess().PrivateMemorySize64;
            //this.hashCode = new Object().hashCode();
            this.hashCode = new Object().GetHashCode();
            this.name = nodeName;
            this.address = nodeAddress;
        }

        /**
         * Constructor taking the full details for a remote Node connection
         * 
         * @param long1
         *            The time component of the remote Node
         * @param long2
         *            The memory component of the remote Node
         * @param int1
         *            The hashCode component of the remote Node
         * @param nodeName
         *            The name component of the remote Node
         * @param nodeAddress
         *            The NodeAddress component of the remote Node
         */
        public NodeID(long long1, long long2, int int1, String nodeName, NodeAddress nodeAddress)
        {
            this.time = long1;
            this.mem = long2;
            this.hashCode = int1;
            this.name = nodeName;
            this.address = nodeAddress;
        }

        /**
         * Compares this NodeID with another NodeID.
         * 
         * @param arg0
         * @return -1, 0 or 1 if less than, equal, or greater than the other NodeID
         */
        //public int compareTo(/*final*/ Object arg0)
        //{

        //}

        /**
         * Checks if the given object is equal to this NodeID
         * 
         * @param arg0
         * @return True if equal, false otherwise
         */
        public Boolean equals(NodeID arg0)
        {
            return this.CompareTo(arg0) == 0;
        }

        /**
         * Returns the hashCode for this object
         * 
         * @return Hashcode for the NodeID
         */
        public int HashCode()
        {
            return this.hashCode;
        }

        /**
         * Converts the NodeID into a string for communication with other implementations, or for display purposes.
         * 
         * @return String representation of the NodeID
         */
        public String toString()
        {
            return this.time + "-" + this.mem + "-" + this.hashCode + "-" + this.name + "-" + this.address.toString();
        }

        /**
         * Gets the NodeAddress part of the NodeID
         * 
         * @return The NodeAddress part of the NodeID
         */
        public NodeAddress getNodeAddress()
        {
            return this.address;
        }

        /**
         * Converts a string representation of a NodeID back to a NodeID object
         * 
         * @param str
         *            The string version of a NodeID
         * @return A new NodeID created from the String representation
         */
        public static NodeID parse(String str)
        {
            // Split the string into its separate parts
            /*        String[] pieces = new String[5];
                    int index = 0;
                    int last = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        index = str.IndexOf("-", index + 1);
                        if (index == -1)
                            index = str.Length;
                        pieces[i] = str.Substring(last, index);
                        last = index + 1;
                    }*/
            String[] pieces = str.Split("-");


            // Get the relevant parts
            long time = Int64.Parse(pieces[0]);
            long mem = Int64.Parse(pieces[1]);
            int hashCode = Int32.Parse(pieces[2]);
            String name = pieces[3];
            // Parse the address
            NodeAddress addr = NodeAddress.parse(pieces[4]);

            // Return the NodeID
            return new NodeID(time, mem, hashCode, name, addr);
        }

        public int CompareTo(NodeID that)
        {
            // Check if other object is a NodeID. If not throw exception
            //if (!(arg0 is NodeID))
            //    throw new ArgumentException("Attempting to compare NodeID to an object that is not a NodeID");

            // Compare to other NodeID values
            if (that.time < this.time)
                return 1;
            else if (that.time > this.time)
                return -1;
            else
            {
                // Time part is equal
                if (that.mem < this.mem)
                    return 1;
                else if (that.mem > this.mem)
                    return -1;
                else
                {
                    // Memory part is equal
                    if (that.hashCode < this.hashCode)
                        return 1;
                    else if (that.hashCode > this.hashCode)
                        return -1;
                    else
                    {
                        // Hashcode part is equal
                        if (!(that.name.Equals(this.name)))
                            return this.name.CompareTo(that.name);
                        return this.address.CompareTo(that.address);
                    }
                }
            }

            return 0;
        }
    }
}