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
     * Reads one <TT>Object</TT> from its input channel.
     * <H2>Process Diagram</H2>
     * <p><img src="doc-files\ProcessRead1.gif"></p>
     * <H2>Description</H2>
     * <TT>ProcessRead</TT> is a process that performs a single read
     * from its <TT>in</TT> channel and then terminates.  It stores
     * the read <TT>Object</TT> in the public <TT>value</TT> field of
     * this process (which is safe to examine <I>after</I> the process
     * has terminated and <I>before</I> it is next run).
     * <P>
     * <TT>ProcessRead</TT> declaration, construction and use should normally
     * be localised within a single method -- so we feel no embarassment about
     * its public field.  Its only (envisaged) purpose is as described in
     * the example below.
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
     * </TABLE>
     * <H2>Example</H2>
     * <TT>ProcessRead</TT> is designed to simplify <I>reading in parallel</I>
     * from channels.  Make as many instances as there
     * are channels, binding each instance to a different channel,
     * together with a {@link jcsp.lang.Parallel} object in which to run them:
     * <PRE>
     *   ChannelInput in0, in1;
     *   .
     *   .
     *   .
     *   ProcessRead read0 = new ProcessRead (in0);
     *   ProcessRead read1 = new ProcessRead (in1);
     *   CSProcess parRead01 = new Parallel (new CSProcess[] {in0, in1});
     * </PRE>
     * The above is best done <I>once</I>, before any looping over the
     * parallel read commences.  A parallel read can now be performed
     * at any time (and any number of times) by executing:
     * <PRE>
     *     parRead01.run ();
     * </PRE>
     * This terminates when, and only when, both reads have completed --
     * the events may occur in <I>any</I> order.  The values read may then
     * be found in <TT>read0.value</TT> and <TT>read1.value</TT>, where they
     * may be safely accessed up until the time that <TT>parRead01</TT> is run again.
     *
     * @see jcsp.lang.Parallel
     * @see jcsp.plugNplay.ProcessWrite
     * @see jcsp.plugNplay.ints.ProcessReadInt
     * @see jcsp.plugNplay.ints.ProcessWriteInt
     *
     * @author P.D.Austin
     */

    public class ProcessRead : IamCSProcess
    {
        /** The <TT>Object</TT> read from the channel */
        public Object value;

        /** The channel from which to read */
        private ChannelInput inChannel;

        /**
         * Construct a new <TT>ProcessRead</TT>.
         *
         * @param in the channel from which to read
         */
        public ProcessRead(ChannelInput inChannel)
        {
            this.inChannel = inChannel;
        }

        /**
         * The main body of this process.
         */
        public void run()
        {
            value = inChannel.read();
        }
    }
}