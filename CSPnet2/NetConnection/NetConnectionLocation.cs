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

namespace CSPnet2.NetConnection
{

/**
 * @author Kevin
 */

[Serializable]
public sealed class NetConnectionLocation : NetLocation
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
     * The vconnn portion of the location
     */
    private readonly int vconnn;

    /**
     * Creates a new NetConnectionLocation
     * 
     * @param aNodeID
     *            The NodeID part of the location
     * @param aVConnN
     *            The vconnn part of the location
     */
    public NetConnectionLocation(NodeID aNodeID, int aVConnN)
    {
        this.nodeID = aNodeID;
        this.vconnn = aVConnN;
    }

    /**
     * Gets the NodeID part of the location
     * 
     * @return The NodeID part of the NetConnectionLocation
     */
    public override NodeID getNodeID()
    {
        return this.nodeID;
    }

    /**
     * Gets the NodeAddress part of the location
     * 
     * @return The NodeAddress part of the NetConnectionLocation
     */
    public override NodeAddress getNodeAddress()
    {
        return this.nodeID.getNodeAddress();
    }

    /**
     * Gets the vconnn part of the location
     * 
     * @return The VConnN part of the NetConnectionLocation
     */
    public int getVConnN()
    {
        return this.vconnn;
    }

    /**
     * Converts the NetConnectionLocation object into a string representation of the form nconnl://[NodeID]/[VConnN]
     * 
     * @return The String form of the NetConnectionLocation
     */
    public String toString()
    {
        return "nconnl://" + this.nodeID.toString() + "/" + this.vconnn;
    }

    /**
     * Converts the string form of a NetConnectionLocation back into its object form
     * 
     * @param str
     *            The string representation of a NetConnectionLocation
     * @return A new NetConnectionLocation created from the String representation
     */
    public static NetConnectionLocation parse(String str)
    {
        if (str.Equals("null", StringComparison.OrdinalIgnoreCase))
            return null;
        if (str.StartsWith("nconnl://"))
        {
            String toParse = str.Substring(6);
            String[] addressBits = toParse.Split("/");
            NodeID nodeID = NodeID.parse(addressBits[0]);
            int vcn = Int32.Parse(addressBits[1]);
            return new NetConnectionLocation(nodeID, vcn);
        }
        throw new ArgumentException("String is not a string form of a NetConnectionLocation");
    }
}
}