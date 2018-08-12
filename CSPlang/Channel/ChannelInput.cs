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
 * This defines the interface for reading from object channels.
 * <H2>Description</H2>
 * <TT>ChannelInput</TT> defines the interface for reading from object channels.
 * The interface contains only one method - <TT>read()</TT>.  This method
 * will block the calling process until an <TT>Object</TT> has been written
 * to the channel by a process at the other end.  If an <TT>Object</TT> has
 * already been written when this method is called, the method will return
 * without blocking.  Either way, the method returns the <TT>Object</TT>
 * sent down the channel.
 * <P>
 * <TT>ChannelInput</TT> variables are used to hold channels
 * that are going to be used only for <I>input</I> by the declaring process.
 * This is a security matter -- by declaring a <TT>ChannelInput</TT>
 * interface, any attempt to <I>output</I> to the channel will generate
 * a compile-time error.  For example, the following code fragment will
 * not compile:
 *
 * <PRE>
 * void doWrite (ChannelInput c, Object o) {
 *   c.write (o);   // illegal
 * }
 * </PRE>
 *
 * When configuring a <TT>CSProcess</TT> with input channels, they should
 * be declared as <TT>ChannelInput</TT> (or, if we wish to be able to make
 * choices between events, as <TT>AltingChannelInput</TT>)
 * variables.  The actual channel passed,
 * of course, may belong to <I>any</I> channel class that implements
 * <TT>ChannelInput</TT> (or <TT>AltingChannelInput</TT>).
 * <P>
 * The <TT>Object</TT> returned can be cast into the actual
 * class the reader process expects.  If the reader can handle more than one
 * class of <TT>Object</TT> (similar to tagged protocols
 * in <I><B>occam</B></I>), checks should be made before casting.
 * <H2>Examples</H2>
 * <H3>Discard data</H3>
 * <PRE>
 * void doRead (ChannelInput c) {
 *   c.read ();                       // clear the channel
 * }
 * </PRE>
 * <H3>Cast data to expected type</H3>
 * <PRE>
 * void doRead (ChannelInput c) {
 *   Boolean b = (Boolean) c.read();  // will cause a ClassCastException
 *                                    // if read does not return a Boolean
 *   ...  etc.
 * }
 * </PRE>
 * <H3>Cast data after checking type</H3>
 * <PRE>
 * void doRead (ChannelInput c) {
 *   Object o = c.read ();
 *   if (o instanceof Boolean) {
 *     System.out.println ("Boolean: " + (Boolean) o);
 *   }
 *   else if (o instanceof Integer) {
 *     System.out.println ("Integer: " + (Integer) o);
 *   }
 *   else {
 *     System.out.println ("Unexpected Class: " + o);
 *   }
 * }
 * </PRE>
 *
 * @see jcsp.lang.AltingChannelInput
 * @see jcsp.lang.ChannelOutput
 * @author P.D.Austin
 */



    public interface ChannelInput : Poisonable
    {
        /**
     * Read an Object from the channel.
     *
     * @return the object read from the channel
     */
        Object read();

        /**
         * Begins an extended rendezvous read from the channel.
         * 
         * In an extended rendezvous, the communication is not 
         * completed until the reader has completed its extended 
         * action (in JCSP, this means that the writer has to wait
         * until the reader calls the corresponding endExtRead
         * function).  The writer notices no difference in the
         * communication (except that it usually takes longer).
         * 
         * <b>You must call {@link endRead} at some point 
         * after this function</b>, otherwise the writer will never
         * be freed and deadlock will almost certainly ensue.
         * 
         * You may perform any actions you like between calling 
         * beginExtRead and {@link endRead}, including communications
         * on other channels, but you must not attempt to communicate 
         * again on this channel until you have called {@link endExtRead}.
         * 
         * An extended rendezvous may be used after the channel's Guard
         * has been selected by an Alternative (i.e. it can be done
         * in place of calling {@link read}).
         * 
         * @return The object read from the channel 
         */
        Object startRead();

        /**
         * The call that signifies the end of the extended rendezvous,
         * as begun by {@link endRead}.  This function should only
         * ever be called after {@link endRead}, and must be called
         * once (and only once) after every {@link endRead} call.  
         *
         * @see startRead
         */
        void endRead();

    }
}