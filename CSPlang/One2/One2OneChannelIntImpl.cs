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
using CSPlang.Alting;
using CSPutil;

namespace CSPlang
{

    /**
     * This implements a one-to-one integer channel.
     * <H2>Description</H2>
     * <TT>One2OneChannelIntImpl</TT> implements a one-to-one integer channel.  Multiple
     * readers or multiple writers are not allowed -- these are catered for
     * by {@link Any2OneChannelIntImpl},
     * {@link One2AnyChannelIntImpl} or
     * {@link Any2AnyChannelIntImpl}.
     * <P>
     * The reading process may {@link Alternative <TT>ALT</TT>} on this channel.
     * The writing process is committed (i.e. it may not back off).
     * <P>
     * The default semantics of the channel is that of CSP -- i.e. it is
     * zero-buffered and fully synchronised.  The reading process must wait
     * for a matching writer and vice-versa.
     * <P>
     * However, the static <TT>create</TT> method allows the user to create
     * a channel with a <I>plug-in</I> driver conforming to the
     * {@link jcsp.util.ints.ChannelDataStoreInt <TT>ChannelDataStoreInt</TT>}
     * interface.  This allows a variety of different channel semantics to be
     * introduced -- including buffered channels of user-defined capacity
     * (including infinite), overwriting channels (with various overwriting
     * policies) etc..
     * Standard examples are given in the <TT>jcsp.util.ints</TT> package, but
     * <I>careful users</I> may write their own.
     * <P>
     * Other static <TT>create</TT> methods allows the user to create fully
     * initialised arrays of channels, including plug-ins if required.
     *
     * @see jcsp.lang.Alternative
     * @see jcsp.lang.Any2OneChannelIntImpl
     * @see jcsp.lang.One2AnyChannelIntImpl
     * @see jcsp.lang.Any2AnyChannelIntImpl
     * @see jcsp.util.ints.ChannelDataStoreInt
     *
     * @author P.D.Austin
     * @author P.H.Welch
     */

    class One2OneChannelIntImpl : ChannelInternalsInt, One2OneChannelInt
    {
    /** The monitor synchronising reader and writer on this channel */
    private Object rwMonitor = new Object();

    /** The (invisible-to-users) buffer used to store the data for the channel */
    private int hold;

    /** The synchronisation flag */
    private Boolean empty = true;

    /** The Alternative class that controls the selection */
    private Alternative alt;

    /** Flag to deal with a spurious wakeup during a write */
    private Boolean spuriousWakeUp = true;

    /*************Methods from One2OneChannelInt******************************/

    /**
     * Returns the <code>AltingChannelInputInt</code> object to use for this
     * channel. As <code>One2OneChannelIntImpl</code> implements
     * <code>AltingChannelInputInt</code> itself, this method simply returns
     * a reference to the object that it is called on.
     *
     * @return the <code>AltingChannelInputInt</code> object to use for this
     *          channel.
     */
    public AltingChannelInputInt In()
    {
        return new AltingChannelInputIntImpl(this, 0);
    }

    /**
     * Returns the <code>ChannelOutputInt</code> object to use for this
     * channel. As <code>One2OneChannelIntImpl</code> implements
     * <code>ChannelOutputInt</code> itself, this method simply returns
     * a reference to the object that it is called on.
     *
     * @return the <code>ChannelOutputInt</code> object to use for this
     *          channel.
     */
    public ChannelOutputInt Out()
    {
        return new ChannelOutputIntImpl(this, 0);
    }

    /**********************************************************************/

    /**
     * Reads an <TT>int</TT> from the channel.
     *
     * @return the integer read from the channel.
     */
    public int read()
    {
        lock (rwMonitor) {
            if (empty)
            {
                empty = false;
                try
                {
                    Monitor.Wait(rwMonitor);
                    while (!empty)
                    {
                        if (Spurious.logging)
                        {
                            SpuriousLog.record(SpuriousLog.One2OneChannelIntRead);
                        }

                        Monitor.Wait(rwMonitor);
                    }
                }
                catch (ThreadInterruptedException e)
                {
                    throw new ProcessInterruptedException(
                        "*** Thrown from One2OneChannelInt.read ()\n" + e.ToString()
                    );
                }
            }
            else
            {
                empty = true;
            }

            spuriousWakeUp = false;
            Monitor.Pulse(rwMonitor);
            return hold;
        }
    }

    public int startRead()
    {
        lock (rwMonitor) {
            if (empty)
            {
                empty = false;
                try
                {
                    Monitor.Wait(rwMonitor);
                    while (!empty)
                    {
                        if (Spurious.logging)
                        {
                            SpuriousLog.record(SpuriousLog.One2OneChannelRead);
                        }

                        Monitor.Wait(rwMonitor);
                    }
                }
                catch (ThreadInterruptedException e)
                {
                    throw new ProcessInterruptedException("*** Thrown from One2OneChannel.read ()\n"
                                                          + e.ToString());
                }
            }
            else
            {
                empty = true;
            }

            return hold;
        }
    }

    public void endRead()
    {
        lock (rwMonitor) {
            spuriousWakeUp = false;
            Monitor.Pulse(rwMonitor);
        }
    }

    /**
     * Writes an <TT>int</TT> to the channel.
     *
     * @param value the integer to write to the channel.
     */
    public void write(int value)
    {
        lock (rwMonitor) {
            hold = value;
            if (empty)
            {
                empty = false;
                if (alt != null)
                {
                    alt.schedule();
                }
            }
            else
            {
                empty = true;
                Monitor.Pulse(rwMonitor);
            }

            try
            {
                Monitor.Wait(rwMonitor);
                while (spuriousWakeUp)
                {
                    if (Spurious.logging)
                    {
                        SpuriousLog.record(SpuriousLog.One2OneChannelIntWrite);
                    }

                    Monitor.Wait(rwMonitor);
                }

                spuriousWakeUp = true;
            }
            catch (ThreadInterruptedException e)
            {
                throw new ProcessInterruptedException(
                    "*** Thrown from One2OneChannelInt.write (int)\n" + e.ToString()
                );
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
        lock (rwMonitor)
        {
            if (empty)
            {
                this.alt = alt;
                return false;
            }
            else
                return true;
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
        lock (rwMonitor)
        {
            alt = null;
            return !empty;
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
     *     int x = c.read ();
     *     ...  do something with x
     *   } else (
     *     ...  do something else
     *   }
     * </PRE>
     * is equivalent to:
     * <PRE>
     *   if (c_pending.priSelect () == 0) {
     *     int x = c.read ();
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
        lock (rwMonitor)
        {
            return !empty;
        }
    }

    /**
     * Creates an array of One2OneChannelInts.
     *
     * @param n the number of channels to create in the array
     * @return the array of One2OneChannelIntImpl
     */
    public static One2OneChannelInt[] create(int n)
    {
        One2OneChannelInt[] channels = new One2OneChannelIntImpl[n];
        for (int i = 0; i < n; i++)
            channels[i] = new One2OneChannelIntImpl();
        return channels;
    }

    /**
     * Creates a One2OneChannelIntImpl using the specified ChannelDataStoreInt.
     *
     * @return the One2OneChannelIntImpl
     */
    public static One2OneChannelInt create(ChannelDataStoreInt store)
    {
        return new BufferedOne2OneChannelIntImpl(store);
    }

    /**
     * Creates an array of One2OneChannelInts using the specified ChannelDataStoreInt.
     *
     * @param n the number of channels to create in the array
     * @return the array of One2OneChannelIntImpl
     */
    public static One2OneChannelInt[] create(int n, ChannelDataStoreInt store)
    {
        One2OneChannelInt[] channels = new One2OneChannelIntImpl[n];
        for (int i = 0; i < n; i++)
            channels[i] = new BufferedOne2OneChannelIntImpl(store);
        return channels;
    }

//  No poison on these channels:
    public void readerPoison(int strength)
    {
    }

    public void writerPoison(int strength)
    {
    }



    }
}