using System;
using System.Threading;

namespace CSPlang
{
    public class CSTimer : Guard
    {
        /**
    * The absolute timeout value set for the <TT>Alternative</TT>.
    *
    * If this is used without setAlarm(msecs) ever having been invoked,
    * the wake-up call is set at time zero, which will always be in
    * the past.  So, the <TT>Alternative</TT> will see the timeout
    * as having occurred.
    *
    */
        private long msecs = 0;

        /**
         * Sets the absolute timeout value that will trigger an <TT>Alternative</TT>
         * <I>select</I> operation (when this <TT>CSTimer</TT> is one of the guards
         * with which that <TT>Alternative</TT> was constructed).
         *
         * @param msecs the absolute timeout value.
         */
        public void setAlarm(/*final*/ long msecs)
        {
            this.msecs = msecs;
        }

        /**
         * Returns the alarm value that has been set by the previous call to
         * {@link #setAlarm(long)}.
         */
        public long getAlarm()
        {
            return msecs;
        }

        /**
         * Sets the absolute timeout value that will trigger an <TT>Alternative</TT>
         * <I>select</I> operation (when this <TT>CSTimer</TT> is one of the guards
         * with which that <TT>Alternative</TT> was constructed).
         *
         * @param msecs the absolute timeout value.
         *
         * @deprecated Use {@link #setAlarm(long)} - this name caused confusion with
         * the idea of setting the current time (a concept that is not supported).
         */
        public void set(/*final*/ long msecs)
        {
            this.msecs = msecs;
        }

        /**
         * Returns the current system time in msecs.
         *
         * @return the current system time in msecs
         */
        public long read()
        {
            return System.currentTimeMillis();
        }

        /**
         * Puts the process to sleep until an absolute time is reached.
         *
         * @param msecs the absolute time awaited.  Note: if this time has already been reached, this returns straight away.
         */
        public void after(/*final*/ long msecs)
        {
            /*final*/ long delay = msecs - System.currentTimeMillis();
            if (delay > 0)
                try
                {
                    Thread.Sleep(delay);
                }
                catch (ThreadInterruptedException  e)
                {
                    throw new ProcessInterruptedException
                            ("*** Thrown from CSTimer.after (long)\n" + e.ToString());
                }
        }

        /**
         * Puts the process to sleep for a specified time (milliseconds).
         *
         * @param msecs the length of the sleep period.  Note: if this is negative, this returns straight away.
         */
        public void sleep(/*final*/ long msecs)
        {
            if (msecs > 0)
                try
                {
                    Thread.Sleep(msecs);
                }
                catch (ThreadInterruptedException  e)
                {
                    throw new ProcessInterruptedException
                            ("*** Thrown from CSTimer.sleep (long)\n" + e.ToString());
                }
        }

        /**
         * Enables this guard.
         *
         * @param alt the Alternative doing the enabling.
         */
        Boolean enable(Alternative alt)
        {
            if ((msecs - System.currentTimeMillis()) <= Spurious.earlyTimeout)
            {
                return true;
            }
            else
            {
                alt.setTimeout(msecs);
                return false;
            }
        }

        /**
         * Disables this guard.
         */
        Boolean disable()
        {
            // final long now = System.currentTimeMillis ();
            // System.out.println ("*** CSTimer.disable: " + msecs + ", " + now);
            // return (msecs <= now);
            return ((msecs - System.currentTimeMillis()) <= Spurious.earlyTimeout);
            // WARNING: the above is an insufficient test to see if the timeout
            // has expired ... since Java wait-with-timeouts sometimes return
            // early!  See the implementation of Alternative for a work-around.
        }
    }
}