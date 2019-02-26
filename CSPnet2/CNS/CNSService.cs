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
using CSPnet2.NetChannels;
using CSPnet2.NetNode;

namespace CSPnet2.CNS
{

/**
 * This is the service object used to register and resolve channel names with a Channel Name Server. This provides a
 * client front end.
 * 
 * @author Kevin Chalmers (updated from Quickstone Technologies)
 */
public sealed class CNSService
{
    /**
     * The channel to send messages to the CNS upon
     */
    private readonly NetChannelOutput toCNS;

    /**
     * The incoming channel to receive messages from the CNS from
     */
    private readonly NetChannelInput fromCNS;

    /**
     * Creates a new CNSService
     * 
     * @param cnsNode
     *            The NodeID of the Node with the CNS on it
     * @//throws JCSPNetworkException
     *             Thrown if something goes wrong in the underlying architecture
     */
    public CNSService(NodeID cnsNode)
       // //throws JCSPNetworkException
    {
        // Create the input and output channel
        this.toCNS = NetChannel.one2net(new NetChannelLocation(cnsNode, 1), new CNSNetworkMessageFilter.FilterTX());
        this.fromCNS = NetChannel.net2one(new CNSNetworkMessageFilter.FilterRX());

        // We now need to logon to the CNS
        CNSMessage message = new CNSMessage();
        message.type = CNSMessageProtocol.LOGON_MESSAGE;
        message.location1 = (NetChannelLocation)this.fromCNS.getLocation();
        this.toCNS.write(message);

        // Wait for logon reply message
        CNSMessage logonReply = (CNSMessage)this.fromCNS.read();

        // Check if we logged on OK
        if (logonReply.success == false)
        {
            Node.err.log(this.GetType(), "Failed to logon to CNS");
            throw new JCSPNetworkException("Failed to Logon to CNS");
        }
        Node.log.log(this.GetType(), "Logged into CNS");
    }

    /**
     * Registers an input end with the CNS
     * 
     * @param name
     *            The name to register the channel with
     * @param in
     *            The NetChannelInput to register with the CNS
     * @return True if the channel was successfully registered, false otherwise
     */
    public Boolean register(String name, NetChannelInput In)
    {
        // Ensure that only one registration can happen at a time
        lock (this)
        {
            // Create a new registration message
            CNSMessage message = new CNSMessage();
            message.type = CNSMessageProtocol.REGISTER_REQUEST;
            message.name = name;
            message.location1 = (NetChannelLocation)this.fromCNS.getLocation();
            message.location2 = (NetChannelLocation) In.getLocation();
            // Write registration message to the CNS
            this.toCNS.write(message);
            // Read in reply
            CNSMessage reply = (CNSMessage)this.fromCNS.read();
            return reply.success;
        }
    }

    /**
     * Resolves a name on the CNS, retrieving the NetChannelLocation for the channel
     * 
     * @param name
     *            The name to resolve
     * @return The NetChannelLocation of the channel declared name
     * @//throws JCSPNetworkException
     *             Thrown if something goes wrong in the underlying architecture
     */
    public NetChannelLocation resolve(String name)
        ////throws JCSPNetworkException
    {
        // Create a temporary channel to receive the incoming NetChannelLocation
        NetChannelInput In = NetChannel.net2one(new CNSNetworkMessageFilter.FilterRX());
        // Create a resolution message
        CNSMessage message = new CNSMessage();
        message.type = CNSMessageProtocol.RESOLVE_REQUEST;
        message.location1 = (NetChannelLocation) In.getLocation();
        message.name = name;
        // Write the resolution message to the CNS
        this.toCNS.write(message);
        // Read in reply
        CNSMessage reply = (CNSMessage) In.read();
        // Destroy the temporary channel
        In.destroy();
        // Now return the resolved location, or throw an exception
        if (reply.success == true)
            return reply.location1;
        throw new JCSPNetworkException("Failed to resolve channel named: " + name);
    }
}
}