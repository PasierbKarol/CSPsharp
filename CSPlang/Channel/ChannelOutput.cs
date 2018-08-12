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

using System;

namespace CSPlang
{

    /**
 * This defines the interface for writing to object channels.
 * <H2>Description</H2>
 * <TT>ChannelOutput</TT> defines the interface for writing to object channels.
 * The interface contains only one method - <TT>write(Object o)</TT>.
 * This method will block the calling process until the <TT>Object</TT> has
 * been accepted by the channel.  In the (default) case of a zero-buffered
 * synchronising CSP channel, this happens only when a process at the other
 * end of the channel invokes (or has already invoked) a <TT>read()</TT>.
 * <P>
 * <TT>ChannelOutput</TT> variables are used to hold channels
 * that are going to be used only for <I>output</I> by the declaring process.
 * This is a security matter -- by declaring a <TT>ChannelOutput</TT>
 * interface, any attempt to <I>input</I> from the channel will generate
 * a compile-time error.  For example, the following code fragment will
 * not compile:
 *
 * <PRE>
 * Object doRead (ChannelOutput c) {
 *   return c.read ();   // illegal
 * }
 * </PRE>
 *
 * When configuring a <TT>CSProcess</TT> with output channels, they should
 * be declared as <TT>ChannelOutput</TT> variables.  The actual channel passed,
 * of course, may belong to <I>any</I> channel class that implements
 * <TT>ChannelOutput</TT>.
 * <P>
 * Instances of any class may be written to a channel.
 *
 * <H2>Example</H2>
 * <PRE>
 * void doWrite (ChannelOutput c, Object o) {
 *   c.write (o);
 * }
 * </PRE>
 *
 * @see jcsp.lang.ChannelInput
 * @author P.D.Austin
 */


    public interface ChannelOutput : Poisonable
    {
        /**
 * Write an Object to the channel.
 *
 * @param object the object to write to the channel
 */
        /*public*/ void write(Object object_name);
    }
}