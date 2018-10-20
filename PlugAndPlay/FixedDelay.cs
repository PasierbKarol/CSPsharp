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
//  Author contact: K.Chalmers@napier.ac.uk                         //
//                                                                  //
//  Author contact: P.H.Welch@ukc.ac.uk                             //
//                                                                  //
//                                                                  //
//////////////////////////////////////////////////////////////////////

using System;
using CSPlang;

namespace PlugAndPlay
{
    /**
     * This holds on to data from its input channel for a fixed delay before passing
     * it on to its output channel.
     * <H2>Process Diagram</H2>
     * <p><img src="doc-files\FixedDelay1.gif"></p>
     * <H2>Description</H2>
     * <TT>FixedDelay</TT> is a process that delays passing on input to its output
     * by a constant delay.
     * <P>
     * <H2>Channel Protocols</H2>
     * <TABLE BORDER="2">
     *   <TR>
     *     <TH COLSPAN="3">Input Channels</TH>
     *   </TR>
     *   <TR>
     *     <TH>in</TH>
     *     <TD>java.lang.Object</TD>
     *     <TD>
     *       The in Channel can accept data of any Class.
     *     </TD>
     *   </TR>
     *   <TR>
     *     <TH COLSPAN="3">Output Channels</TH>
     *   </TR>
     *   <TR>
     *     <TH>out</TH>
     *     <TD>java.lang.Object</TD>
     *     <TD>
     *       The out Channel sends the the same type of data (in
     *       fact, the <I>same</I> data) as is input.
     *     </TD>
     *   </TR>
     * </TABLE>
     *
     * @author P.D.Austin
     */

    public sealed class FixedDelay : IamCSProcess
    {
        /** The input Channel */
        private ChannelInput In;

        /** The output Channel */
        private ChannelOutput Out;

        /**
         * The time the process is to wait in milliseconds between receiving a
         * message and then sending it.
         */
        private long delayTime;

        /**
         * Construct a new FixedDelay process with the input Channel in and the
         * output Channel out.
         *
         * @param delayTime the time the process is to wait in milliseconds
         *   between receiving a message and then sending it (a negative
         *   <TT>delayTime</TT> implies no waiting).
         * @param in the input Channel
         * @param out the output Channel
         */
        public FixedDelay(long delayTime, ChannelInput In, ChannelOutput Out)
        {
            this.In = In;
            this.Out = Out;
            this.delayTime = delayTime;
        }

        /**
         * The main body of this process.
         */
        public void run()
        {
            CSTimer tim = new CSTimer();
            while (true)
            {
                Object o = In.read();
                tim.sleep(delayTime);
                Out.write(o);
            }
        }
    }
}