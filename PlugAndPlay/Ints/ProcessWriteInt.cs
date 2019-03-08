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
     * Writes one <TT>int</TT> to its output channel.
     * <H2>Process Diagram</H2>
     * <p><IMG SRC="doc-files\ProcessWriteInt1.gif"></p>
     * <H2>Description</H2>
     * <TT>ProcessWriteInt</TT> is a process that performs a single write
     * to its <TT>out</TT> channel and then terminates.  The <TT>int</TT>
     * that is written must first be placed in the public <TT>value</TT> field
     * of this process (which is safe to set <I>before</I> and <I>in between</I>
     * process runs).
     * <P>
     * <TT>ProcessWriteInt</TT> declaration, construction and use should normally
     * be localised within a single method -- so we feel no embarassment about
     * its public field.  Its only (envisaged) purpose is as described in
     * the example below.
     * <H2>Channel Protocols</H2>
     * <TABLE BORDER="2">
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
     * <H2>Example</H2>
     * <TT>ProcessWriteInt</TT> is designed to simplify <I>writing in parallel</I>
     * to channels.  Make as many instances as there
     * are channels, binding each instance to a different channel,
     * together with a {@link jcsp.lang.Parallel} object in which to run them:
     * <PRE>
     *   ChannelOutputInt out0, out1;
     *   .
     *   .
     *   .
     *   ProcessWriteInt write0 = new ProcessWriteInt (out0);
     *   ProcessWriteInt write1 = new ProcessWriteInt (out1);
     *   CSProcess parWrite01 = new Parallel (new CSProcess[] {out0, out1});
     * </PRE>
     * The above is best done <I>once</I>, before any looping over the
     * parallel read commences.  A parallel write can now be performed
     * at any time (and any number of times) by executing:
     * <PRE>
     *     write0.value = ...;   // whatever we want sent down out0
     *     write1.value = ...;   // whatever we want sent down out1
     *     parWrite01.run ();
     * </PRE>
     * This terminates when, and only when, both writes have completed --
     * the events may occur in <I>any</I> order.
     *
     * @see jcsp.lang.Parallel
     * @see jcsp.plugNplay.ProcessRead
     * @see jcsp.plugNplay.ProcessWrite
     * @see jcsp.plugNplay.ints.ProcessReadInt
     *
     * @author P.D.Austin
     */

    public class ProcessWriteInt : IamCSProcess
    {
        /** The <TT>int</TT> to be written to the channel */
        public int value;

        /** The channel to which to write */
        private ChannelOutputInt Out;

        /**
         * Construct a new <TT>ProcessWriteInt</TT>.
         *
         * @param out the channel to which to write
         */
        public ProcessWriteInt(ChannelOutputInt Out)
        {
            this.Out = Out;
        }

        /**
         * The main body of this process.
         */
        public void run()
        {
            //Console.WriteLine("Before writing value in ProcessWriter");
            Out.write(value);
            //Console.WriteLine("After writing value in ProcessWriter");
        }
    }
}