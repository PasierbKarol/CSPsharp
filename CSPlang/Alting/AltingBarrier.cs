using System;
using System.Threading;

namespace CSPlang
{
    public class AltingBarrier : Guard
    {
        /** This references the barrier on which this is enrolled.
           */
        AltingBarrierBase baseClass;

        /** Link to the next <i>front-end</i> (used by {@link AltingBarrierBase}). */
        AltingBarrier next = null;

        /** The process offering this barrier (protected by the base monitor). */
        private Alternative alt = null;

        /** Safety check (protected by the base monitor). */
        private Thread myThread = null;

        /** Safety check (protected by the base monitor).
         *  Also package visible (needed by the base.contract(...) method).
         */
        Boolean enrolled = true;

        /** Used to support the {@link #sync() <code>sync</code>} method. */
        private Alternative singleAlt = null;

        /** Used to support the {@link #poll() <code>sync</code>} method. */
        private Alternative pollAlt = null;

        /** Used to support the {@link #poll() <code>sync</code>} method. */
        private CSTimer pollTime = null;

        /** Package-only constructor (used by {@link AltingBarrierBase}). */
        public AltingBarrier (AltingBarrierBase baseClass, AltingBarrier next) : base()
        {
            this.baseClass= baseClass;
            this.next = next;
        }

        /**
         * This creates a new <i>alting</i> barrier with an (initial) enrollment
         * count of <code>n</code>.
         * It provides an array of <code>n</code> <i>front-end</i>s to this barrier.
         * It is the invoker's responsibility to install one of these (by constructor
         * or <code>set</code> method) in each process that will be synchronising on
         * the barrier, <i>before</i> firing up those processes.
         * <p>
         * <i>Note:</i> each process must use a different <i>front-end</i> to
         * the barrier.  Usually, a process retains an <code>AltingBarrier</code>
         * <i>front-end</i> throughout its lifetime -- however, see {@link #mark mark}.
         * <p>
         *
         * @param n the number of processes enrolled (initially) on this barrier.
         * <p>
         *
         * @return an array of <code>n</code> <i>front-end</i>s to this barrier.
         * <p>
         * 
         * @throws IllegalArgumentException if <code>n</code> <= <code>0</code>.
         */
        public static AltingBarrier[] create(int n)
        {
            if (n <= 0)
            {
                throw new /*IllegalArgumentException*/ ArgumentException(
                  "\n*** An AltingBarrier must have at least one process enrolled, not " + n
                );
            }
            return new AltingBarrierBase().expand(n);
        }

        /**
         * This creates a new <i>alting</i> barrier with an (initial) enrollment
         * count of <code>1</code>.
         * It provides a single <i>front-end</i> to the barrier, from which others may
         * be generated (see {@link #expand() expand()}) -- usually <i>one-at-a-time</i>
         * to feed processes individually <i>forked</i> (by a {@link ProcessManager}).
         * It is the invoker's responsibility to install each one (by constructor
         * or <code>set</code> method) in the process that will be synchronising on
         * the barrier, <i>before</i> firing up that process.
         * Usually, a process retains an <code>AltingBarrier</code> <i>front-end</i>
         * throughout its lifetime -- however, see {@link #mark mark}.
         * <p>
         * <i>Note:</i> if a known number of processes needing the barrier are to be run
         * (e.g. by a {@link Parallel}), creating the barrier with an array of
         * <i>front-end</i>s using {@link #create(int) create(n)} would be more convenient.
         * <p>
         *
         * @return a single <i>front-end</i> for this barrier.
         */
        public static AltingBarrier create()
        {
            return new AltingBarrierBase().expand();
        }

        /**
         * This expands the number of processes enrolled in this <i>alting</i> barrier.
         * <p>
         * Use it when an enrolled process is about to go {@link Parallel} itself and
         * some/all of those sub-processes also need to be enrolled.
         * It returns an array new <i>front-end</i>s for this barrier.
         * It is the invoker's responsibility to pass these on to those sub-processes.
         * </p>
         * <p>
         * Note that if there are <code>x</code> sub-processes to be enrolled, this method
         * must be invoked with an argument of <code>(x - 1)</code>.
         * Pass the <i>returned</i> <code>AltingBarrier</code>s to <i>any</i> <code>(x - 1)</code>
         * of those sub-processes.
         * Pass <i>this</i> <code>AltingBarrier</code> to the last one.
         * </p>
         * <p>
         * Before using its given <i>front-end</i> to this barrier, each sub-process
         * must {@link #mark mark} it to take ownership.
         * <i>[Actually, only the sub-process given the original front-end (which
         * may be running in a different thread) really has to do this.]</i>
         * </p>
         * <p>
         * Following termination of the {@link Parallel}, the original process must
         * take back ownership of its original <code>AltingBarrier</code> <i>(loaned to one
         * of the sub-processes, which may have been running on a different thread)</i>
         * by {@link #mark mark}ing it again.
         * </p>
         * <p>
         * Also following termination of the {@link Parallel}, the original process
         * must contract the number of processes enrolled on the barrier.
         * To do this, it must have retained the <i>front-end</i> array returned by
         * this method and pass it to {@link #contract(AltingBarrier[]) contract}.
         * </p>
         * <p>
         *
         * @param n the number of processes to be added to this barrier.
         * <p>
         *
         * @return an array of new <i>front-end</i>s for this barrier.
         * <p>
         * 
         * @throws IllegalArgumentException  if <code>n</code> <= <code>0</code>.
         * <p>
         * 
         * @throws AltingBarrierError if currently resigned or not owner of this
         *   <i>front-end</i>.
         */
        public AltingBarrier[] expand(int n)
        {
            if (n <= 0)
            {
                throw new /*IllegalArgumentException*/ ArgumentException(
                  "\n*** Expanding an AltingBarrier must be by at least one, not " + n
                );
            }
            /*synchronized*/ lock (baseClass) {
                if (myThread == null)
                {
                    myThread = Thread.CurrentThread();
                }
                else if (myThread != Thread.CurrentThread())
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier expand attempted by non-owner."
                    );
                }
                if (!enrolled)
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier expand attempted whilst resigned."
                    );
                }
                return baseClass.expand(n);
            }
        }

        /**
         * This expands by one the number of processes enrolled in this <i>alting</i>
         * barrier.
         * <p>
         * Use it when an enrolled process is about to <i>fork</i> a new process
         * (using {@link ProcessManager}) that also needs to be enrolled.
         * It returns an new <i>front-end</i> for this barrier.
         * It is the invoker's responsibility to pass it to the new process.
         * </p>
         * <p>
         * Before terminating, the <i>forked</i> process should
         * {@link #contract() contract} (by one) the number of processes
         * enrolled in this barrier.
         * Otherwise, no further synchronisations on this barrier would be able
         * to complete.
         * </p>
         * <p>
         *
         * @return a new <i>front-end</i> for this barrier.
         * <p>
         * 
         * @throws AltingBarrierError if currently resigned or not owner of this
         *   <i>front-end</i>.
         */
        public AltingBarrier expand()
        {
            /*synchronized*/ lock (baseClass) {
                if (myThread == null)
                {
                    myThread = Thread.CurrentThread();
                }
                else if (myThread != Thread.CurrentThread())
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier expand attempted by non-owner."
                    );
                }
                if (!enrolled)
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier expand attempted whilst resigned."
                    );
                }
                return baseClass.expand();
            }
        }

        /**
         * This contracts the number of processes enrolled in this <i>alting</i> barrier.
         * The given <i>front-end</i>s are discarded.
         * <p>
         * Use it following termination of a {@link Parallel}, some/all of whose
         * sub-processes were enrolled by being given <i>front-end</i>s
         * returned by {@link #expand(int) expand}.
         * See the documentation for {@link #expand(int) expand}.
         * </p>
         * <p>
         * <i>Warning:</i> only the process that went {@link Parallel} should invoke
         * this method -- never one of the sub-processes.
         * </p>
         * <p>
         * <i>Warning:</i> never invoke this method whilst processes using
         * its argument's <i>front-end</i>s are running.
         * </p>
         * <p>
         * <i>Warning:</i> do not attempt to reuse any of the argument elements afterwards
         * -- they <i>front-end</i> nothing.
         * </p>
         * <p>
         *
         * @param ab the <i>front-ends</i> being discarded from this barrier.
         *   This array must be unaltered from one previously delivered by
         *   an {@link #expand(int) expand}.
         * <p>
         * 
         * @throws IllegalArgumentException if <code>ab</code> is <code>null</code> or zero Length.
         * <p>
         * 
         * @throws AltingBarrierError if the given array is <i>not</i> one previously
         *   delivered by an {@link #expand(int) expand(n)}, or the invoking process is
         *   currently resigned or not the owner of this <i>front-end</i>.
         */
        public void contract(AltingBarrier[] ab)
        {
            if (ab == null)
            {
                throw new /*IllegalArgumentException*/ ArgumentException(
                  "\n*** AltingBarrier contract given a null array."
                );
            }
            if (ab.Length == 0)
            {
                throw new /*IllegalArgumentException*/ ArgumentException(
                  "\n*** AltingBarrier contract given an empty array."
                );
            }
            /*synchronized*/ lock (baseClass) {
                // if (myThread == null) {                         // contract on
                //   myThread = Thread.currentThread ();           // a virgin AltingBarrier
                // }                                               // is an error ???
                // else                                            // (PHW)
                if (myThread != Thread.CurrentThread())
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier contract attempted by non-owner."
                    );
                }
                if (!enrolled)
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier contract attempted whilst resigned."
                    );
                }
                baseClass.contract(ab);
            }
        }

        /**
         * This contracts by one the number of processes enrolled in this <i>alting</i>
         * barrier.
         * This <i>front-end</i> cannot not be used subsequently.
         * <p>
         * This method should be used on individually created <i>front-end</i>s
         * (see {@link #expand() expand()}) when, and only when, the process holding
         * it is about to terminate.
         * Normally, that process would have been <i>forked</i> by the process
         * creating this barrier.
         * </p>
         * <p>
         * <i>Warning:</i> do not try to use this <i>front-end</i> following invocation
         * of this method -- it no longer fronts anything.
         * </p>
         * <p>
         *
         * @throws AltingBarrierError if currently resigned or not the owner of
         *   this <i>front-end</i>.
         */
        public void contract()
        {
            /*synchronized*/ lock (baseClass) {
                // if (myThread == null) {                         // contract on
                //   myThread = Thread.currentThread ();           // a virgin AltingBarrier
                // }                                               // is an error ???
                // else                                            // (PHW)
                if (myThread != Thread.CurrentThread())
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier contract attempted by non-owner."
                    );
                }
                if (!enrolled)
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier contract attempted whilst resigned."
                    );
                }
                baseClass.contract(this);
            }
        }

        //Boolean enable(Alternative a)
        protected override bool enable(Alternative alt)
        {            // package-only visible
            /*synchronized*/ lock (baseClass) {
                if (myThread == null)
                {
                    myThread = Thread.CurrentThread();
                }
                else if (myThread != Thread.CurrentThread())
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier front-end enable by more than one Thread."
                    );
                }
                if (!enrolled)
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier front-end enable whilst resigned."
                    );
                }
                if (alt != null)
                {                      // in case the same barrier
                    return false;                         // occurs more than once in
                }                                       // the same Alternative.
                if (baseClass.enable())
                {
                    a.setBarrierTrigger();               // let Alternative know we did it
                    return true;
                }
                else
                {
                    alt = a;
                    return false;
                }
            }
        }

        //        Boolean disable()
        protected override bool disable()
        {                        // package-only visible
            /*synchronized*/ lock (baseClass) {
                if (alt == null)
                {                      // in case the same barrier
                    return false;                         // occurs more than once in
                }                                       // the same Alternative.
                if (baseClass.disable())
                {
                    alt.setBarrierTrigger();             // let Alternative know we did it
                    alt = null;
                    return true;
                }
                else
                {
                    alt = null;
                    return false;
                }
            }
        }

        /**
         * This is the call-back from a successful 'base.enable'.  If it was us
         * that invoked 'base.enable', our 'alt' is null and we don't need to be
         * scheduled!  If we are resigned, ditto.  Whoever is calling this 'schedule'
         * has the 'base' monitor.
         */
        void schedule()
        {                          // package-only visible
            if (alt != null)
            {
                alt.schedule();
            }
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
         * <i>Note:</i> a process must not transfer its <i>front-end</i> to another
         * process whilst resigned from the barrier -- see {@link #mark mark}.
         * </p>
         * <p>
         * 
         * @throws AltingBarrierError if currently resigned.
         */
        public void resign()
        {
            /*synchronized*/ lock (baseClass) {
                if (!enrolled)
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier.resign() whilst not enrolled."
                    );
                }
                enrolled = false;
                baseClass.resign();
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
         * 
         * @throws AltingBarrierError if currently enrolled.
         */
        public void enroll()
        {
            /*synchronized*/ lock (baseClass) {
                if (enrolled)
                {
                    throw new AltingBarrierError(
                      "\n*** AltingBarrier.enroll() whilst not resigned."
                    );
                }
                enrolled = true;
                baseClass.enroll();
            }
        }

        /**
         * A process may hand its barrier front-end over to another process,
         * but the receiving process must invoke this method before using it.
         * Beware that the process that handed it over must no longer use it.
         * </p>
         * <p>
         * <i>Note:</i> a process must not transfer its <i>front-end</i> to another
         * process whilst resigned from the barrier -- see {@link #resign resign}.
         * The receiving process assumes this is the case.
         * This <i>mark</i> will fail if it is not so.
         * </p>
         * <p>
         * See {@link #expand(int) expand(n)} for an example pattern of use.
         * <p>
         * 
         * @throws AltingBarrierError if the front-end is resigned.
         */
        public void mark()
        {
            /*synchronized*/ lock (baseClass) {
                if (!enrolled)
                {
                    throw new AltingBarrierError(
                      "\n*** Attempt to AltingBarrier.mark() a resigned front-end."
                    );
                }
                myThread = Thread.CurrentThread();
            }
        }

        /**
         * This resets a <i>front-end</i> for reuse.
         * It still fronts the same barrier.
         * Following this method, this front-end is <i>enrolled</i> on the barrier
         * and not <i>owned</i> by any process.
         * </p>
         * <p>
         * <i>Warning:</i> this should only be used to recycle a <i>front-end</i>
         * whose process has terminated.
         * It should not be used to transfer a <i>front-end</i> between running
         * processes (for which {@link #mark mark} should be used).
         * </p>
         * <p>
         * <i>Example</i>:
         * <pre>
         *   AltingBarrier[] action = AltingBarrier.create (n);
         * 
         *   Parallel[] system = new Parallel[n];
         *   for (int i = 0; i < system.Length; i++) {
         *     system[i] = new Something (action[i], ...);
         *   }
         * 
         *   while (true) {
         *     // invariant: all 'action' front-ends are enrolled on the barrier.
         *     // invariant: all 'action' front-ends are not yet <i>owned</i> by any process.
         *     system.run ();
         *     // assume: no 'system' process discards (contracts) its 'action' front-end.
         *     // note: some 'system' processes may have resigned their 'action' front-ends.
         *     // note: in the next run of 'system', its processes may be <i>different</i>
         *     //       from the point of view of the 'action' front-ends.
         *     for (int i = 0; i < action.Length; i++) {
         *       action[i].reset ();
         *     }
         *     // deduce: loop invariant re-established.
         *   }
         * </pre>
         */
        public void reset()
        {
            /*synchronized*/ lock (baseClass) {
                if (!enrolled)
                {
                    enrolled = true;
                    baseClass.enroll();
                }
                myThread = null;
            }
        }

        /**
         * This is a simple way to perform a <i>committed</i> synchonisation on an
         * {@link AltingBarrier} without having to set up an {@link Alternative}.
         * For example, if <code>group</code> is an <code>AltingBarrier</code>, then:
         * <PRE>
         *     group.sync ();
         * </PRE>
         * saves first having to construct the single guarded:
         * <PRE>
         *     Alternative groupCommit = new Alternative (new Guard[] {group});
         * </PRE>
         * and then:
         * <PRE>
         *     groupCommit.select ();
         * </PRE>
         * If this is the only method of synchronisation performed by all parties
         * to this barrier, a <i>non-alting</i> {@link Barrier} would be more efficient.
         * </p>
         * <p>
         * <i>Important note:</i> following a <code>select</code>, <code>priSelect</code> or
         * <code>fairSelect</code> on an {@link Alternative} that returns the index of
         * an <code>AltingBarrier</code>, that barrier synchronisation has happened.
         * Do not proceed to invoke this <code>sync</code> method -- unless, of course,
         * you want to wait for a second synchronisation.
         */
        public void sync()
        {
            if (singleAlt == null)
            {
                singleAlt = new Alternative(new Guard[] { this });
            }
            singleAlt.priSelect();
        }

        /**
         * This is a simple way to <i>poll</i> for synchonisation on an
         * {@link AltingBarrier} without having to set up an {@link Alternative}.
         * The parameter specifies how long this poll should leave its offer
         * to synchronise on the table.
         * If <code>true</code> is returned, the barrier has completed.
         * If <code>false</code>, the barrier was unable to complete within
         * the time specified (i.e. at no time were <i>all</i> parties making
         * an offer).
         * </p>
         * <p>
         * For example, if <code>group</code> is an <code>AltingBarrier</code>, then:
         * <PRE>
         *     if (group.poll (offerTime)) {
         *       ...  group synchronisation achieved
         *     } else {
         *       ...  group synchronisation failed (within offerTime millisecs)
         *     }
         * </PRE>
         * is equivalent to:
         * <PRE>
         *     groupTimer.setAlarm (groupTimer.read () + offerTime);
         *     if (groupPoll.priSelect () == 0) {
         *       ...  group synchronisation achieved
         *     } else {
         *       ...  group synchronisation failed (within offerTime millisecs)
         *     }
         * </PRE>
         * where first would have to have been constructed:
         * <PRE>
         *     CSTimer groupTimer = new CSTimer ();
         *     Alternative groupPoll =
         *       new Alternative (new Guard[] {group, groupTimer});
         * </PRE>
         * <i>Note:</i> polling algorithms should generally be a last resort!
         * If all parties to this barrier only use this method, synchronisation
         * depends on all their poll periods coinciding.
         * An <code>offerTime</code> of zero is allowed: if all other parties
         * are offering, the barrier will complete -- otherwise, the poll returns
         * immediately.
         * However, if more than one party only ever polls like this,
         * no synchronisation will ever take place.
         * </p>
         * <p>
         *
         * @param offerTime the time (in milliseconds) that this offer to synchronise
         *   should be left on the table.
         * <p>
         *
         * @return <code>true</code> if and only if the barrier completes within
         *   time specifed.
         */
        public Boolean poll(long offerTime)
        {
            if (pollAlt == null)
            {
                pollTime = new CSTimer();
                pollAlt = new Alternative(new Guard[] { this, pollTime });
            }
            pollTime.setAlarm(pollTime.read() + offerTime);
            return (pollAlt.priSelect() == 0);
        }




    }
}