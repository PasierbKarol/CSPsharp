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
using CSPnet2.NetNode;

namespace CSPnet2.NetChannels
{
    /**
     * This class is a data structure representing the location of a NetChannelInput in a network. The NetChannelLocation
     * consists of the NodeID of the Node on which the NetChannelInput resides, and its Virtual Channel Number, which is the
     * number uniquely identifying the NetChannelInput on said Node.
     * <p>
     * To acquire the NetChannelLocation of a NetBarrier, use the getLocation method:
     * </p>
     * <p>
     * <code>
     * NetChannelLocation location = (NetChannelLocation)chan.getLocation();
     * </code>
     * </p>
     * <p>
     * The location returned depends on whether the channel is a NetChannelInput or a NetChannelOutput end. An input end
     * will return its own location. An output end will return the location of the input end it is connected to. This is
     * because we consider a networked channel to be a single, virtual construct, with only one location. That location is
     * where the input end is located.
     * </p>
     * 
     * @see NetChannelInput
     * @see NetChannelOutput
     * @see NetLocation
     * @author Kevin Chalmers
     */

    [Serializable]
    public sealed class NetChannelLocation : NetLocation
    {
        /**
         * The SUID representing this class
         */
        private static readonly long serialVersionUID = 1L;

        /**
         * The NodeID portion of the location
         */
        private readonly NodeID nodeID;

        /**
         * The vcn portion of the location
         */
        private readonly int vcn;

        /**
         * Creates a new NetChannelLocation
         * 
         * @param aNodeID
         *            The NodeID part of the location
         * @param aVCN
         *            The vcn part of the location
         */
        public NetChannelLocation(NodeID aNodeID, int aVCN)
        {
            this.nodeID = aNodeID;
            this.vcn = aVCN;
        }

        /**
         * Gets the NodeID part of the location
         * 
         * @return The NodeID part of the NetChannelLocation
         */
        public override NodeID getNodeID()
        {
            return this.nodeID;
        }

        /**
         * Gets the NodeAddress part of the location
         * 
         * @return The NodeAddress part of the NetChannelLocation
         */
        public override NodeAddress getNodeAddress()
        {
            return this.nodeID.getNodeAddress();
        }

        /**
         * Gets the vcn part of the location
         * 
         * @return The VCN part of the NetChannelLocation
         */
        public int getVCN()
        {
            return this.vcn;
        }

        /**
         * Converts the NetChannelLocation object into a string representation of the form ncl://[NodeID]/[VCN]
         * 
         * @return The String form of the NetChannelLocation
         */
        public String toString()
        {
            return "ncl://" + this.nodeID.toString() + "/" + this.vcn;
        }

        /**
         * Converts the string form of a NetChannelLocation back into its object form
         * 
         * @param str
         *            The string representation of a NetChannelLocation
         * @return A new NetChannelLocation created from the String representation
         */
        public static NetChannelLocation parse(String str)
        {
            NodeID nodeID = null;
            int vcn = 0;
            try
            {
                if (str.Equals("null", StringComparison.OrdinalIgnoreCase))
                    return null;
                if (str.StartsWith("ncl://"))
                {
                    String toParse = str.Substring(6);
                    int index = toParse.IndexOf("/");
                    nodeID = NodeID.parse(toParse.Substring(0, index));
                    vcn = Int32.Parse(toParse.Substring(index + 1));
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e + "String is not a string form of a NetChannelLocation");
            }

            return new NetChannelLocation(nodeID, vcn);
        }
    }
}