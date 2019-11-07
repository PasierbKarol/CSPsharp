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

using CSPlang;

namespace PlugAndPlay.Ints
{
    /**
     * Adds <I>one</I> to each integer in the stream flowing through.
     * <H2>Process Diagram</H2>
     * <p><IMG SRC="doc-files\SuccessorInt1.gif"></p>
     * <H2>Description</H2>
     * <TT>SuccessorInt</TT> increments each integer that flows through it.
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
     * <P>
     * <H2>Example</H2>
     * The following example shows how to use the SuccessorInt process in a small program.
     * The program also uses some of the other plugNplay processes. The
     * program generates a sequence of numbers, adds one to them and prints
     * this on the screen.
     *
     * <PRE>
     * import jcsp.lang.*;
     * import jcsp.util.ints.*;
     * <I></I>
     * public final class SuccessorIntExample {
     * <I></I>
     *   public static void main (String[] argv) {
     * <I></I>
     *     final One2OneChannelInt a = ChannelInt.createOne2One ();
     *     final One2OneChannelInt b = ChannelInt.createOne2One ();
     * <I></I>
     *     new Parallel (
     *       new CSProcess[] {
     *         new NumbersInt (a.out ()),
     *         new SuccessorInt (a.in (), b.out ()),
     *         new PrinterInt (b.in ())
     *       }
     *     ).run ();
     * <I></I>
     *   }
     * <I></I>
     * }
     * </PRE>
     *
     * @author P.D.Austin
     */

    public sealed class SuccessorInt : IamCSProcess
    {
        private ChannelInputInt In;
        private ChannelOutputInt Out;

        /**
         * Construct a new SuccessorInt process with the input Channel in and the
         * output Channel out.
         *
         * @param in the input Channel
         * @param out the output Channel
         */
        public SuccessorInt(ChannelInputInt In, ChannelOutputInt Out)
        {
            this.In = In;
            this.Out = Out;
        }

        public void run()
        {
            while (true)
            {
                Out.write(In.read() + 1);
            }
        }
    }
}