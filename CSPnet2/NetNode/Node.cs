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
using CSPlang;
using CSPnet2.Net2Link;
using CSPnet2.CNS;
using CSPnet2.BNS;

namespace CSPnet2.NetNode
{

/**
 * @author Kevin Chalmers
 */
public sealed class Node
{
    /**
     * 
     */
    private NodeID nodeID;

    /**
     * 
     */
    private Boolean initialized = false;

    /**
     * 
     */
    private NodeKey nk;

    /**
     * 
     */
    private static Node instance = new Node();

    /**
     * 
     */
    public static Logger log = new Logger();

    /**
     * 
     */
    public static Logger err = new Logger();

    /**
     * @return The singleton instance of the Node
     */
    public static Node getInstance()
    {
        return instance;
    }

    /**
     * @return The NodeID of this Node
     */
    public NodeID getNodeID()
    {
        return this.nodeID;
    }

    /**
     * @param aNodeID
     */
    void setNodeID(NodeID aNodeID)
    {
        this.nodeID = aNodeID;
    }

    /**
     * 
     */
    private Node()
    {
        // Empty constructor
    }

    /**
     * @param addr
     * @return NodeKey for this Node
     * @//throws JCSPNetworkException
     */
    public NodeKey init(NodeAddress addr)
        ////throws JCSPNetworkException
    {
        return this.init("", addr);
    }

    /**
     * @param name
     * @param addr
     * @return NodeKey for this Node
     * @//throws JCSPNetworkException
     */
    public NodeKey init(String name, NodeAddress addr)
        ////throws JCSPNetworkException
    {
        Node.log.log(this.GetType(), "Node initialisation begun");
        if (this.initialized)
            throw new JCSPNetworkException("Node already initialised");
        this.initialized = true;
        LinkServer.start(addr);
        this.nodeID = new NodeID(name, addr);
        this.nk = new NodeKey();
        NodeAddress.installProtocol(addr.getProtocol(), addr.getProtocolID());
        Node.log.log(this.GetType(), "Node initialisation complete");
        return this.nk;
    }

    /**
     * @param factory
     * @return NodeKey for this Node
     * @//throws JCSPNetworkException
     */
    public NodeKey init(NodeFactory factory)
        ////throws JCSPNetworkException
    {
        Node.log.log(this.GetType(), "Node initialisation begun");
        if (this.initialized)
            throw new JCSPNetworkException("Node already initialised");
        NodeAddress localAddr = factory.initNode(this);
        this.nodeID = new NodeID("", localAddr);
        this.initialized = true;
        this.nk = new NodeKey();
        Link toServer = LinkFactory.getLink(factory.cnsAddress);
        
        CNS.CNS.initialise(toServer.remoteID);
        BNS.BNS.initialise(toServer.remoteID);
        return this.nk;
    }

    /**
     * @return A channel to receive disconnect events on
     */
    public AltingChannelInput getLinkLostEventChannel()
    {
        return LinkManager.getInstance().getLinkLostEventChannel();
    }

    /**
     * @param stream
     */
    public void setLog(StreamWriter stream)
    {
        log = new Logger(stream);
    }

    /**
     * @param stream
     */
    public void setErr(StreamWriter stream)
    {
        err = new Logger(stream);
    }

    
}
}