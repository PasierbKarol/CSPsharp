//////////////////////////////////////////////////////////////////////
//                                                                  //
//  JCSP ("CSP for Java") Libraries                                 //
// Copyright 1996-2017 Peter Welch, Paul Austin and Neil Brown      //
//           2005-2017 Kevin Chalmers and Jon Kerridge              //
//                                                                  //
// Licensed under the Apache License, Version 2.0 (the "License");  //
// you may not use this file except in compliance with the License. //
// You may obtain a copy of the License at                          //
//                                                                  //
//      http://www.apache.org/licenses/LICENSE-2.0                  //
//                                                                  //
// Unless required by applicable law or agreed to in writing,       //
// software distributed under the License is distributed on         //
// an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,  //
// either express or implied. See the License for the specific      //
// language governing permissions and limitations under the License.//
//                                                                  //
//                                                                  //
//                                                                  //
//                                                                  //
//  Author Contact: P.H.Welch@ukc.ac.uk                             //
//                                                                  //
//  Author contact: K.Chalmers@napier.ac.uk                         //
//                                                                  //
//                                                                  //
//////////////////////////////////////////////////////////////////////

using System;
using System.Threading;
using CSPutil;

namespace CSPlang
{
    /**
 * This implements a one-to-one object channel with user-definable buffering.
 * <H2>Description</H2>
 * <TT>BufferedOne2OneChannel</TT> implements a one-to-one object channel with
 * user-definable buffering.  Multiple readers or multiple writers are
 * not allowed -- these are catered for by {@link BufferedAny2OneChannel},
 * {@link BufferedOne2AnyChannel} or {@link BufferedAny2AnyChannel}.
 * <P>
 * The reading process may {@link Alternative <TT>ALT</TT>} on this channel.
 * The writing process is committed (i.e. it may not back off).
 * <P>
 * The constructor requires the user to provide
 * the channel with a <I>plug-in</I> driver conforming to the
 * {@link jcsp.util.ChannelDataStore <TT>ChannelDataStore</TT>}
 * interface.  This allows a variety of different channel semantics to be
 * introduced -- including buffered channels of user-defined capacity
 * (including infinite), overwriting channels (with various overwriting
 * policies) etc..
 * Standard examples are given in the <TT>jcsp.util</TT> package, but
 * <I>careful users</I> may write their own.
 *
 * @see jcsp.lang.Alternative
 * @see jcsp.lang.BufferedAny2OneChannel
 * @see jcsp.lang.BufferedOne2AnyChannel
 * @see jcsp.lang.BufferedAny2AnyChannel
 * @see jcsp.util.ChannelDataStore
 *
 * @author P.D.Austin
 * @author P.H.Welch
 */


    internal class BufferedOne2OneChannel : One2OneChannel, ChannelInternals
    {
        /** The ChannelDataStore used to store the data for the channel */
        private readonly ChannelDataStore data;

        private readonly Object rwMonitor = new Object();

        private Alternative alt;

        /**
         * Constructs a new BufferedOne2OneChannel with the specified ChannelDataStore.
         *
         * @param data the ChannelDataStore used to store the data for the channel
         */
        public BufferedOne2OneChannel(ChannelDataStore data)
        {
            if (data == null)
                throw new ArgumentException
                        ("Null ChannelDataStore given to channel constructor ...\n");
            this.data = (ChannelDataStore)data.clone();
        }

        /**
         * Reads an <TT>Object</TT> from the channel.
         *
         * @return the object read from the channel.
         */
        public Object read()
        {
            /*synchronized*/
            lock (rwMonitor)
            {
                if (data.getState() == ChannelDataStore.EMPTY)
                {
                    try
                    {
                        Monitor.Wait(rwMonitor);
                        while (data.getState() == ChannelDataStore.EMPTY)
                        {
                            if (Spurious.logging)
                            {
                                SpuriousLog.record(SpuriousLog.One2OneChannelXRead);
                            }
                            Monitor.Wait(rwMonitor);
                        }
                    }
                    catch (ThreadInterruptedException e)
                    {
                        throw new ProcessInterruptedException(
                          "*** Thrown from One2OneChannel.read (int)\n" + e.ToString()
                        );
                    }
                }
                Monitor.Pulse(rwMonitor);
                return data.get();
            }
        }

        public Object startRead()
        {
            /*synchronized*/
            lock (rwMonitor)
            {
                if (data.getState() == ChannelDataStore.EMPTY)
                {
                    try
                    {
                        Monitor.Wait(rwMonitor);
                        while (data.getState() == ChannelDataStore.EMPTY)
                        {
                            if (Spurious.logging)
                            {
                                SpuriousLog.record(SpuriousLog.One2OneChannelXRead);
                            }
                            Monitor.Wait(rwMonitor);
                        }
                    }
                    catch (ThreadInterruptedException e)
                    {
                        throw new ProcessInterruptedException(
                          "*** Thrown from One2OneChannel.read (int)\n" + e.ToString()
                        );
                    }
                }

                return data.startGet();
            }
        }

        public void endRead()
        {
            /*synchronized*/
            lock (rwMonitor)
            {
                data.endGet();
                Monitor.Pulse(rwMonitor);
            }
        }

        /**
         * Writes an <TT>Object</TT> to the channel.
         *
         * @param value the object to write to the channel.
         */
        public void write(Object value)
        {
            /*synchronized*/
            lock (rwMonitor)
            {
                data.put(value);
                if (alt != null)
                {
                    alt.schedule();
                }
                else
                {
                    Monitor.Pulse(rwMonitor);
                }
                if (data.getState() == ChannelDataStore.FULL)
                {
                    try
                    {
                        Monitor.Wait(rwMonitor);
                        while (data.getState() == ChannelDataStore.FULL)
                        {
                            if (Spurious.logging)
                            {
                                SpuriousLog.record(SpuriousLog.One2OneChannelXWrite);
                            }
                            Monitor.Wait(rwMonitor);
                        }
                    }
                    catch (ThreadInterruptedException e)
                    {
                        throw new ProcessInterruptedException(
                          "*** Thrown from One2OneChannel.write (Object)\n" + e.ToString()
                        );
                    }
                }
            }
        }

        /**
         * turns on Alternative selection for the channel. Returns true if the
         * channel has data that can be read immediately.
         * <P>
         * <I>Note: this method should only be called by the Alternative class</I>
         *
         * @param alt the Alternative class which will control the selection
         * @return true if the channel has data that can be read, else false
         */
        public Boolean readerEnable(Alternative alt)
        {
            /*synchronized*/
            lock (rwMonitor)
            {
                if (data.getState() == ChannelDataStore.EMPTY)
                {
                    this.alt = alt;
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /**
         * turns off Alternative selection for the channel. Returns true if the
         * channel contained data that can be read.
         * <P>
         * <I>Note: this method should only be called by the Alternative class</I>
         *
         * @return true if the channel has data that can be read, else false
         */
        public Boolean readerDisable()
        {
            /*synchronized*/
            lock (rwMonitor)
            {
                alt = null;
                return data.getState() != ChannelDataStore.EMPTY;
            }
        }

        /**
         * Returns whether there is data pending on this channel.
         * <P>
         * <I>Note: if there is, it won't go away until you read it.  But if there
         * isn't, there may be some by the time you check the result of this method.</I>
         * <P>
         * This method is provided for convenience.  Its functionality can be provided
         * by <I>Pri Alting</I> the channel against a <TT>SKIP</TT> guard, although
         * at greater run-time and syntactic cost.  For example, the following code
         * fragment:
         * <PRE>
         *   if (c.pending ()) {
         *     Object x = c.read ();
         *     ...  do something with x
         *   } else (
         *     ...  do something else
         *   }
         * </PRE>
         * is equivalent to:
         * <PRE>
         *   if (c_pending.priSelect () == 0) {
         *     Object x = c.read ();
         *     ...  do something with x
         *   } else (
         *     ...  do something else
         * }
         * </PRE>
         * where earlier would have had to have been declared:
         * <PRE>
         * final Alternative c_pending =
         *   new Alternative (new Guard[] {c, new Skip ()});
         * </PRE>
         *
         * @return state of the channel.
         */
        public Boolean readerPending()
        {
            /*synchronized*/
            lock (rwMonitor)
            {
                return (data.getState() != ChannelDataStore.EMPTY);
            }
        }

        

        /**
         * Returns the <code>AltingChannelInput</code> to use for this channel.
         * As <code>BufferedOne2OneChannel</code> implements
         * <code>AltingChannelInput</code> itself, this method simply returns
         * a reference to the object that it is called on.
         *
         * @return the <code>AltingChannelInput</code> object to use for this
         *          channel.
         */
        public AltingChannelInput In()
        {
            return new AltingChannelInputImpl(this, 0);
        }

        /**
         * Returns the <code>ChannelOutput</code> object to use for this channel.
         * As <code>BufferedOne2OneChannel</code> implements
         * <code>ChannelOutput</code> itself, this method simply returns
         * a reference to the object that it is called on.
         *
         * @return the <code>ChannelOutput</code> object to use for this
         *          channel.
         */
        public ChannelOutput Out()
        {
            return new ChannelOutputImpl(this, 0);
        }

        //  No poison in these channels:
        public void writerPoison(int strength)
        {
        }
        public void readerPoison(int strength)
        {
        }


        public bool writerEnable(Alternative alt)
        {
            throw new NotImplementedException();
        }

        public bool writerDisable()
        {
            throw new NotImplementedException();
        }

        public bool writerPending()
        {
            throw new NotImplementedException();
        }
    }
}