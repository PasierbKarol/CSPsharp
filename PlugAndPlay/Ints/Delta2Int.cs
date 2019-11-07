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
     * This process broadcasts integers arriving on its input channel <I>in parallel</I>
     * to its two output channels.
     *
     * <H2>Process Diagram</H2>
     * <p><IMG SRC="doc-files\Delta2Int1.gif"></p>
     * <H2>Description</H2>
     * <TT>Delta2Int</TT> is a process that broadcasts (<I>in parallel</I>) on its two output channels
     * everything that arrives on its input channel.
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
     *     <TH>out0, out1</TH>
     *     <TD>int</TD>
     *     <TD>
     *       The output Channels will carry a broadcast of whatever
     *       integers are sent down the in Channel.
     *     </TD>
     *   </TR>
     * </TABLE>
     *
     * @author P.D.Austin
     */

    public sealed class Delta2Int : IamCSProcess
    {
        private ChannelInputInt In;
        private ChannelOutputInt Out0;
        private ChannelOutputInt Out1;

        /**
         * Construct a new Delta2Int process with the input Channel in and the output
         * Channels Out0 and Out1. The ordering of the Channels Out0 and Out1 make
         * no difference to the functionality of this process.
         *
         * @param in the input channel
         * @param Out0 an output Channel
         * @param Out1 an output Channel
         */
        public Delta2Int(ChannelInputInt In, ChannelOutputInt out0, ChannelOutputInt out1)
        {
            this.In = In;
            this.Out0 = out0;
            this.Out1 = out1;
        }

        public void run()
        {
            ProcessWriteInt[] parWrite = {new ProcessWriteInt(Out0), new ProcessWriteInt(Out1)};
            CSPParallel par = new CSPParallel(parWrite);

            while (true)
            {
                int value = In.read();
                parWrite[0].value = value;
                parWrite[1].value = value;
                par.run();
            }
        }
    }
}