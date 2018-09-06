using System;
using System.Threading;
using CSPlang.Alting;
using CSPutil;

namespace CSPlang
{

    class PoisonableBufferedOne2OneChannelInt : One2OneChannelInt, ChannelInternalsInt
    {
        /** The ChannelDataStore used to store the data for the channel */
        private readonly ChannelDataStoreInt data;

        private readonly Object rwMonitor = new Object();

        private Alternative alt;

        //Only passed to channel-ends, not used directly:
        private int immunity;

        private int poisonStrength = 0;

        /**
         * Constructs a new BufferedOne2OneChannel with the specified ChannelDataStore.
         *
         * @param data the ChannelDataStore used to store the data for the channel
         */
        public PoisonableBufferedOne2OneChannelInt(ChannelDataStoreInt data, int _immunity)
        {
            if (data == null)
                throw new ArgumentException
                    ("Null ChannelDataStore given to channel constructor ...\n");
            this.data = (ChannelDataStoreInt)data.clone();
            immunity = _immunity;
        }

        private Boolean isPoisoned()
        {
            return poisonStrength > 0;
        }

        /**
         * Reads an <TT>Object</TT> from the channel.
         *
         * @return the object read from the channel.
         */
        public int read()
        {
            lock (rwMonitor)
            {

                if (data.getState() == ChannelDataStoreInt.EMPTY)
                {
                    //Reader only sees poison if buffer is empty:
                    if (isPoisoned())
                    {
                        throw new PoisonException(poisonStrength);
                    }

                    try
                    {
                        Monitor.Wait(rwMonitor);
                        while (data.getState() == ChannelDataStoreInt.EMPTY && !isPoisoned())
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

                    if (isPoisoned())
                    {
                        throw new PoisonException(poisonStrength);
                    }
                }

                Monitor.Pulse(rwMonitor);
                return data.get();
            }
        }

        public int startRead()
        {
            lock (rwMonitor)
            {

                if (data.getState() == ChannelDataStoreInt.EMPTY)
                {
                    //    	Reader only sees poison if buffer is empty:
                    if (isPoisoned())
                    {
                        throw new PoisonException(poisonStrength);
                    }

                    try
                    {
                        Monitor.Wait(rwMonitor);
                        while (data.getState() == ChannelDataStoreInt.EMPTY && !isPoisoned())
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

                    //    Reader only sees poison if buffer is empty:
                    if (isPoisoned())
                    {
                        throw new PoisonException(poisonStrength);
                    }
                }

                return data.startGet();
            }
        }

        public void endRead()
        {
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
        public void write(int value)
        {
            lock (rwMonitor)
            {
                //Writer always sees poison:
                if (isPoisoned())
                {
                    throw new PoisonException(poisonStrength);
                }

                data.put(value);
                if (alt != null)
                {
                    alt.schedule();
                }
                else
                {
                    Monitor.Pulse(rwMonitor);
                }

                if (data.getState() == ChannelDataStoreInt.FULL)
                {
                    try
                    {
                        Monitor.Wait(rwMonitor);
                        while (data.getState() == ChannelDataStoreInt.FULL && !isPoisoned())
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

                    if (isPoisoned())
                    {
                        throw new PoisonException(poisonStrength);
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
            lock (rwMonitor)
            {
                if (isPoisoned())
                {
                    //If it's poisoned, it will be ready whether because of the poison, or because
                    //the buffer has data in it
                    return true;
                }
                else if (data.getState() == ChannelDataStoreInt.EMPTY)
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
            lock (rwMonitor)
            {
                alt = null;
                return data.getState() != ChannelDataStoreInt.EMPTY || isPoisoned();
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
         * readonly Alternative c_pending =
         *   new Alternative (new Guard[] {c, new Skip ()});
         * </PRE>
         *
         * @return state of the channel.
         */
        public Boolean readerPending()
        {
            lock (rwMonitor)
            {
                return (data.getState() != ChannelDataStoreInt.EMPTY) || isPoisoned();
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
        public AltingChannelInputInt In()
        {
            return new AltingChannelInputIntImpl(this, immunity);
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
        public ChannelOutputInt Out()
        {
            return new ChannelOutputIntImpl(this, immunity);
        }

        public void writerPoison(int strength)
        {
            if (strength > 0)
            {
                lock (rwMonitor)
                {
                    this.poisonStrength = strength;

                    //Poison by writer does *NOT* clear the buffer

                    Monitor.PulseAll(rwMonitor);

                    if (null != alt)
                    {
                        alt.schedule();
                    }
                }
            }
        }

        public void readerPoison(int strength)
        {
            if (strength > 0)
            {
                lock (rwMonitor)
                {
                    this.poisonStrength = strength;

                    //Poison by reader clears the buffer:
                    data.removeAll();

                    Monitor.PulseAll(rwMonitor);
                }
            }
        }
    }
}