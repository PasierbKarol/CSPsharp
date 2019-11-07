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

namespace PlugAndPlay
{
    /**
     * This is a running-sum integrator of the <TT>Integers</TT> on its input stream
     * to its output stream.
     * <H2>Process Diagram</H2>
     * <H3>External View</H3>
     * <p><img src="doc-files\Integrate1.gif"></p>
     * <H3>Internal View</H3>
     * <p><img src="doc-files\Integrate2.gif"></p>
     * <H2>Description</H2>
     * The Integrate class is a process which outputs running totals of
     * the Numbers sent down the in Channel.
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
     *       The Channel can accept data from any subclass of Number.  All values
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
     *
     * @author P.D.Austin
     */
    public class Integrate : IamCSProcess
    {
        private ChannelInput In;
        private ChannelOutput Out;

        /**
         * Construct a new Integrate process with the input Channel in and the
         * output Channel out.
         *
         * @param in the input Channel
         * @param out the output Channel
         */
        public Integrate(ChannelInput In, ChannelOutput Out)
        {
            this.In = In;
            this.Out = Out;
        }

        public void run()
        {
            /*final*/
            One2OneChannel a = Channel.createOne2One();
            /*final*/
            One2OneChannel b = Channel.createOne2One();
            /*final*/
            One2OneChannel c = Channel.createOne2One();

            new CSPParallel(new IamCSProcess[]
            {
                new Prefix(0, b.In(), c.Out()), 
                new Delta2(a.In(), Out, b.Out()),
                new Plus(In, c.In(), a.Out())
            }).run();
        }
    }
}