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
using System.Threading;

namespace CSPlang
{
    /**
     * This is a process that immediately terminates <I>and</I>
     * a {@link Guard} that is always ready.
     * <H2>Description</H2>
     * <TT>Skip</TT> is a process that starts, engages in no events, performs no
     * computation and terminates.  It can also be used as a {@link Guard} in
     * an <A HREF="Alternative.html#Polling"><TT>Alternative</TT></A>
     * that is always ready.
     * <P>
     * <I>Note: the process form is included for completeness -- it is one of
     * the fundamental primitives of <B>CSP</B>.</I>
     *
     * @see jcsp.lang.Stop
     *
     * @author P.D.Austin
     * @author P.H.Welch
     *
     */

    public class Skip : Guard, IamCSProcess
    {
        /**
         * @param alt the Alternative doing the enabling.
         */
        public override Boolean enable(Alternative alt)
        {
            Thread.Yield();
            return true;
        }

        /**
         * Disables this guard.
         */
        public override Boolean disable()
        {
            return true;
        }

        public void run()
        {
        }
    }
}