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
using System.Diagnostics;
using CSPlang;

namespace PlugAndPlay.Ints
{
    /**
     * This copies its input stream to its output stream, adding a one-place buffer
     * to the stream.
     * <H2>Process Diagram</H2>
     * <p><IMG SRC="doc-files\IdentityInt1.gif"></p>
     * <H2>Description</H2>
     * <TT>IdentityInt</TT> is a process stream whose output stream is the same
     * as its input stream.  The difference between a bare wire and a wire
     * into which an <TT>IdentityInt</TT> process has been spliced is that the
     * latter provides a buffering capacity of <I>one more</I> than the bare wires
     * (which is zero for the default semantics of channels).
     * <P>
     * <H2>Channel Protocols</H2>
     * <TABLE BORDER="2">
     *   <TR>
     *     <TH COLSPAN="3">Input Channels</TH>
     *   </TR>
     *   <TR>
     *     <TH>in</TH>
     *     <TD>int</TD>
     *     <TD>
     *       All channels in this package carry integers.
     *     </TD>
     *   </TR>
     *   <TR>
     *     <TH COLSPAN="3">Output Channels</TH>
     *   </TR>
     *   <TR>
     *     <TH>out</TH>
     *     <TD>int</TD>
     *     <TD>
     *       All channels in this package carry integers.
     *     </TD>
     *   </TR>
     * </TABLE>
     *
     * @author P.D.Austin
     */
    public class IdentityInt : IamCSProcess
    {
        /** The input Channel */
        private ChannelInputInt In;

        /** The output Channel */
        private ChannelOutputInt Out;

        /**
         * Construct a new IdentityInt process with the input Channel in and the
         * output Channel out.
         *
         * @param in the input Channel
         * @param out the output Channel
         */
        public IdentityInt(ChannelInputInt In, ChannelOutputInt Out)
        {
            this.In = In;
            this.Out = Out;
        }

        /**
         * The main body of this process.
         */
        public void run()
        {
            //Console.WriteLine("Inside identity int");
            while (true)
            {
                //Out.write(In.read());
                var a = In.read();
                //Console.WriteLine("Prefix  identityInt is " + a.ToString());
                Out.write(a);
            }
        }
    }
}