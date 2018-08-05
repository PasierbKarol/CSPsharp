using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CSPlang
{
    class One2OneChannelImpl : One2OneChannel, ChannelInternals
    {
        /** The monitor synchronising reader and writer on this channel */
        private Object rwMonitor = new Object();

        /** The (invisible-to-users) buffer used to store the data for the channel */
        private Object hold;

        /** The synchronisation flag */
        private Boolean empty = true;

        /** The Alternative class that controls the selection */
        private Alternative alt;

        /** Flag to deal with a spurious wakeup during a write */
        private Boolean spuriousWakeUp = true;

        /*************Methods from One2OneChannel******************************/

        /**
         * Returns the <code>AltingChannelInput</code> to use for this channel.
         * As <code>One2OneChannelImpl</code> implements
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
         * As <code>One2OneChannelImpl</code> implements
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

        /*************Methods from ChannelOutput*******************************/

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
                            SpuriousLog.record(SpuriousLog.One2OneChannelWrite);
                        }
                        Monitor.Wait(rwMonitor);

                    }
                    spuriousWakeUp = true;
                }
                catch (/*InterruptedException*/  ThreadInterruptedException e)
                {
                    throw new ProcessInterruptedException(
                        "*** Thrown from One2OneChannel.write (Object)\n" + e.ToString());
                }
            }
        }

        /** ***********Methods from AltingChannelInput************************* */

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
                    catch (/*InterruptedException*/  ThreadInterruptedException e)
                    {
                        throw new ProcessInterruptedException("*** Thrown from One2OneChannel.read ()\n"
                                                           + e.ToString());
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

        public Object startRead()
        {
            /*synchronized*/
            lock (rwMonitor)
            {
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
                            //rwMonitor.wait();
                            Monitor.Wait(rwMonitor);
                        }
                    }
                    catch (/*InterruptedException*/  ThreadInterruptedException e)
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
            /*synchronized*/
            lock (rwMonitor)
            {
                spuriousWakeUp = false;
                //rwMonitor.notify();
                Monitor.Pulse(rwMonitor);
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
                if (empty)
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
                return !empty;
            }
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

        //No poison in these channels:
        public void writerPoison(int strength)
        {
        }
        public void readerPoison(int strength)
        {
        }


    }
}
