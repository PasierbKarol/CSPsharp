using System;
using System.Threading;

namespace CSPlang
{
    public class AltingBarrierCoordinate
    {
        /*
   * This records number of processes active in ALT enable/disable sequences
   * involving a barrier.
   * <P>
   * Only one process may be engaged in an enable sequence involving a barrier.
   * <P>
   * Disable sequences, triggered by a successful barrier enable, may happen
   * in parallel.  Disable sequences, triggered by a successful barrier enable,
   * may not happen in parallel with an enable sequence involving a barrier.
   * <P>
   * Disable sequences involving a barrier, triggered by a successful non-barrier
   * enable, may happen in parallel with an enable sequence involving a barrier.
   * Should the enable sequence complete a barrier that is in a disable sequence
   * (which can't yet have been disabled, else it could not have been completed),
   * the completed barrier will be found (when it is disabled) and that disable
   * sequence becomes as though it had been triggered by that successful barrier
   * enable (rather than the non-barrier event).
   */
        private static int active = 0;

        /** Lock object for coordinating enable/disable sequences. */
        private static Object activeLock = new Object();

        /* Invoked at start of an enable sequence involving a barrier. */
        public static void startEnable()
        {
            /*synchronized*/ lock (activeLock) {
                if (active > 0)
                {
                    try
                    {
                        //activeLock.wait();
                        Monitor.Wait(activeLock); //Guessing what should be here. Was empty
                        while (active > 0)
                        {
                            // This may be a spurious wakeup.  More likely, this is a properly
                            // notified wakeup that has been raced to the 'activelock' monitor
                            // by another thread (quite possibly the notifying one) that has
                            // (re-)acquired it and set 'active' greater than zero.  We have
                            // not instrumented the code to tell the difference.  Either way:
                            Monitor.Wait(activeLock); //Guessing what should be here. Was empty
                        }
                    }
                    catch (/*InterruptedException*/  ThreadInterruptedException e)
                    {
                        throw new ProcessInterruptedException(e.ToString());
                    }
                }
                if (active != 0)
                {
                    throw new JCSP_InternalError(
                  "\n*** AltingBarrier enable sequence starting " +
                  "with 'active' count not equal to zero: " + active
                );
                }
                active = 1;
            }
        }

        /* Invoked at finish of an unsuccessful enable sequence involving a barrier. */
        public static void finishEnable()
        {
            /*synchronized*/ lock (activeLock) {
                if (active != 1)
                {
                    throw new JCSP_InternalError(
                  "\n*** AltingBarrier enable sequence finished " +
                  "with 'active' count not equal to one: " + active
                );
                }
                active = 0;
                Monitor.Pulse(activeLock); // Originally was activeLock.notify() - KP

            }
        }

        /*
         * Invoked by a successful barrier enable.
         *
         * @param n The number of processes being released to start their disable sequences.
         */
        public static void startDisable(int n)
        {
            if (n <= 0)
            {
                throw new JCSP_InternalError(
                  "\n*** attempt to start " + n + " disable sequences!"
                );
            }
            /*synchronized*/ lock (activeLock) {               // not necessary ... ?
                if (active != 1)
                {
                    throw new JCSP_InternalError(
                  "\n*** completed AltingBarrier found in ALT sequence " +
                  "with 'active' count not equal to one: " + active
                );
                }
                active = n;
            }
        }

        /* Invoked at finish of a disable sequence selecting a barrier. */
        public static void finishDisable()
        {
            /*synchronized*/ lock (activeLock) {
                if (active < 1)
                {
                    throw new JCSP_InternalError(
                  "\n*** AltingBarrier disable sequence finished " +
                  "with 'active' count less than one: " + active
                );
                }
                active--;
                if (active == 0)
                {
                    Monitor.Pulse(activeLock);
                }
            }
        }
    }
}