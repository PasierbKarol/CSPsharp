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
    /**
     * This class is a concrete implementation of a NetSharedChannelInput, and acts as a wrapper to a Net2OneChannel,
     * allowing safe shared access. This class is internal to the JCSP architecture. To create an instance of this class,
     * use the NetChannel factory, or the CNS.
     * 
     * @see NetChannelInput
     * @see NetSharedChannelInput
     * @see NetChannel
     * @author Kevin Chalmers (updated from Quickstone Technologies)
     * @author Neil Brown (for the extended read operations)
     */
    public sealed class Net2AnyChannel : NetSharedChannelInput
    {
        private readonly Net2OneChannel actualChannel;

        /**
         * A mutual exclusion lock, allowing only one process access to perform a read operation at a time
         */
        private readonly Net2_Mutex mutex = new Net2_Mutex();

        /**
         * A static factory method to create a new Net2AnyChannel object
         * 
         * @param poisonImmunity
         *            The immunity level of the channel
         * @param filter
         *            The filter used to convert an incoming byte array into an object
         * @return A new Net2AnyChannel
         * @//throws JCSPNetworkException
         *             Thrown if there is a problem creating the underlying channel
         */
        internal static Net2AnyChannel create(int poisonImmunity, NetworkMessageFilter.FilterRx filter)
        ////throws JCSPNetworkException
        {
            // Create the underlying channel
            Net2OneChannel chan = Net2OneChannel.create(poisonImmunity, filter);
            return new Net2AnyChannel(chan);
        }

        /**
         * Static factory method for creating a new instance of Net2AnyChannel, given a particular index
         * 
         * @param index
         *            The index to create the channel with
         * @param poisonImmunity
         *            the immunity level of the channels
         * @param filter
         *            The filter used to convert the byte array back into an object
         * @return A new Net2AnyChannel
         * @//throws ArgumentException 
         *             Thrown if a channel with the given index already exists
         * @//throws JCSPNetworkException
         *             Thrown if something goes wrong during the creation of the underlying channel
         */
        internal static Net2AnyChannel create(int index, int poisonImmunity, NetworkMessageFilter.FilterRx filter)
        {
            // Create the underlying channel
            Net2OneChannel chan = null;

            try
            {
                chan = Net2OneChannel.create(index, poisonImmunity, filter);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
            catch (JCSPNetworkException e)
            {
                Console.WriteLine(e);
            }
            // Return a new instance of Net2AnyChannel
            return new Net2AnyChannel(chan);
        }

        /**
         * @param chan
         *            The underlying channel that this object will wrap around
         */
        private Net2AnyChannel(Net2OneChannel chan)
        {
            this.actualChannel = chan;
        }

        /**
         * Ends an extended read operation
         * 
         * @//throws InvalidOperationException
         *             Thrown if the channel is not in an extended read state
         * @//throws JCSPNetworkException
         *             Thrown if something goes wrong in the underlying network architecture
         * @//throws NetworkPoisonException
         *             Thrown if the underlying channel has been poisoned
         */
        public void endRead()
        //throws InvalidOperationException, JCSPNetworkException, NetworkPoisonException
        {
            // Acquire lock on the channel to ensure exclusive access
            lock (this)
            {
                // We now try and end the read operation. There are a number of possible exceptions
                // that can be thrown, so we must catch them and re-throw them. What we must ensure is
                // done is that the mutex is released.
                try
                {
                    this.actualChannel.endRead();
                }
                catch (InvalidOperationException ise)
                {
                    throw ise;
                }
                catch (JCSPNetworkException jne)
                {
                    throw jne;
                }
                catch (NetworkPoisonException npe)
                {
                    throw npe;
                }
                finally
                {
                    this.mutex.release();
                }
            }
        }

        /**
         * Reads the next message from the channel
         * 
         * @return Message read from the channel
         * @//throws JCSPNetworkException
         *             Thrown if something goes wrong in the underlying architecture
         * @//throws InvalidOperationException
         *             Thrown if the channel is in an extended read state
         * @//throws NetworkPoisonException
         *             Thrown if the channel has been poisoned
         */
        public Object read()
        //throws JCSPNetworkException, InvalidOperationException, NetworkPoisonException
        {
            // First, we must endure that only one process is trying to perform a read operations
            this.mutex.claim();

            // Now ensure that we are the only process operating on the underlying channel
            lock (this)
            {
                // We now try and perform the read operation. There are a number of possible exceptions, which we must
                // catch and then re-throw again. Finally, we must ensure that the read operation is finished by
                // releasing the mutex.
                try
                {
                    Object toReturn = this.actualChannel.read();
                    return toReturn;
                }
                catch (JCSPNetworkException jne)
                {
                    throw jne;
                }
                catch (InvalidOperationException ise)
                {
                    throw ise;
                }
                catch (NetworkPoisonException npe)
                {
                    throw npe;
                }
                finally
                {
                    this.mutex.release();
                }
            }
        }

        /**
         * Begins an extended read operation on the channel
         * 
         * @return The message read from the channel
         * @//throws JCSPNetworkException
         *             Thrown if something goes wrong in the underlying architecture
         * @//throws NetworkPoisonException
         *             Thrown if the channel has been poisoned
         * @//throws InvalidOperationException
         *             Thrown if the channel is in an extended read state
         */
        public Object startRead()
        //throws JCSPNetworkException, NetworkPoisonException, InvalidOperationException
        {
            // First ensure we have exclusive read access
            this.mutex.claim();

            // Now ensure we have exclusive access to the channel
            lock (this)
            {
                // Now return the message read from the channel. We could encounter a number of exceptions here, in which
                // case we must re-throw the exception, remembering to release to release the read lock prior to doing so.
                try
                {
                    return this.actualChannel.startRead();
                }
                catch (JCSPNetworkException jne)
                {
                    this.mutex.release();
                    throw jne;
                }
                catch (NetworkPoisonException npe)
                {
                    this.mutex.release();
                    throw npe;
                }
                catch (InvalidOperationException ise)
                {
                    this.mutex.release();
                    throw ise;
                }
            }
        }

        /**
         * Poisons the underlying channel
         * 
         * @param strength
         *            The strength of the poison
         */
        public void poison(int strength)
        {
            lock (this)
            {
                this.actualChannel.poison(strength);
            }
        }

        /**
         * Gets the channel location of this channel
         * 
         * @return Location of this channel
         */
        public NetLocation getLocation()
        {
            return this.actualChannel.getLocation();
        }

        public String GetLocationAsString()
        {
            return this.actualChannel.getLocation().ToString();
        }

        /**
         * Destroys the channel
         */
        public void destroy()
        {
            lock (this)
            {
                this.actualChannel.destroy();
            }
        }

        /**
         * Sets the underlying message filter
         * 
         * @param decoder
         *            The new message filter to use
         */
        public void setDecoder(NetworkMessageFilter.FilterRx decoder)
        {
            lock (this)
            {
                this.actualChannel.setDecoder(decoder);
            }
        }

    }
}