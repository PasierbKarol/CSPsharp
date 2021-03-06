﻿//////////////////////////////////////////////////////////////////////
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
using System.Diagnostics;
using System.Threading;
using CSPutil;

namespace CSPlang
{
    /**
 * This is a {@link Guard} for setting timeouts in an {@link Alternative}.
 * <H2>Description</H2>
 * <TT>CSTimer</TT> is a {@link Guard} for setting timeouts in an
 * {@link Alternative}.  It also provides the current system time
 * and can set straight (i.e. committed) timeouts.  The timeouts
 * are in terms of <I>absolute time values</I> - not <I>relative delays</I>.
 * <P>
 * <I>Note: for those familiar with the <I><B>occam</B></I> multiprocessing
 * language, </I><TT>CSTimer</TT><I> gives the semantics of the
 * </I><TT>TIMER</TT><I> type (including its use as a guard in an
 * </I><TT>ALT</TT><I> construct).</I>
 * <P>
 * Warning: a <TT>CSTimer</TT> records the timeout value for use by an
 * {@link Alternative}.  Therefore, <I>different</I> <TT>CSTimer</TT>s
 * must be used by <I>different</I> processes - the same <TT>CSTimer</TT>
 * must not be shared.
 * <P>
 * <I>Implementation note: all </I><TT>CSTimer</TT><I>s currently
 * use the same </I><TT>System.currentTimeMillis</TT><I> time.</I>
 * </P>
 * <H2>Examples</H2>
 * The use of a <TT>CSTimer</TT> for setting timeouts on channel input is documented
 * in the {@link Alternative} class (see the examples
 * <I>A Fair Multiplexor with a Timeout</I>
 * and <I>A Simple Traffic Flow Regulator</I>).
 * <P>
 * Here, we just show its use for setting committed timeouts.  <TT>Regular</TT>
 * generates a regular stream of output on its <TT>out</TT> channel.  The rate
 * of output is determined by its <TT>interval</TT> parameter.  Recall that timeouts
 * implemented by <TT>CSTimer</TT> are in terms of <I>absolute time values</I>.
 * Notice that the sequence of output times maintains
 * an arithmetic progression.  Any delays in completing each cycle (e.g. caused by
 * the process scheduler or the lateness of the process synchronising with us to accept
 * our data) will be compensated for automatically - the output sequence always returns
 * to its planned schedule whenever it can.
 * <PRE>
 * import jcsp.lang.*;
 *  <I></I>
 * public class Regular implements CSProcess {
 *  <I></I>
 *   final private ChannelOutput out;
 *   final private Integer N;
 *   final private long interval;
 *  <I></I>
 *   public Regular (final ChannelOutput out, final int n, final long interval) {
 *     this.out = out;
 *     this.N = new Integer (n);
 *     this.interval = interval;
 *   }
 *  <I></I>
 *   public void run () {
 *  <I></I>
 *     final CSTimer tim = new CSTimer ();
 *     long timeout = tim.read ();       // read the (absolute) time once only
 *  <I></I>
 *     while (true) {
 *       out.write (N);
 *       timeout += interval;            // set the next (absolute) timeout
 *       tim.after (timeout);            // wait until that (absolute) timeout
 *     }
 *   }
 *  <I></I>
 * }
 * </PRE>
 *
 * For convenience, a {@link #sleep <TT>sleep</TT>} method that blocks for a specified
 * time period (in milliseconds) is also provided.  This has the same semantics as
 * {@link java.lang.Thread#sleep(long) <TT>java.lang.Thread.sleep</TT>}.
 * [<I>Note:</I> programming a regular sequence of events is a little easier using
 * {@link #after <TT>after</TT>} (as in the above) rather than {@link #sleep <TT>sleep</TT>}.]
 *
 * @see jcsp.lang.Alternative
 * @see jcsp.lang.Guard
 *
 * @author P.D.Austin
 * @author P.H.Welch
 */

    public class CSTimer : Guard
    {
        /** Variable used to read the Java style time in miliseconds since 1970
         *  It was used in JCSP and copied into this library in similar manner.
         *  Used in method CurrentTimeMillis() to read time passed since that date
         *  Author: Karol Pasierb
         */
        private static readonly DateTime Jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /**
        * The absolute timeout value set for the <TT>Alternative</TT>.
        *
        * If this is used without setAlarm(msecs) ever having been invoked,
        * the wake-up call is set at time zero, which will always be in
        * the past.  So, the <TT>Alternative</TT> will see the timeout
        * as having occurred.        
        */

        private long msecs = 0;

        /**
         * Sets the absolute timeout value that will trigger an <TT>Alternative</TT>
         * <I>select</I> operation (when this <TT>CSTimer</TT> is one of the guards
         * with which that <TT>Alternative</TT> was constructed).
         *
         * @param msecs the absolute timeout value.
         */
        public void setAlarm( /*final*/ long msecs)
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
        public void setAbsoluteTimeout( /*final*/ long msecs)
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
            return CurrentTimeMillis();
        }

        /**
         * Puts the process to sleep until an absolute time is reached.
         *
         * @param msecs the absolute time awaited.  Note: if this time has already been reached, this returns straight away.
         */
        public void after( /*final*/ long msecsReceived)
        {
            long timeRead = CurrentTimeMillis();
            /*final*/
            long delay = msecsReceived - timeRead;
            if (delay > 0)
                try
                {
                    Thread.Sleep((int) delay);
                }
                catch (ThreadInterruptedException e)
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
        public void sleep( /*final*/ long msecs)
        {
            if (msecs > 0)
                try
                {
                    Thread.Sleep((int) msecs);
                }
                catch (ThreadInterruptedException e)
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
        public override Boolean enable(Alternative alt)
        {
            if (CurrentTimeMillis() > msecs)
            {
                return true;
            }

            alt.setTimeout((int) msecs);
            return false;
        }

        public override Boolean disable()
        {
            return (CurrentTimeMillis() > msecs);
            // WARNING: the above is an insufficient test to see if the timeout
            // has expired ... since Java wait-with-timeouts sometimes return
            // early!  See the implementation of Alternative for a work-around.
        }

        public static long CurrentTimeMillis()
        {
            return (long) (DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}