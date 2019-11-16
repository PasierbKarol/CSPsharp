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
using System.Diagnostics;
using System.IO;
using CSPlang;
using CSPlang.Any2;
using CSPnet2.Barriers;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;

namespace CSPnet2.Net2Link
{
    /**
     * Abstract class representing a Link. This class defines the two processes (Link TX, Link RX) where the network
     * protocol is of key importance. Specific technology protocols (e.g. TCP/IP) must extend this class, providing the
     * necessary streams for operation, and also overriding the methods connect, createResources and destroyResources, which
     * will have specific implementations dependent on the underlying technology. Everything else should operate as defined
     * in this class.
     * 
     * @see jcsp.net2.NetworkProtocol
     * @author Kevin Chalmers
     */
    public abstract class Link : IamCSProcess
    {
        /**
         * A flag used to indicate whether the Link is connected or not. This flag is set normally during the connect
         * operation, but may be done within the constructor. If not set during the constructor, and if connect is not
         * called to set the flag to true, then when the process is run connect will be called.
         */
        protected Boolean connected = false;

        /**
         * The incoming stream for the connection. This must be created by the specific protocol implementation.
         */
        protected BinaryReader rxStream;

        /**
         * The outgoing stream for the connection. This must be created by the specific protocol implementation.
         */
        protected BinaryWriter txStream;

        /**
         * The channel connected to the Link Tx process. This is used by channels, barriers, and the Link Rx to send
         * messages to the node this Link is connected to.
         */
        private readonly Any2OneChannel txChannel = Channel.any2one();

        /**
         * The NodeID of the opposite end of the connection. This should be set either during construction, or during the
         * connect method of a child class.
         */
        internal NodeID remoteID;

        /**
         * Link priority in the system. This is a publicly accessible value that can be set by a user.
         */
        public static int LINK_PRIORITY = ProcessManager.PRIORITY_NORM;

        /**
         * Link priority for this Link. This is exposed to child classes to allow specific Link priorities for different
         * Link types.
         */
        internal int priority = Link.LINK_PRIORITY;

        /**
         * This Hashtable is used to keep track of the current output channels that are connected to this Link. In the
         * outcome of a connection failure to the remote Node, the Link uses this table to notify all registered output
         * ends, allowing them to throw an exception instead of deadlocking.
         */
        private Hashtable connectedOutputs = new Hashtable();

        /**
         * This Hashtable is used to keep track of the current barriers that are connected to this Link. In the outcome of a
         * connection failure to the remote Node, the Link uses this table to notify all registered barriers, allowing them
         * to throw an exception instead of deadlocking.
         */
        private Hashtable connectedBarriers = new Hashtable();

        /**
         * Returns the NodeID of the connected Link.
         * 
         * @return NodeID of the connected Link.
         */
        public /*final*/ NodeID getRemoteNodeID()
        {
            return this.remoteID;
        }

        /**
         * Gets the channel that is connected to the Link Tx process.
         * 
         * @return The ChannelOutput used to communicate with the Link Tx.
         */
        internal /*final*/ ChannelOutput getTxChannel()
        {
            return this.txChannel.Out();
        }

        /**
         * Connects to the remote Node. This must be overridden by a child class implementation.
         * 
         * @return True if the connection succeeds, false otherwise.
         * @//throws JCSPNetworkException
         *             Thrown if the connection fails.
         */
        public abstract Boolean connect();
        //throws JCSPNetworkException;

        /**
         * Creates the resources (if any) required for the Node. These could be set up during construction, but if not, this
         * method is called immediately after connect within the run method. Child implementations should override this
         * method.
         * 
         * @return True if resources were created OK, false otherwise.
         * @//throws JCSPNetworkException
         *             Thrown if a problem occurs creating the resources.
         */
        protected abstract Boolean createResources();
        //throws JCSPNetworkException;

        /**
         * Destroys any used resources. This is called whenever a Node fails. Particular implementations must overwrite this
         * method.
         */
        public abstract void destroyResources();

        /**
         * Registers the Link with the LinkManager
         * 
         * @return True if Link was registered, false otherwise.
         */
        public /*final*/ Boolean registerLink()
        {
            return LinkManager.getInstance().registerLink(this);
        }

        /**
         * Marks the Link as lost within the LinkManager.
         */
        protected /*final*/ void lostLink()
        {
            // Acquire a lock on the Link. We wish to stop any more channel or barrier registrations,
            // so therefore we stop any more happening until the Link lost operation is completed.
            lock (this)
            {
                // First set connected to false, and inform the LinkManager
                this.connected = false;
                LinkManager.getInstance().lostLink(this);

                // Iterate through the registered channels and send them all LINK_LOST messages.
                for (IEnumerator enumerator = this.connectedOutputs.Values.GetEnumerator(); enumerator.MoveNext();)
                {
                    // Really we could send just the same LINK_LOST message to all channels. Aliasing should not be a
                    // concern
                    // as the channel will effectively be broken after this
                    ChannelOutput toChannel = ((ChannelData)enumerator.Current).toChannel;
                    NetworkMessage message = new NetworkMessage();
                    message.type = NetworkProtocol.LINK_LOST;
                    toChannel.write(message);
                }

                // Clear the table of registered channels, and then set it to null.
                this.connectedOutputs.Clear();
                this.connectedOutputs = null;

                // Now do the same for the barriers, sending LINK_LOST to each.
                for (IEnumerator enumerator = this.connectedBarriers.Values.GetEnumerator(); enumerator.MoveNext();)
                {
                    ChannelOutput toBar = ((BarrierData)enumerator.Current).toBarrier;
                    NetworkMessage message = new NetworkMessage();
                    message.type = NetworkProtocol.LINK_LOST;
                    toBar.write(message);
                }

                // Clear the table of registered barriers, and then set it to null.
                this.connectedBarriers.Clear();
                this.connectedBarriers = null;
            }
        }

        /**
         * Register a channel with the Link.
         * 
         * @param data
         *            The ChannelData object representing the channel
         */
        internal void registerChannel(ChannelData data)
        {
            // Acquire a lock on the Link. This is to ensure that we don't try and register when the Link is going down.
            lock (this)
            {
                // Check that the Link is still up. If it isn't, then we inform the channel.
                // *IMPLEMENTATION NOTE*: Technically, we could either throw an exception or return a error message here.
                // (for example false). However, this means that the user would have to worry about this outcome in a catch
                // block. The creation methods for channels return channel objects, so the only other option would be to
                // return null. This would lead to confusion if the user did not know that null was a possible return value.
                // It is better to send the LINK_LOST message to the channel, and allow normal error handling during a write
                // operation. Therefore, if the user wishes to ensure that a link is up, it is no different than a normal
                // catch on JCSPNetworkException.
                if (!this.connected)
                {
                    NetworkMessage message = new NetworkMessage();
                    message.type = NetworkProtocol.LINK_LOST;
                    data.toChannel.write(message);
                }

                // Otherwise the Link can take the channel. Add the channel to the table of registered channels.
                else
                {
                    int objIndex = data.vcn;
                    this.connectedOutputs.Add(objIndex, data);
                }
            }
        }

        /**
         * Unregisters and removes the channel from the Link.
         * 
         * @param data
         *            The ChannelData object representing the channel.
         */
        public void deRegisterChannel(ChannelData data)
        {
            // Acquire a lock on the Link.
            lock (this)
            {
                // All we need to do is ensure that the Hashtable of connected channels still exists. It is unlikely that
                // this occurrence can happen, but destroy may be called on the channel as the Link is going down.
                if (this.connectedOutputs != null)
                {
                    // Remove the channel from the registered channels Hashtable
                    int objIndex = data.vcn;
                    this.connectedOutputs.Remove(objIndex);
                }
            }
        }

        /**
         * Registers a barrier with the Link
         * 
         * @param data
         *            The barrier to register with the Link
         */
        internal void registerBarrier(BarrierData data)
        {
            // Acquire lock on the Link
            lock (this)
            {
                // Check that the Link is still up. If it isn't, then we inform the Barrier.
                // *IMPLEMENTATION NOTE*: Technically, we could either throw an exception or return a error message here.
                // (for example false). However, this means that the user would have to worry about this outcome in a catch
                // block. The creation methods for barriers return barrier objects, so the only other option would be to
                // return null. This would lead to confusion if the user did not know that null was a possible return value.
                // It is better to send the LINK_LOST message to the barrier, and allow normal error handling during an
                // operation. Therefore, if the user wishes to ensure that a link is up, it is no different than a normal
                // catch on JCSPNetworkException.
                if (!this.connected)
                {
                    NetworkMessage message = new NetworkMessage();
                    message.type = NetworkProtocol.LINK_LOST;
                    data.toBarrier.write(message);
                }
                else
                {
                    // Otherwise add the barrier to the connected Hashtable of connected barriers
                    int objIndex = data.vbn;
                    this.connectedBarriers.Add(objIndex, data);
                }
            }
        }

        /**
         * Unregisters a barrier with the Link
         * 
         * @param data
         *            The BarrierData representing the Barrier to unregister
         */
        void deRegisterBarrier(BarrierData data)
        {
            // Acquire a lock on the Link
            lock (this)
            {
                // First we check if the connectedBarriers is not equal to null. If it is, then we can assume that the Link
                // has gone down. In such a circumstance, we ignore the deregistration. The interacting barrier is already
                // going down.
                if (this.connectedBarriers != null)
                {
                    //Integer objIndex = new Integer(data.vbn);
                    this.connectedBarriers.Remove(data.vbn);
                }
            }
        }

        /**
         * The run method for the process. This will connect the Link (if necessary) and then start the Tx and Rx Loops.
         */
        public /*final*/ void run()
        {
            // Check if connected, and if not try and connect.
            if (!this.connected)
            {
                try
                {
                    if (!connect())
                        // Failed to connect. Stop Link process
                        return;
                    if (!createResources())
                        // Failed to create resources. Stop Link process.
                        return;
                }
                catch (JCSPNetworkException jne)
                {
                    // Something went wrong during connection. Stop Link
                    Console.WriteLine(jne.Message);
                    jne.StackTrace();
                    Node.err.log(this.GetType(), "Failed to connect Link to " + this.remoteID);
                    return;
                }
            }

            // Create and start Tx and Rx loops.
            TxLoop txLoop = new TxLoop(this.txChannel.In(), this.txStream);
            RxLoop rxLoop = new RxLoop(this.txChannel.Out(), this.rxStream);
            ProcessManager txProc = new ProcessManager(txLoop);
            ProcessManager rxProc = new ProcessManager(rxLoop);
            txProc.setPriority(this.priority);
            rxProc.setPriority(this.priority);
            IamCSProcess[] processes = { txProc, rxProc };
            new CSPParallel(processes).run();

            // At this point the Link has gone down. Should we be accepting messages? This should have really been
            // handled during the destroy resources stage. But just in case we send LINK_LOST messages appropriately.
            while (true)
            {
                NetworkMessage linkLost = new NetworkMessage();
                linkLost.type = NetworkProtocol.LINK_LOST;
                NetworkMessage msg = (NetworkMessage)this.txChannel.In().read();
                switch (msg.type)
                {
                    // We only respond to certain message types.
                    case NetworkProtocol.SEND:
                    case NetworkProtocol.ASYNC_SEND:
                        // Get the appropriate channel
                        ChannelData chan = ChannelManager.getInstance().getChannel(msg.attr2);
                        chan.toChannel.write(linkLost);
                        break;

                    case NetworkProtocol.SYNC:
                        // Get the appropriate barrier
                        BarrierData bar = BarrierManager.getInstance().getBarrier(msg.attr2);
                        bar.toBarrier.write(linkLost);
                        break;
                }
            }
        }

        /**
         * The TxLoop for the Link. This could be implemented as a synchronized method call.
         * 
         * @author Kevin Chalmers
         */
        sealed class TxLoop : IamCSProcess
        {
            /**
             * The input channel to the TX process. Channels and Barriers send outgoing messages via this channel
             */
            private readonly ChannelInput input;

            /**
             * The output stream connecting to the remote node's input stream.
             */
            //private readonly DataOutputStream outputStream;
            private readonly BinaryWriter outputStream;

            /**
             * Constructor to create the TX part of the Link
             * 
             * @param in
             *            The channel connecting into the Link TX from the various channels and barriers
             * @param stream
             *            The output stream connected to the remote node
             */
            internal TxLoop(ChannelInput In, BinaryWriter stream)
            {
                this.input = In;
                this.outputStream = stream;
            }

            /**
             * The run loop of the TX process
             */
            public void run()
            {
                try
                {
                    while (true)
                    {
                        // Read in next message
                        NetworkMessage message = (NetworkMessage)this.input.read();

                        // Write message to the stream.
                        this.outputStream.Write(message.type);
                        this.outputStream.Write(message.attr1);
                        this.outputStream.Write(message.attr2);

                        // Check if message has data element
                        if (message.type == NetworkProtocol.SEND || message.type == NetworkProtocol.ARRIVED
                                                             || message.type == NetworkProtocol.ASYNC_SEND)
                        {
                            // Write data element
                            this.outputStream.Write(message.jsonData);
                        }

                        // Flush the stream.
                        this.outputStream.Flush();
                    }
                }
                catch (IOException ioe)
                {
                    // Something went wrong during I/O. Destroy resources.
                    //TODO Call to the function below needs to be restored and operate properly
                    //destroyResources();
                }
            }
        }

        /**
         * The RxLoop for the Link.
         * 
         * @author Kevin Chalmers
         */
        sealed class RxLoop : IamCSProcess
        {
            /**
             * This is the channel connected to the Link's TX process. We retain this in the RX process for messages such as
             * reject to be sent directly back to the remote node, and to ease the communication mechanism. Any message that
             * requires an acknowledgement has this channel attached to it for simplicity.
             */
            private readonly ChannelOutput toTxProcess;

            /**
             * The input stream receiving messages from the opposite remote node
             */
            //private readonly DataInputStream inputStream;
            private readonly BinaryReader inputStream;

            /**
             * This is the list of barrier server ends that this Link has received an enroll for. We retain this to allow
             * server ends to be notified when the Link goes down, letting them know that one of the client ends have gone
             */
            private readonly ArrayList incomingEnrolledBarriers = new ArrayList();

            /**
             * Constructor for the RX part of the Link
             * 
             * @param out
             *            The Channel connected to the TX part of the Link
             * @param stream
             *            The input stream used to receive messages upon
             */
            //internal RxLoop(ChannelOutput Out, DataInputStream stream)
            internal RxLoop(ChannelOutput Out, BinaryReader stream)
            {
                this.toTxProcess = Out;
                this.inputStream = stream;
            }

            /**
             * The point here is to try and deal with any message that may come into the Node. This involves a great deal of
             * choice, and therefore this run method looks quite complicated. However, splitting the switch statements into
             * parts should allow a good idea of exactly what is going on.
             */
            public void run()
            {
                try
                {
                    // Declare the reference for the possible channel and barrier we are may operate on
                    ChannelData data = null;
                    BarrierData barier = null;

                    // Loop forever (or until something goes wrong)
                    while (true)
                    {
                        // Read in the next message from the stream
                        //var a = this.inputStream.BaseStream.ReadByte();
                        int type = this.inputStream.ReadInt32();

                        int attr1 = this.inputStream.ReadInt32();
                        int attr2 = this.inputStream.ReadInt32();

                        // Reconstruct the message object
                        NetworkMessage message = new NetworkMessage();
                        message.type = type;
                        message.attr1 = attr1;
                        message.attr2 = attr2;

                        // Now operate on the message
                        switch (message.type)
                        {
                            // ------------------------------------------------------------------------
                            // *** SEND & ASYNC_SEND ***
                            // ------------------------------------------------------------------------
                            // Data sent to the link from another Node. Deal with
                            // the message
                            case NetworkProtocol.SEND:
                            case NetworkProtocol.ASYNC_SEND:

                                // First also read the data portion of the message
                                message.jsonData = this.inputStream.ReadString();

                                // Attach the channel to allow the acknowledge message to be sent later.
                                message.toLink = this.toTxProcess;

                                // Get the channel we are dealing with.
                                data = ChannelManager.getInstance().getChannel(message.attr1);

                                // Now check if the channel does exist. If the previous operation returned null, we can
                                // determine that
                                // it does not.
                                if (data != null)
                                {
                                    // The channel does exist. Now we must operate on the channel dependent on its state. We
                                    // must ensure that this state can't change while we are doing this, so we lock onto the
                                    // ChannelData object.

                                    // Acquire lock on the channel data
                                    lock (data)
                                    {
                                        // Operate on message based on the current state of the channel.
                                        switch (data.state)
                                        {
                                            // Channel is OK for input, so pass on the message
                                            case ChannelDataState.OK_INPUT:
                                                data.toChannel.write(message);
                                                break;

                                            // Channel is currently migrating. Still pass on the message. When the Channel
                                            // arrives, the REQUEST still needs to occur before this channel is passed on.
                                            case ChannelDataState.MOVING:
                                                data.toChannel.write(message);
                                                break;

                                            // Channel has moved. Still pass on the message.
                                            case ChannelDataState.MOVED:
                                                data.toChannel.write(message);
                                                break;

                                            // Channel has been poisoned. Spread the poison to the sender.
                                            case ChannelDataState.POISONED:
                                                // Create a new poison NetworkMessage
                                                NetworkMessage poison = new NetworkMessage();
                                                poison.type = NetworkProtocol.POISON;
                                                // Destination is the source of the incoming message
                                                poison.attr1 = message.attr2;
                                                // Send the poison level
                                                poison.attr2 = data.poisonLevel;
                                                // Then write the message to the TX process so it can send it to the remote
                                                // Link
                                                message.toLink.write(poison);
                                                break;

                                            // In all other cases we reject the message. The sender is informed and can act
                                            // accordingly.
                                            default:
                                                // Create a new reject NetworkMessage
                                                NetworkMessage reject = new NetworkMessage();
                                                reject.type = NetworkProtocol.REJECT_CHANNEL;
                                                // Destination is source of incoming message
                                                reject.attr1 = message.attr2;
                                                // Attribute 2 unnecessary
                                                reject.attr2 = -1;
                                                // Write reject to the TX process so it can send it to the remote Link
                                                message.toLink.write(reject);
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    // Channel does not exist. Reject the message so the sender can act accordingly.
                                    NetworkMessage reject = new NetworkMessage();
                                    reject.type = NetworkProtocol.REJECT_CHANNEL;
                                    reject.attr1 = message.attr2;
                                    reject.attr2 = -1;
                                    message.toLink.write(reject);
                                    Console.WriteLine("Message was rejected");
                                }

                                break;

                            // ------------------------------------------------------------------------
                            // *** ACK ***
                            // ------------------------------------------------------------------------
                            // Acknowledgement message received. Must inform sending channel.
                            case NetworkProtocol.ACK:

                                // Retrieve the channel
                                data = ChannelManager.getInstance().getChannel(message.attr1);

                                // Check if the channel exists. The previous operation will set data to null if no channel
                                // of that index has been created.
                                if (data != null)
                                {
                                    // The channel exists. We need to acquire a lock on the channel state to ensure it does
                                    // not change as we operate on it.

                                    // Acquire lock on data state
                                    lock (data)
                                    {
                                        // Now behave according to the channel data state
                                        switch (data.state)
                                        {
                                            // Channel is OK_OUTPUT, acknowledge channel.
                                            case ChannelDataState.OK_OUTPUT:
                                                data.toChannel.write(message);
                                                break;

                                            // Channel is not an output, or is destroyed, poisoned, etc. In this case we
                                            // can just ignore the message as there is nothing to do. The receiving end is
                                            // attempting to acknowledge a channel that never sent it a message. Could be
                                            // considered dangerous.
                                            default:
                                                // Ignore message in all other cases
                                                break;
                                        }
                                    }
                                }

                                break;

                            // ------------------------------------------------------------------------
                            // *** ENROLL ***
                            // ------------------------------------------------------------------------
                            // Enrolment on a barrier received.
                            case NetworkProtocol.ENROLL:

                                // Retrieve the barrier
                                barier = BarrierManager.getInstance().getBarrier(message.attr1);

                                // Now check that the barrier exists. The previous operation would have set bar to null if
                                // no channel
                                // of that index exists
                                if (barier != null)
                                {
                                    // Barrier exists. We need to lock the state while we interact to avoid conflicts

                                    // Acquire lock on barrier data state
                                    lock (barier)
                                    {
                                        // Now behave according to the state of the barrier
                                        switch (barier.currentBarrierState)
                                        {
                                            // Barrier is in OK state, and is a server. Enroll with barrier.
                                            case BarrierDataState.OK_SERVER:
                                                // Forward the enrolment
                                                barier.toBarrier.write(message);

                                                // Add the barrier to the incomingEnrolledBarriers
                                                this.incomingEnrolledBarriers.Add(barier);
                                                break;

                                            // Barrier is other state. Reject the enroll and let the enrolling process
                                            // handle it.
                                            default:
                                                // Create the reject message
                                                NetworkMessage reject = new NetworkMessage();
                                                reject.type = NetworkProtocol.REJECT_BARRIER;
                                                // Destination of reject is source of incoming message
                                                reject.attr1 = message.attr2;
                                                // Attribute 2 is not used
                                                reject.attr2 = -1;
                                                // Send message to the TX process of the Link
                                                this.toTxProcess.write(reject);
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    // Barrier does not exist. Reject message and let enrolling process handle it.
                                    NetworkMessage reject = new NetworkMessage();
                                    reject.type = NetworkProtocol.REJECT_BARRIER;
                                    reject.attr1 = message.attr2;
                                    reject.attr2 = -1;
                                    this.toTxProcess.write(reject);
                                    Console.WriteLine("Message was rejected");

                                }

                                break;

                            // ------------------------------------------------------------------------
                            // *** RESIGN ***
                            // ------------------------------------------------------------------------
                            // Resignation from a barrier received.
                            case NetworkProtocol.RESIGN:

                                // Retrieve the barrier.
                                barier = BarrierManager.getInstance().getBarrier(message.attr1);

                                // Check if the Barrier exists. The previous operation returns null if no barrier of the
                                // given
                                // index exists.
                                if (barier != null)
                                {
                                    // The Barrier exists. We now acquire a lock on the Barrier to avoid it changing while
                                    // we
                                    // operate on it.

                                    // Acquire lock on the barrier data state
                                    lock (barier)
                                    {
                                        // Now behave based on the state of the barrier
                                        switch (barier.currentBarrierState)
                                        {
                                            // Barrier is in OK_SERVER state. Attempt resign from barrier.
                                            case BarrierDataState.OK_SERVER:

                                                // First check that a enrolment occurred previously and remove the enrolled
                                                // barrier from the list. This operation returns true if the object was
                                                // successfully removed
                                                //if (!this.incomingEnrolledBarriers.Remove(bar))
                                                //{
                                                //    // The barrier was not previously enrolled, therefore do not resign from
                                                //    // the barrier. We *COULD* reject the resignation here, but that would
                                                //    // be
                                                //    // pointless. Simply continue.
                                                //}
                                                //else
                                                if (!this.incomingEnrolledBarriers.Contains(barier))
                                                {

                                                    // Forward the resignation to the barrier
                                                    barier.toBarrier.write(message);
                                                }

                                                break;

                                            // Barrier is other state. This can be ignored. From the point of view of the
                                            // resigner, no different operation has occurred. It can carry on as normal.
                                            // Could be considered dangerous.
                                            default:
                                                break;
                                        }
                                    }
                                }

                                // Barrier doesn't exist. Ignore.
                                break;

                            // ------------------------------------------------------------------------
                            // *** SYNC ***
                            // ------------------------------------------------------------------------
                            // Sync message for a barrier received.
                            case NetworkProtocol.SYNC:

                                // Retrieve the barrier
                                barier = BarrierManager.getInstance().getBarrier(message.attr1);

                                // Check that the barrier exists. The previous operation returns null if no barrier of the
                                // given
                                // index exists.
                                if (barier != null)
                                {
                                    // The barrier exists. We need to operate on it based on its state. Therefore we need to
                                    // ensure
                                    // that the state doesn't change as we do so, and must acquire a lock on the barrier

                                    // Acquire lock on barrier state
                                    lock (barier)
                                    {
                                        // Attach the output channel to the txLink so when barrier is ready it can inform
                                        // the networked barriers connected to it.
                                        message.toLink = this.toTxProcess;

                                        // Now we must behave based on the state of the barrier
                                        switch (barier.currentBarrierState)
                                        {
                                            // Barrier is in OK_SERVER state. Pass SYNC onto the barrier.
                                            case BarrierDataState.OK_SERVER:

                                                // TODO: Should we check that this Link is enrolled? If we are doing this,
                                                // the ArrayList structure will have to be re-thought to something faster.

                                                // Forward the SYNC
                                                barier.toBarrier.write(message);

                                                break;

                                            // Barrier is not in OK_SERVER state. Reject message
                                            default:
                                                // Create reject message
                                                NetworkMessage reject = new NetworkMessage();
                                                reject.type = NetworkProtocol.REJECT_BARRIER;
                                                // Destination is source of previous message
                                                reject.attr1 = message.attr2;
                                                // Attribute 2 is not required
                                                reject.attr2 = -1;
                                                // Write reject message to the TX process
                                                this.toTxProcess.write(reject);
                                                break;
                                        }
                                    }
                                }
                                else
                                {
                                    // Barrier doesn't exist. Reject the message.
                                    NetworkMessage reject = new NetworkMessage();
                                    reject.type = NetworkProtocol.REJECT_BARRIER;
                                    reject.attr1 = message.attr2;
                                    reject.attr2 = -1;
                                    this.toTxProcess.write(reject);
                                    Console.WriteLine("Message was rejected");
                                }

                                break;

                            // ------------------------------------------------------------------------
                            // *** RELEASE ***
                            // ------------------------------------------------------------------------
                            // Barrier has been released after a sync. Notify the barrier.
                            case NetworkProtocol.RELEASE:

                                // Retrieve the barrier.
                                barier = BarrierManager.getInstance().getBarrier(message.attr1);

                                // Check and see if the barrier exists. The previous operation returns null if no barrier of
                                // the given index exists
                                if (barier != null)
                                {
                                    // We now operate on the barrier based on its state. We therefore need to acquire a lock
                                    // on the barrier to ensure the state doesn't change as we do so.

                                    // Acquire lock on barrier state
                                    lock (barier)
                                    {
                                        // Now behave based on the state of the barrier
                                        switch (barier.currentBarrierState)
                                        {
                                            // Barrier is in OK_CLIENT state. Release the waiting processes.
                                            case BarrierDataState.OK_CLIENT:

                                                // TODO: Should we be checking that this Link is indeed connected to this
                                                // Barrier? This would require the Hashtable of registered barriers to be
                                                // passed into this process.

                                                // Forward on the message
                                                barier.toBarrier.write(message);

                                                break;

                                            // Barrier is not in OK_CLIENT state. Ignore message. From the point of view of
                                            // the releaser no difference is apparent. It may be worth in future informing
                                            // the releaser that this Barrier is broken so it can reduce the number of
                                            // enrolled.
                                            default:
                                                break;
                                        }
                                    }
                                }

                                // Barrier doesn't exist. Ignore.
                                break;

                            // ------------------------------------------------------------------------
                            // *** REJECT_CHANNEL ***
                            // ------------------------------------------------------------------------
                            // A channel message has been rejected. Inform the Channel
                            case NetworkProtocol.REJECT_CHANNEL:

                                // Retrieve the channel
                                data = ChannelManager.getInstance().getChannel(message.attr1);

                                // Now check that the channel exists. The previous operation returns null if no channel
                                // of the given index exists
                                if (data != null)
                                {
                                    // We now behave based on the state of the channel. The state cannot change as we do
                                    // this, so we must ensure it does not change

                                    // Acquire lock on channel state
                                    lock (data)
                                    {
                                        // Behave based on state of the channel
                                        switch (data.state)
                                        {
                                            // Channel is in OK_OUTPUT state. Reject the message sent by it.
                                            case ChannelDataState.OK_OUTPUT:
                                                data.toChannel.write(message);
                                                break;

                                            // Channel is in other state. We can ignore the message. The rejector is
                                            // rejecting anyway, so there is no need to inform that this channel would also
                                            // reject.
                                            default:
                                                break;
                                        }
                                    }
                                }

                                // Barrier doesn't exist, ignore message.
                                break;

                            // ------------------------------------------------------------------------
                            // *** REJECT_BARRIER ***
                            // ------------------------------------------------------------------------
                            // A barrier message has been rejected.
                            case NetworkProtocol.REJECT_BARRIER:

                                // Retrieve the barrier
                                barier = BarrierManager.getInstance().getBarrier(message.attr1);

                                // Now check if the barrier exists. The previous operation returns null if no barrier
                                // exists at the given index.
                                if (barier != null)
                                {
                                    // Barrier exists. We now operate on it based on its state. We must ensure that the
                                    // state doesn't change during this time and must lock the barrier state

                                    // Acquire lock on barrier state
                                    lock (barier)
                                    {
                                        // Not behave based on the state of the barrier
                                        switch (barier.currentBarrierState)
                                        {
                                            // Barrier is in OK_CLIENT state. Reject the message sent by it.
                                            case BarrierDataState.OK_CLIENT:
                                                barier.toBarrier.write(message);
                                                break;

                                            // Barrier is in other state. We can ignore the message. The rejector is broken
                                            // anyway, so there is no need to inform that this barrier is also down.
                                            default:
                                                break;
                                        }
                                    }
                                }

                                // Barrier doesn't exist, ignore message.
                                break;

                            // ------------------------------------------------------------------------
                            // *** MOVED ***
                            // ------------------------------------------------------------------------
                            // TODO: Bits for mobility
                            case NetworkProtocol.MOVED:
                                break;

                            // ------------------------------------------------------------------------
                            // *** ARRIVED ***
                            // ------------------------------------------------------------------------
                            // TODO: Bits for mobility
                            case NetworkProtocol.ARRIVED:
                                break;

                            // ------------------------------------------------------------------------
                            // *** POISON ***
                            // ------------------------------------------------------------------------
                            // Poison message received.
                            case NetworkProtocol.POISON:

                                // Retrieve the channel
                                data = ChannelManager.getInstance().getChannel(message.attr1);

                                // Now check that the channel exists. The previous operation returns
                                // null if no channel of the given index exists
                                if (data != null)
                                {
                                    // The channel exists. We need to ensure that the channel does not change
                                    // state as we operate on.

                                    // Acquire lock on channel data
                                    lock (data)
                                    {
                                        // Now behave based on the state of the barrier
                                        switch (data.state)
                                        {
                                            // Channel is in OK state.
                                            case ChannelDataState.OK_INPUT:
                                            case ChannelDataState.OK_OUTPUT:
                                                // We now must check the channels immunity level
                                                if (message.attr2 > data.immunityLevel)
                                                {
                                                    // The poison message is strong enough to poison the channel.
                                                    // Forward on the message
                                                    data.toChannel.write(message);
                                                }

                                                break;

                                            // TODO: Bits for mobility
                                            case ChannelDataState.MOVING:
                                                break;

                                            // TODO: Bits for mobility
                                            case ChannelDataState.MOVED:
                                                break;

                                            // Channel is already poisoned. Check level and forward if necessary.
                                            case ChannelDataState.POISONED:
                                                if (data.poisonLevel < message.attr2)
                                                    data.toChannel.write(message);
                                                break;

                                            // Channel is in another state. Ignore message. Poisoner is trying to poison a
                                            // channel that is already down. Should be safe to ignore.
                                            default:
                                                break;
                                        }
                                    }
                                }

                                // Channel doesn't exist. Ignore message.
                                break;
                        }
                    }
                }

                // Something has gone wrong at the the communication layer. Destroy the Link.
                catch (IOException ioe)
                {
                    // First destroyResources as appropriate for the implementation
                    //TODO Call to the function below needs to be restored and operate properly
                    //destroyResources();

                    // Now we wish to inform any server ends of a barrier that may have had enrollments via this Link that
                    // the Link is now dead.
                    IEnumerator iter = this.incomingEnrolledBarriers.GetEnumerator();
                    for (; iter.MoveNext();)
                    {
                        BarrierData barrier = (BarrierData)iter.Current;
                        NetworkMessage message = new NetworkMessage();
                        message.type = NetworkProtocol.LINK_LOST;
                        barrier.toBarrier.write(message);
                    }

                    this.incomingEnrolledBarriers.Clear();
                }
            }
        }
    }
}