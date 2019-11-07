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
     * Adds <I>one</I> to each <TT>Integer</TT> in the stream flowing through.
     * <H2>Process Diagram</H2>
     * <p><img src="doc-files\Successor1.gif"></p>
     * <H2>Description</H2>
     * <TT>Successor</TT> increments each Integer that flows through it.
     * <P>
     * <H2>Channel Protocols</H2>
     * <TABLE BORDER="2">
     *   <TR>
     *     <TH COLSPAN="3">Input Channels</TH>
     *   </TR>
     *   <TR>
     *     <TH>in</TH>
     *     <TD>java.lang.Number</TD>
     *     <TD>
     *       The Channel can accept data from any subclass of Number.   All values
     *       will be converted to ints.
     *     </TD>
     *   </TR>
     *   <TR>
     *     <TH COLSPAN="3">Output Channels</TH>
     *   </TR>
     *   <TR>
     *     <TH>out</TH>
     *     <TD>java.lang.Integer</TD>
     *     <TD>
     *       The output will always be of type Integer.
     *     </TD>
     *   </TR>
     * </TABLE>
     * <P>
     * <H2>Example</H2>
     * The following example shows how to use the Successor process in a small program.
     * The program also uses some of the other building block processes. The
     * program generates a sequence of numbers and adds one to them and prints
     * this on the screen.
     *
     * <PRE>
     * import jcsp.lang.*;
     * import jcsp.util.*;
     * <I></I>
     * public class SuccessorExample {
     * <I></I>
     *   public static void main (String[] argv) {
     * <I></I>
     *     One2OneChannel a = Channel.createOne2One ();
     *     One2OneChannel b = Channel.createOne2One ();
     * <I></I>
     *     new Parallel (
     *       new CSProcess[] {
     *         new Numbers (a.out ()),
     *         new Successor (a.in (), b.out ()),
     *         new Printer (b.in ())
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

    public sealed class Successor : IamCSProcess
    {
        /** The input Channel */
        private readonly ChannelInput In;

        /** The output Channel */
        private readonly ChannelOutput Out;

        /**
         * Construct a new Successor process with the input Channel in and the
         * output Channel out.
         *
         * @param in the input Channel.
         * @param out the output Channel.
         */
        public Successor(/*final*/ ChannelInput In, /*final*/ ChannelOutput Out)
        {
            this.In = In;
            this.Out = Out;
        }

        /**
         * The main body of this process.
         */
        public void run()
        {
            while (true)
            {
                int i = Int32.Parse(In.read().ToString());
                Out.write(i + 1);
            }
        }
    }
}