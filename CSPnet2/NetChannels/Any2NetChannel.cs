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

namespace CSPnet2.NetChannels
{
    /***********************************************************************************************************************
     * An outputting network channel (TX) that can be safely shared amongst multiple writers (Any2Net). This is the concrete
     * implementation of the construct. For information on the user level interface, see NetSharedChannelOutput
     * 
     * @see jcsp.net2.NetSharedChannelOutput
     * @author Kevin Chalmers (updated from Quickstone Technologies Limited)
     */
    sealed class Any2NetChannel : NetSharedChannelOutput
    {
        /**
         * The underlying One2NetChannel used by this channel. This class acts like a wrapper, protecting the underlying
         * unshared connection.
         */
        private readonly One2NetChannel chan;

        /**
         * Static factory method used to create an Any2NetChannel
         * 
         * @param loc
         *            The location of the input channel end
         * @param immunity
         *            The immunity level of the channel
         * @param filter
         *            The filter used to convert the object being sent into bytes
         * @return A new Any2NetChannel connected to the input end.
         * @//throws JCSPNetworkException
         *             Thrown if a connection to the Node cannot be made.
         */
        internal static Any2NetChannel create(NetChannelLocation loc, int immunity, NetworkMessageFilter.FilterTx filter)
        {
            One2NetChannel channel = null;
            try
            {
                channel = One2NetChannel.create(loc, immunity, filter);
            }
            catch (JCSPNetworkException e)
            {
                Console.WriteLine(e);
            }
            return new Any2NetChannel(channel);
        }

        /**
         * Constructor wrapping an existing One2NetChannel in an Any2NetChannel
         * 
         * @param channel
         *            The One2NetChannel to be wrapped.
         */
        private Any2NetChannel(One2NetChannel channel)
        {
            this.chan = channel;
        }

/**
 * Poisons the underlying channel
 * 
 * @param strength
 *            The strength of the poison being put on the channel
 */
        public void poison(int strength)
        {
            lock (this)
            {
                this.chan.poison(strength);
            }
        }

/**
 * Gets the NetChannelLocation of the input end this channel is connected to.
 * 
 * @return The location of the input end that this output end is connected to.
 */
        public NetLocation getLocation()
        {
            return this.chan.getLocation();
        }

/**
 * Writes an object to the underlying channel.
 * 
 * @param object
 *            The Object to write to the channel
 * @//throws JCSPNetworkException
 *             Thrown if something happens in the underlying architecture
 * @//throws PoisonException
 *             Thrown if the channel has been poisoned.
 */
        public void write(Object _object)
//throws JCSPNetworkException, PoisonException
        {
            lock (this)
            {
                this.chan.write( _object);
            }
        }

/**
 * Writes asynchronously to the underlying channel.
 * 
 * @param object
 *            The object to write to the channel
 * @//throws JCSPNetworkException
 *             Thrown if something happens in the underlying architecture
 * @//throws PoisonException
 *             Thrown is the channel has been poisoned
 */
        public void asyncWrite(Object _object)
//throws JCSPNetworkException, PoisonException
        {
            lock (this)
            {
                this.chan.asyncWrite(_object);
            }
        }

/**
 * Removes the channel from the ChannelManager, and sets the state to DESTROYED
 */
        public void destroy()
        {
            lock (this)
            {
                this.chan.destroy();
            }
        }

/**
 * Sets the underlying message filter
 * 
 * @param encoder
 *            The new message filter to use
 */
        public void setEncoder(NetworkMessageFilter.FilterTx encoder)
        {
            lock (this)
            {
                this.chan.setEncoder(encoder);
            }
        }
    }
}