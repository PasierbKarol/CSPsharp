using CSPutil;

namespace CSPlang
{
    internal class BufferedOne2OneChannel : One2OneChannel, ChannelInternals
    {
        /** The ChannelDataStore used to store the data for the channel */
        private final ChannelDataStore data;
    
    private final Object rwMonitor = new Object();

        private Alternative alt;

        /**
         * Constructs a new BufferedOne2OneChannel with the specified ChannelDataStore.
         *
         * @param data the ChannelDataStore used to store the data for the channel
         */
        public BufferedOne2OneChannel(ChannelDataStore data)
        {
            if (data == null)
                throw new IllegalArgumentException
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
            synchronized(rwMonitor) {
                if (data.getState() == ChannelDataStore.EMPTY)
                {
                    try
                    {
                        rwMonitor.wait();
                        while (data.getState() == ChannelDataStore.EMPTY)
                        {
                            if (Spurious.logging)
                            {
                                SpuriousLog.record(SpuriousLog.One2OneChannelXRead);
                            }
                            rwMonitor.wait();
                        }
                    }
                    catch (InterruptedException e)
                    {
                        throw new ProcessInterruptedException(
                          "*** Thrown from One2OneChannel.read (int)\n" + e.toString()
                        );
                    }
                }
                rwMonitor.notify();
                return data.get();
            }
        }

        public Object startRead()
        {
            synchronized(rwMonitor) {
                if (data.getState() == ChannelDataStore.EMPTY)
                {
                    try
                    {
                        rwMonitor.wait();
                        while (data.getState() == ChannelDataStore.EMPTY)
                        {
                            if (Spurious.logging)
                            {
                                SpuriousLog.record(SpuriousLog.One2OneChannelXRead);
                            }
                            rwMonitor.wait();
                        }
                    }
                    catch (InterruptedException e)
                    {
                        throw new ProcessInterruptedException(
                          "*** Thrown from One2OneChannel.read (int)\n" + e.toString()
                        );
                    }
                }

                return data.startGet();
            }
        }

        public void endRead()
        {
            synchronized(rwMonitor) {
                data.endGet();
                rwMonitor.notify();
            }
        }

        /**
         * Writes an <TT>Object</TT> to the channel.
         *
         * @param value the object to write to the channel.
         */
        public void write(Object value)
        {
            synchronized(rwMonitor) {
                data.put(value);
                if (alt != null)
                {
                    alt.schedule();
                }
                else
                {
                    rwMonitor.notify();
                }
                if (data.getState() == ChannelDataStore.FULL)
                {
                    try
                    {
                        rwMonitor.wait();
                        while (data.getState() == ChannelDataStore.FULL)
                        {
                            if (Spurious.logging)
                            {
                                SpuriousLog.record(SpuriousLog.One2OneChannelXWrite);
                            }
                            rwMonitor.wait();
                        }
                    }
                    catch (InterruptedException e)
                    {
                        throw new ProcessInterruptedException(
                          "*** Thrown from One2OneChannel.write (Object)\n" + e.toString()
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
        public boolean readerEnable(Alternative alt)
        {
            synchronized(rwMonitor) {
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
        public boolean readerDisable()
        {
            synchronized(rwMonitor) {
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
        public boolean readerPending()
        {
            synchronized(rwMonitor) {
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
        public AltingChannelInput in()
    {
        return new AltingChannelInputImpl(this,0);
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
    public ChannelOutput out()
    {
        return new ChannelOutputImpl(this,0);
}

//  No poison in these channels:
public void writerPoison(int strength)
{
}
public void readerPoison(int strength)
{
}

    }
}