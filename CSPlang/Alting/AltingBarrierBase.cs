/*************************************************************************
*                                                                        *
*  JCSP ("CSP for Java") libraries                                       *
*  Copyright (C) 1996-2006 Peter Welch and Paul Austin.                  *
*                                                                        *
*  This library is free software; you can redistribute it and/or         *
*  modify it under the terms of the GNU Lesser General Public            *
*  License as published by the Free Software Foundation; either          *
*  version 2.1 of the License, or (at your option) any later version.    *
*                                                                        *
*  This library is distributed in the hope that it will be useful,       *
*  but WITHOUT ANY WARRANTY; without even the implied warranty of        *
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU     *
*  Lesser General Public License for more details.                       *
*                                                                        *
*  You should have received a copy of the GNU Lesser General Public      *
*  License along with this library; if not, write to the Free Software   *
*  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  *
*  USA.                                                                  *
*                                                                        *
*  Author contact: P.H.Welch@ukc.ac.uk                                   *
*                                                                        *
*************************************************************************/

using System;

namespace CSPlang
{
    public class AltingBarrierBase
    {
    /**
    * All front-ends are chained off here.  Each process enrolled must have one,
    * and only one, of these.
    */
        private AltingBarrier frontEnds = null;

        /** The number of processes enrolled. */
        private int enrolled = 0;

        /** The number of processes not yet offered to sync on this barrier. */
        private int countdown = 0;

        /*
         * This creates, and returns, more front-ends to be held by newly enrolling
         * processes.  Initially, none exist - so this
         * (or {@link #expand()}) must be called at least once.
         * <p>
         * <i>Note: except for the first time, this method should only be called by
         * an AltingBarrier synchronised on this AltingBarrierBase.</I>
         * <p>
         * 
         * @param n the number of front-ends to be created.
         * <p>
         *
         * @return the new front-ends.
         * 
         */
        public AltingBarrier[] expand(int index)
        {
            AltingBarrier[] altingBarrier = new AltingBarrier[index];
            for (int i = 0; i < index; i++)
            {
                frontEnds = new AltingBarrier(this, frontEnds);
                altingBarrier[i] = frontEnds;
            }
            enrolled += index;
            countdown += index;
            return altingBarrier;
        }

        /*
         * This creates, and returns, another front-end to be held by a newly enrolling
         * process.  Initially, none exist - so this
         * (or {@link #expand(int)}) must be called at least once.
         * <p>
         * <i>Note: except for the first time, this method should only be called by
         * an AltingBarrier synchronised on this AltingBarrierBase.</I>
         * <p>
         *
         * @return the new front-ends.
         * 
         */
        internal AltingBarrier expand()
        {
            enrolled++;
            countdown++;
            frontEnds = new AltingBarrier(this, frontEnds);
            return frontEnds;
        }

        /**
         * This removes the given <i>front-ends</i> chained to this <i>alting</i> barrier.
         * It also nulls all of them - to prevent any attempted reuse!
         * <p>
         * <i>Note: this method should only be called by an AltingBarrier synchronised
         * on this AltingBarrierBase.</I>
         * <p>
         *
         * @param ab the <i>front-ends</i> being discarded from this barrier.
         *   This array must be unaltered from one previously delivered by
         *   an {@link #expand expand}.
         */
        internal void contract(AltingBarrier[] altingBarriers)
        {
            // assume: (ab != null) && (ab.Length > 0)
            AltingBarrier first = altingBarriers[0];

            // counts the number of front-ends whose (hopefully terminated) processes
            // were still enrolled.
            int discard = 0;

            AltingBarrier previousFrontBarrier = null;
            AltingBarrier nextFrontBarrier = frontEnds;
            while ((nextFrontBarrier != null) && (nextFrontBarrier != first))
            {
                previousFrontBarrier = nextFrontBarrier;
                nextFrontBarrier = nextFrontBarrier.next;
            }

            if (nextFrontBarrier == null)
            {
                throw new AltingBarrierError(
                  "\n*** Could not find first front-end in AltingBarrier contract."
                );
            }

            // Below, we will null elements of "ab" as we pass though the array.
            // However, the formal "deduce" and "invariant" comments that follow
            // relate to code that does not do this.  This is safe since the logic
            // of the code does not depend on values of "ab", subsequent to their
            // anullment.

            altingBarriers[0].baseClass = null;
            altingBarriers[0] = null;

            // deduce: (fb == ab[0]) && (fb != null)
            // deduce: (fa == null) || (fa.next == fb)
            // deduce: (fa == null) <==> (frontEnds == ab[0])
            // deduce: (fa != null) <==> (fa.next == ab[0])

            for (int i = 1; i < altingBarriers.Length; i++)
            {
                // invariant: (fb == ab[i-1]) && (fb != null)
                if (nextFrontBarrier.enrolled)
                    discard++;
                nextFrontBarrier = nextFrontBarrier.next;
                if (nextFrontBarrier == null)
                {
                    throw new AltingBarrierError(
                      "\n*** Could not find second (or later) front-end in AltingBarrier contract."
                    );
                }
                if (nextFrontBarrier != altingBarriers[i])
                {
                    throw new AltingBarrierError(
                      "\n*** Removal array in AltingBarrier contract not one delivered by expand."
                    );
                }
                // deduce: (fb == ab[i]) && (fb != null)
                altingBarriers[i].baseClass = null;
                altingBarriers[i] = null;
            }

            // deduce: (fb == ab[(ab.Length) - 1]) && (fb != null)

            if (nextFrontBarrier.enrolled)
                discard++;

            // deduce: (fa == null) <==> (frontEnds == ab[0])   [NO CHANGE]
            // deduce: (fa != null) <==> (fa.next == ab[0])     [NO CHANGE]

            if (previousFrontBarrier == null)
            {
                frontEnds = nextFrontBarrier.next;
            }
            else
            {
                previousFrontBarrier.next = nextFrontBarrier.next;
            }

            enrolled -= discard;
            countdown -= discard;
            if (countdown == 0)
            {
                countdown = enrolled;
                if (enrolled > 0)
                {
                    AltingBarrierCoordinate.startEnable();
                    AltingBarrierCoordinate.startDisable(enrolled);
                    AltingBarrier frontEndBarrier = frontEnds;
                    while (frontEndBarrier != null)
                    {
                        frontEndBarrier.schedule();
                        frontEndBarrier = frontEndBarrier.next;
                    }
                }
            }
            else if (countdown < 0)
            {
                throw new JCSP_InternalError(
                  "Please report the circumstances to jcsp-team@kent.ac.uk - thanks!"
                );
            }

        }

        /**
         * This removes the given <i>front-end</i> chained to this <i>alting</i> barrier.
         * It also nulls its reference to this base - to prevent any attempted reuse!
         * <p>
         * <i>Note: this method should only be called by an AltingBarrier synchronised
         * on this AltingBarrierBase.</I>
         * <p>
         *
         * @param ab the <i>front-end</i> being discarded from this barrier.
         *   This array must be unaltered from one previously delivered by
         *   an {@link #expand expand}.
         */
        internal void contract(AltingBarrier altingBarrier)
        {
            // assume: (ab != null)
            AltingBarrier previousFrontBarrier = null;
            AltingBarrier nextFrontBarrier = frontEnds;
            while ((nextFrontBarrier != null) && (nextFrontBarrier != altingBarrier))
            {
                previousFrontBarrier = nextFrontBarrier;
                nextFrontBarrier = nextFrontBarrier.next;
            }

            if (nextFrontBarrier == null)
            {
                throw new AltingBarrierError(
                  "\n*** Could not find front-end in AltingBarrier contract."
                );
            }

            // deduce: (fb == ab) && (fb != null)
            // deduce: (fa == null) || (fa.next == fb)
            // deduce: (fa == null) <==> (frontEnds == ab)
            // deduce: (fa != null) <==> (fa.next == ab)

            if (previousFrontBarrier == null)
            {
                frontEnds = nextFrontBarrier.next;
            }
            else
            {
                previousFrontBarrier.next = nextFrontBarrier.next;
            }

            altingBarrier.baseClass = null;

            if (altingBarrier.enrolled)
            {
                enrolled--;
                countdown--;
            }
            if (countdown == 0)
            {
                countdown = enrolled;
                if (enrolled > 0)
                {
                    AltingBarrierCoordinate.startEnable();
                    AltingBarrierCoordinate.startDisable(enrolled);
                    AltingBarrier frontEndBarrier = frontEnds;
                    while (frontEndBarrier != null)
                    {
                        frontEndBarrier.schedule();
                        frontEndBarrier = frontEndBarrier.next;
                    }
                }
            }
            else if (countdown < 0)
            {
                throw new JCSP_InternalError(
                  "Please report the circumstances to jcsp-team@kent.ac.uk - thanks!"
                );
            }

        }

        /**
         * Record the offer to synchronise.
         * <P>
         * <I>Note: this method should only be called by an AltingBarrier synchronised
         * on this AltingBarrierBase.</I>
         * <p>
         *
         * @return true if all the offers are in.
         */
        internal Boolean enable()
        {
            countdown--;
            if (countdown == 0)
            {
                countdown = enrolled;
                AltingBarrierCoordinate.startDisable(enrolled);
                AltingBarrier frontEndBarrier = frontEnds;
                while (frontEndBarrier != null)
                {
                    frontEndBarrier.schedule();
                    frontEndBarrier = frontEndBarrier.next;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * Withdraw the offer to synchronise.
         * <P>
         * <I>Note: this method should only be called by an AltingBarrier synchronised
         * on this AltingBarrierBase.</I>
         * <p>
         *
         * @return true all the offers are in.
         */
        internal Boolean disable()
        {
            if (countdown == enrolled)
            {
                return true;
            }
            else
            {
                countdown++;
                return false;
            }
        }

        /**
         * Record resignation.
         * <p>
         * <I>Note: this method should only be called by an AltingBarrier synchronised
         * on this AltingBarrierBase.</I>
         * <p>
         */
        internal void resign()
        {
            enrolled--;
            countdown--;
            if (countdown == 0)
            {
                countdown = enrolled;
                if (enrolled > 0)
                {
                    AltingBarrierCoordinate.startEnable();
                    AltingBarrierCoordinate.startDisable(enrolled);
                    AltingBarrier frontEndBarrier = frontEnds;
                    while (frontEndBarrier != null)
                    {
                        frontEndBarrier.schedule();
                        frontEndBarrier = frontEndBarrier.next;
                    }
                }
            }
        }

        /**
         * Record re-enrollment.
         * <p>
         * <I>Note: this method should only be called by an AltingBarrier synchronised
         * on this AltingBarrierBase.</I>
         */
        internal void enroll()
        {
            enrolled++;
            countdown++;
        }
    }
}
