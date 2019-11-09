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
//  Author Contact: P.H.Welch@ukc.ac.uk                             //
//                                                                  //
//  Author contact: K.Chalmers@napier.ac.uk                         //
//                                                                  //
//                                                                  //
//////////////////////////////////////////////////////////////////////

namespace CSPlang
{
    /**
     * This defines the interface for writing to integer channels.
     * <H2>Description</H2>
     * <TT>ChannelOutputInt</TT> defines the interface for writing to integer channels.
     * The interface contains only one method - <TT>write(int o)</TT>.
     * This method will block the calling process until the <TT>int</TT> has
     * been accepted by the channel.  In the (default) case of a zero-buffered
     * synchronising CSP channel, this happens only when a process at the other
     * end of the channel invokes (or has already invoked) a <TT>read()</TT>.
     * <P>
     * <TT>ChannelOutputInt</TT> variables are used to hold integer channels
     * that are going to be used only for <I>output</I> by the declaring process.
     * This is a security matter -- by declaring a <TT>ChannelOutputInt</TT>
     * interface, any attempt to <I>input</I> from the channel will generate
     * a compile-time error.  For example, the following code fragment will
     * not compile:
     *
     * <PRE>
     * int doRead (ChannelOutputInt c) {
     *   return c.read ();   // illegal
     * }
     * </PRE>
     *
     * When configuring a <TT>CSProcess</TT> with output integer channels, they should
     * be declared as <TT>ChannelOutputInt</TT> variables.  The actual channel passed,
     * of course, may belong to <I>any</I> channel class that implements
     * <TT>ChannelOutputInt</TT>.
     *
     * <H2>Example</H2>
     * <PRE>
     * void doWrite (ChannelOutputInt c, int i) {
     *   c.write (i);
     * }
     * </PRE>
     *
     * @see jcsp.lang.ChannelInputInt
     * @author P.D.Austin
     */

    public interface ChannelOutputInt : Poisonable
    {
        /**
         * @param i the integer to write to the channel
         */
        void write(int i);
    }
}