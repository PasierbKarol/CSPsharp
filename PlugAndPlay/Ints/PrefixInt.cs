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

namespace PlugAndPlay.Ints
{
    /**
     * <I>Prefixes</I> a user-supplied integer to the <TT>int</TT> stream
     * flowing through.
     * <H2>Process Diagram</H2>
     * <p><IMG SRC="doc-files\PrefixInt1.gif"></p>
     * <H2>Description</H2>
     * The output stream from <TT>PrefixInt</TT> is its input stream prefixed
     * by the integer, <TT>n</TT>, with which it is configured.
     * <P>
     * One output is gererated before any input but that,
     * thereafter, one output is produced for each input.
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
    public sealed class PrefixInt : IamCSProcess
    {
        /** The input Channel */
        private ChannelInputInt In;

        /** The output Channel */
        private ChannelOutputInt Out;

        /** The initial int to be sent down the Channel. */
        private int n;

        /**
         * Construct a new PrefixInt process with the input Channel in and the
         * output Channel out.
         *
         * @param n the initial int to be sent down the Channel.
         * @param in the input Channel
         * @param out the output Channel
         */
        public PrefixInt(int n, ChannelInputInt In, ChannelOutputInt Out)
        {
            this.In = In;
            this.Out = Out;
            this.n = n;
        }

        /**
         * The main body of this process.
         */
        public void run()
        {
            Out.write(n);
            //Debug.WriteLine("Prefix  object is " + o.ToString());

            new IdentityInt(In, Out).run();
        }
    }
}