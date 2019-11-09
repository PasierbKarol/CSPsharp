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

namespace CSPlang
{
    /**
     * This is the super-class for all {@link Alternative} events selectable by a process.
     * <H2>Description</H2>
     * <TT>Guard</TT> defines an abstract interface to be implemented by events competing
     * for selection by a process executing an {@link Alternative}.  Its methods have
     * only <I>package</I> visibility within <TT>jcsp.lang</TT> and are of no concern to
     * <I>users</I> of this package.  Currently, JCSP supports channel inputs, accepts,
     * timeouts and skips as guards.
     * <P>
     * <I>Note: for those familiar with the <I><B>occam</B></I> multiprocessing
     * language, classes implementing </I><TT>Guard</TT><I> correspond to process
     * guards for use within </I><TT>ALT</TT><I> constructs.</I>
     *
     * @see jcsp.lang.CSTimer
     * @see jcsp.lang.Skip
     * @see jcsp.lang.AltingChannelInput
     * @see jcsp.lang.AltingChannelInputInt
     * @see jcsp.lang.Alternative
     * @author P.D.Austin
     * @author P.H.Welch
     */

    public abstract class Guard
    {
        /**
         * Returns true if the event is ready.  Otherwise, this enables the guard
         * for selection and returns false.
         * <P>
         * <I>Note: this method should only be called by the Alternative class</I>
         *
         * @param alt the Alternative class that is controlling the selection
         * @return true if and only if the event is ready
         */
        /*protected*/
        public abstract Boolean enable(Alternative alt);

        /**
         * Disables the guard for selection. Returns true if the event was ready.
         * <P>
         * <I>Note: this method should only be called by the Alternative class</I>
         *
         * @return true if and only if the event was ready
         */
        /*protected*/
        public abstract Boolean disable();

        /**
         * Schedules the process performing the given Alternative to run again.
         * This is intended for use by advanced users of the library who want to
         * create their own Guards that are not in the jcsp.lang package.
         * 
         * @param alt The Alternative to schedule
         */
        /*protected*/
        public void schedule(Alternative alt)
        {
            alt.schedule();
        }
    }
}
