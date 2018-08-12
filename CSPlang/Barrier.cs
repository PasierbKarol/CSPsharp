using System;

namespace CSPlang
{
    [Serializable]
    public class Barrier
    {
        /**
   * The number of processes currently enrolled on this barrier.
   */
        private int nEnrolled = 0;

        /**
         * The number of processes currently enrolled on this barrier and who have not yet
         * synchronised in this cycle.
         */
        private int countDown = 0;

        /**
         * The monitor lock used for synchronisation.
         */
        private Object barrierLock = new Object();

        /**
         * The even/odd flag used to detect spurious wakeups.
         */
        private boolean evenOddCycle = true;      // could be initialised to false ...

        /**
         * Construct a barrier initially associated with no processes.
         */
        public Barrier()
        {
        }

        /**
         * Construct a barrier (initially) associated with <TT>nEnrolled</TT> processes.
         * It is the responsibility of the constructing process to pass this
         * (by constructor or <tt>set</tt> method) to each process that will be
         * synchronising on the barrier, <i>before</i> firing up those processes.
         * <p>
         *
         * @param nEnrolled the number of processes (initially) associated with this barrier.
         * <p>
         * 
         * @throws IllegalArgumentException if <tt>nEnrolled</tt> < <tt>0</tt>.
         */
        public Barrier(final int nEnrolled)
        {
            if (nEnrolled < 0)
            {
                throw new IllegalArgumentException(
                  "*** Attempt to set a negative enrollment on a barrier\n"
                );
            }
            this.nEnrolled = nEnrolled;
            countDown = nEnrolled;
            //System.out.println ("Barrier.constructor : " + nEnrolled + ", " + countDown);
        }

        /**
         * Reset this barrier to be associated with <TT>nEnrolled</TT> processes.
         * This must only be done at a time when no processes are active on the barrier.
         * It is the responsibility of the invoking process to pass this barrier
         * (by constructor or <tt>set</tt> method) to each process that will be
         * synchronising on the barrier, <i>before</i> firing up those processes.
         * <p>
         *
         * @param nEnrolled the number of processes reset to this barrier.
         * <p>
         * 
         * @throws IllegalArgumentException if <tt>nEnrolled</tt> < <tt>0</tt>.
         */
        public void reset(final int nEnrolled)
        {
            if (nEnrolled < 0)
            {
                throw new IllegalArgumentException(
                  "*** Attempt to set a negative enrollment on a barrier\n"
                );
            }
            synchronized(barrierLock) {
                this.nEnrolled = nEnrolled;
                countDown = nEnrolled;
            }
            //System.out.println ("Barrier.reset : " + nEnrolled + ", " + countDown);
        }

        /**
         * Synchronise the invoking process on this barrier.
         * <I>Any</I> process synchronising on this barrier will be blocked until <I>all</I>
         * processes associated with the barrier have synchronised (or resigned).
         */
        public void sync()
        {
            synchronized(barrierLock) {
                countDown--;
                //System.out.println ("Barrier.sync : " + nEnrolled + ", " + countDown);
                if (countDown > 0)
                {
                    try
                    {
                        boolean spuriousCycle = evenOddCycle;
                        barrierLock.wait();
                        while (spuriousCycle == evenOddCycle)
                        {
                            if (Spurious.logging)
                            {
                                SpuriousLog.record(SpuriousLog.BarrierSync);
                            }
                            barrierLock.wait();
                        }
                    }
                    catch (InterruptedException e)
                    {
                        throw new ProcessInterruptedException(
                      "*** Thrown from Barrier.sync ()\n" + e.toString()
                    );
                    }
                }
                else
                {
                    countDown = nEnrolled;
                    evenOddCycle = !evenOddCycle;         // to detect spurious wakeups  :(
                                                          //System.out.println ("Barrier.sync : " + nEnrolled + ", " + countDown);
                    barrierLock.notifyAll();
                }
            }
        }

        /**
         * A process may enroll only if it is resigned.
         * A re-enrolled process may resume offering to synchronise on this barrier
         * (until a subsequent {@link #resign resign}).
         * Other processes cannot complete the barrier (represented by this front-end)
         * without participation by the re-enrolled process.
         * <p>
         * <i>Note:</i> timing re-enrollment on a barrier usually needs some care.
         * If the barrier is being used for synchronising phases of execution between
         * a set of processes, it is crucial that re-enrollment occurs in
         * an appropriate <i>(not arbitrary)</i> phase.
         * If the trigger for re-enrollment comes from another enrolled process,
         * that process should be in such an appropriate phase.
         * The resigned process should re-enroll and, then, acknowledge the trigger.
         * The triggering process should wait for that acknowledgement.
         * If the decision to re-enroll is internal (e.g. following a timeout),
         * a <i>buddy</i> process, enrolled on the barrier, should be asked to provide
         * that trigger when in an appropriate phase.
         * The <i>buddy</i> process, perhaps specially built just for this purpose, polls
         * a service channel for that question when in that phase.
         * </p>
         * <p>
         * <i>Warning:</i> the rule in the first sentence above is
         * the responsibility of the designer -- it is not checked by
         * implementation.
         * If not honoured, things will go wrong.
         */
        public void enroll()
        {
            synchronized(barrierLock) {
                nEnrolled++;
                countDown++;
            }
            //System.out.println ("Barrier.enroll : " + nEnrolled + ", " + countDown);
        }

        /**
         * A process may resign only if it is enrolled.
         * A resigned process may not offer to synchronise on this barrier
         * (until a subsequent {@link #enroll enroll}).
         * Other processes can complete the barrier (represented by this front-end)
         * without participation by the resigned process.
         * <p>
         * Unless <i>all</i> processes synchronising on this barrier terminate in
         * the same phase, it is usually appropriate for a terminating process
         * to <i>resign</i> first.  Otherwise, its sibling processes will never be
         * able to complete another synchronisation.
         * </p>
         * <p>
         * <i>Warning:</i> the rules in the first two sentences above are
         * the responsibility of the designer -- they are not checked by
         * implementation.
         * If not honoured, things will go wrong.
         * </p>
         * <p>
         * 
         * @throws BarrierError if not enrolled <i>(but this is not always detected)</i>.
         * 
         */
        public void resign()
        {
            synchronized(barrierLock) {
                nEnrolled--;
                countDown--;
                //System.out.println ("Barrier.resign : " + nEnrolled + ", " + countDown);
                if (countDown == 0)
                {
                    countDown = nEnrolled;
                    evenOddCycle = !evenOddCycle;         // to detect spurious wakeups  :(
                                                          //System.out.println ("Barrier.resign : " + nEnrolled + ", " + countDown);
                    barrierLock.notifyAll();
                }
                else if (countDown < 0)
                {
                    throw new BarrierError(
                  "*** A process has resigned on a barrier without first enrolling\n"
                );
                }
            }
        }

    }
}