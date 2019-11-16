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
using CSPnet2.NetNode;
using CSPutil;

namespace CSPnet2.Net2Link
{
    /**
     * Class for managing Links. Ensures that only one Link is only ever created for each individual Node that the hosting
     * Node may be connected to. This is an internal management class of JCSP. For information on how to create Links, see
     * LinkFactory.
     * 
     * @see Link
     * @see LinkFactory
     * @author Kevin Chalmers (updated from Quickstone Technologies)
     */
    sealed class LinkManager
    {
        /**
         * A table containing the links currently in operation within the Node. The key is a NodeID and the value is the
         * Link itself for that specific NodeID.
         */
        private static readonly Hashtable links = new Hashtable();

        /**
         * These event channels are used by the LinkManager to inform any process that may be interested in Link Lost
         * events.
         */
        private static ArrayList eventChans = new ArrayList();
        private static LinkManager instance = new LinkManager();

        private LinkManager()
        {
        }

        /**
         * Gets the singleton instance of the LinkManager
         * 
         * @return The singleton instance of the LinkManager
         */
        internal static LinkManager getInstance()
        {
            return instance;
        }

        /**
         * Handles a Link Lost event. This is done by sending messages over the event channels registered in the eventChans
         * ArrayList. This is possible in the current thread as each event channel is infinitely buffered, so no blocking
         * can occur.
         * 
         * @param link
         *            The Link that has been lost.
         */
        /*synchronized*/
        internal void lostLink(Link link)
        {
            // First remove the Link from the links table, using the Link's NodeID
            NodeID removedNodeID = link.remoteID;
            links.Remove(link.remoteID);
            bool removed = links.Contains(link.remoteID);

            // Now check if the Link was indeed removed. Unlikely to happened, but the Link may have been previously removed
            // meaning we have achieved nothing
            if (removed != null)
            {
                // Log the Link Lost
                Node.log.log(this.GetType(), "Link lost to: " + removedNodeID);

                // Now inform any process listening on a Link Lost channel
                for (IEnumerator enumerator = eventChans.GetEnumerator(); enumerator.MoveNext();)
                    ((ChannelOutput)enumerator.Current).write(removedNodeID);
            }
        }

        /**
         * Registers a new Link with the LinkManager.
         * 
         * @param link
         *            The Link to register.
         * @return True if a Link to the Node does not yet exist, false otherwise.
         */
        /*synchronized*/
        internal Boolean registerLink(Link link)
        {
            // Log the registration attempt
            Node.log.log(this.GetType(), "Trying to register Link to: " + link.remoteID);

            // Retrieve the NodeID for the Link
            NodeID remoteID = link.remoteID;

            // Now check whether the key has been registered in the Links table
            if (links.ContainsKey(remoteID))
            {
                // Connection to the Node already exists. Log.
                Node.err.log(this.GetType(), "Failed to register Link to " + link.remoteID
                                              + ". Connection to Node already exists");
                return false;
            }

            // Link registration successful. Log.
            Node.log.log(this.GetType(), "Link established to: " + link.remoteID);

            // Add the Link to the links table
            links.Add(link.remoteID, link);
            return true;
        }

        /**
         * Returns the Link for the given NodeID
         * 
         * @param id
         *            The NodeID of the remote node
         * @return The Link for the given NodeID
         */
        /*synchronized*/
        internal Link requestLink(NodeID id)
        {
            return (Link)links[id];
        }

        /**
         * Gets a channel input end for receiving Link Lost events.
         * 
         * @return A input end for receiving Link Lost Events.
         */
        /*synchronized*/
        internal AltingChannelInput getLinkLostEventChannel()
        {
            // Create a new infinitely buffered one to one channel
            One2OneChannel eventChan = Channel.one2one(new InfiniteBuffer());

            // Add the output end to the list of event channels
            eventChans.Add(eventChan.Out());
            return eventChan.In();
        }
    }
}