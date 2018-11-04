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

namespace PlugAndPlay
{

    /**
     * <I>Prefixes</I> a user-supplied object to the <TT>Object</TT> stream
     * flowing through.
     * <H2>Process Diagram</H2>
     * <p><img src="doc-files\Prefix1.gif"></p>
     * <H2>Description</H2>
     * The Prefix class is a process which outputs an initial Object and then
     * has an infinite loop that waits a Object of any type to be sent down the
     * in Channel. The process then passes it on to the out Channel.
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
    public sealed class Prefix : IamCSProcess
    {
        /** The input Channel */
        private ChannelInput In;

        /** The output Channel */
        private ChannelOutput Out;

        /** The initial Object to be sent down the Channel. */
        private Object o;

        /**
         * Construct a new Prefix process with the input Channel in and the
         * output Channel out.
         *
         * @param o the initial Object to be sent down the Channel.
         * @param in the input Channel
         * @param out the output Channel
         */
        public Prefix(Object o, ChannelInput In, ChannelOutput Out)
        {
            this.In = In;
            this.Out = Out;
            this.o = o;
        }

        /**
         * The main body of this process.
         */
        public void run()
        {
            Out.write(o);
            //Debug.WriteLine("Prefix  object is " + o.ToString());

            new Identity(In, Out).run();
        }
    }
}