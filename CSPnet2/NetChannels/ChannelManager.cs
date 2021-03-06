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

namespace CSPnet2.NetChannels
{

/**
 * A class used to manage the networked channels on the Node. This is an internal object to JCSP networking. For a
 * description of networked channels, see the relevant documentation.
 * 
 * @see jcsp.net2.NetChannelInput
 * @see jcsp.net2.NetChannelOutput
 * @author Kevin Chalmers
 */
sealed class ChannelManager
{
    /**
     * The index for the next channel to be created. We start at 50 as it allows us to have up to 50 default channels,
     * for example for use to connect to a Channel Name Server.
     */
    private static int index = 50;

    /**
     * The table containing the channels. An Integer (object wrapped int) is used as the key, and the ChannelData as the
     * value.
     */
    private readonly Hashtable channels = new Hashtable();

    /**
     * Singleton instance of the ChannelManager
     */
    private static ChannelManager instance = new ChannelManager();

    /**
     * Private default constructor. Used for the singleton instance.
     */
    private ChannelManager()
    {
        // Empty constructor
    }

    /**
     * Allows getting of the singleton instance.
     * 
     * @return The singleton instance of the ChannelManager
     */
    internal static ChannelManager getInstance()
    {
        return instance;
    }

    /**
     * Allocates a new number to the channel, and stores it in the table.
     * 
     * @param cd
     *            The ChannelData for the channel
     */
    /*synchronized*/ internal void create(ChannelData cd)
    {
        // First allocate a new number for the channel
        int objIndex = index;
        while (this.channels[objIndex] != null)
        {
            //objIndex = new Integer(++index);
            ++index;
        }

        // Set the index of the ChannelData
        cd.vcn = index;

        // Now put the channel in the channel Hashtable
        this.channels.Add(objIndex, cd);

        // Finally increment the index for the next channel to be created
        index++;
    }

    /**
     * Stores a channel in the given index in the table.
     * 
     * @param idx
     *            The index to use for the channel
     * @param cd
     *            The ChannelData for the channel
     * @//throws ArgumentException 
     *             If a channel of the given index already exists.
     */
    /*synchronized*/ internal void create(int idx, ChannelData cd)
        //throws ArgumentException 
    {
        // First check that a channel of the given index does not exist. If it does, throw an exception
        int objIndex = idx;
        if (this.channels[objIndex] != null)
            throw new ArgumentException ("Channel of given number already exists.");

        // Set the index of the channel data
        cd.vcn = idx;

        // Now add the channel to the channels table
        this.channels.Add(objIndex, cd);

        // Update the index if necessary
        if (idx == ChannelManager.index)
            ChannelManager.index++;
    }

    /**
     * Retrieves a channel from the table
     * 
     * @param idx
     *            Index in the table to retrieve the channel from.
     * @return The ChannelData object for the channel.
     */
    internal ChannelData getChannel(int idx)
    {
        int objIndex = idx;
        return (ChannelData)this.channels[objIndex];
    }

    /**
     * Removes a channel from the table.
     * 
     * @param data
     *            ChannelData for channel to remove
     */
    internal void removeChannel(ChannelData data)
    {
        int objIndex = data.vcn;
        this.channels.Remove(objIndex);
    }

}
}