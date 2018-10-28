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
     * <I>Sums</I> two <TT>Integer</TT> streams to one stream.
     *
     * <H2>Process Diagram</H2>
     * <p><img src="doc-files\Plus1.gif"></p>
     * <H2>Description</H2>
     * The Plus class is a process which has an infinite loop that waits for
     * a Object of type Number to be sent down each of the in1 and in2 Channels.
     * The process then calculates the Plus operation on the intValue() of the
     * two Numbers then write the result as a new Integer down the out Channel.
     * <P>
     * <H2>Channel Protocols</H2>
     * <TABLE BORDER="2">
     *   <TR>
     *     <TH COLSPAN="3">Input Channels</TH>
     *   </TR>
     *   <TR>
     *     <TH>in1,in2</TH>
     *     <TD>java.lang.Number</TD>
     *     <TD>
     *       Both Channels can accept data from any subclass of Number. It is
     *       possible to send Floats down one channel and Integers down the
     *       other. However all values will be converted to ints.
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
     * The following example shows how to use the Plus process in a small program.
     * The program also uses some of the other building block processes. The
     * program generates a sequence of numbers and doubles them and prints
     * this on the screen.
     *
     * <PRE>
     * import jcsp.lang.*;
     * import jcsp.util.*;
     * <I></I>
     * public class PlusExample {
     * <I></I>
     *   public static void main (String[] argv) {
     * <I></I>
     *     One2OneChannel a = Channel.createOne2One ();
     *     One2OneChannel b = Channel.createOne2One ();
     *     One2OneChannel c = Channel.createOne2One ();
     * <I></I>
     *     new Parallel (
     *       new CSProcess[] {
     *         new Numbers (a.out ()),
     *         new Numbers (b.out ()),
     *         new Plus (a.in (), b.in (), c.out ()),
     *         new Printer (c.in ())
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
    public sealed class Plus : IamCSProcess
    {
    /** The first input Channel */
    private ChannelInput In1;

    /** The second input Channel */
    private ChannelInput In2;

    /** The output Channel */
    private ChannelOutput Out;

    /**
     * Construct a new Plus process with the input Channels in1 and in2 and the
     * output Channel out. The ordering of the Channels in1 and in2 make
     * no difference to the functionality of this process.
     *
     * @param in1 The first input Channel
     * @param in2 The second input Channel
     * @param out The output Channel
     */
    public Plus(ChannelInput In1, ChannelInput In2, ChannelOutput Out)
    {
        this.In1 = In1;
        this.In2 = In2;
        this.Out = Out;
    }

    /**
     * The main body of this process.
     */
    public void run()
    {
        ProcessRead[] parRead = {new ProcessRead(in1), new ProcessRead(in2)};
        CSPParallel par = new CSPParallel(parRead);

        while (true)
        {
            par.run();
            int i1 = ((Number) parRead[0].value).intValue();
            int i2 = ((Number) parRead[1].value).intValue();
                Out.write((int)(i1 + i2));
        }
    }
    }
}
