﻿//////////////////////////////////////////////////////////////////////
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


using CSPlang.Shared;

namespace CSPlang.Any2
{

    /**
 * This defines an interface for an any-to-one object channel,
 * safe for use by many writers and one reader.
 * <H2>Description</H2>
 * <TT>Any2OneChannel</TT> is an implementation of a channel which
 * is safe for use by many writing processes but only one reader.
 * Writing processes compete with each other to use the channel.
 * Only the reader and one writer will
 * actually be using the channel at any one time.  This is taken care of by
 * <TT>Any2OneChannel</TT> -- user processes just read from or write to it.
 * <P>
 * The reading process may {@link Alternative <TT>ALT</TT>} on this channel.
 * The writing process is committed (i.e. it may not back off).
 * <P>
 * The default semantics of the channel is that of CSP -- i.e. it is
 * zero-buffered and fully synchronised.  The reading process must wait
 * for a matching writer and vice-versa.
 * <P>
 * A factory pattern is used to create channel instances. The <tt>create</tt> methods of {@link Channel}
 * allow creation of channels, arrays of channels and channels with varying semantics such as
 * buffering with a user-defined capacity or overwriting with various policies.
 * Standard examples are given in the <TT>jcsp.util</TT> package, but
 * <I>careful users</I> may write their own.
 *
 * <H3><A NAME="Caution">Implementation Note and Caution</H3>
 * <I>Fair</I> servicing of writers to this channel depends on the <I>fair</I>
 * servicing of requests to enter a <TT>synchronized</TT> block (or method) by
 * the underlying Java Virtual Machine (JVM).  Java does not specify how threads
 * waiting to synchronize should be handled.  Currently, Sun's standard JDKs queue
 * these requests - which is <I>fair</I>.  However, there is at least one JVM
 * that puts such competing requests on a stack - which is legal but <I>unfair</I>
 * and can lead to infinite starvation.  This is a problem for <I>any</I> Java system
 * relying on good behaviour from <TT>synchronized</TT>, not just for these
 * <I>any-1</I> channels.
 *
 * @see jcsp.lang.Alternative
 * @see jcsp.lang.One2OneChannel
 * @see jcsp.lang.One2AnyChannel
 * @see jcsp.lang.Any2AnyChannel
 * @see jcsp.util.ChannelDataStore
 *
 * @author P.D.Austin and P.H.Welch
 */



    public interface Any2OneChannel
    {
        /**
     * Returns the input end of the channel.
     */
        AltingChannelInput In();

        /**
         * Returns the output end of the channel.
         */
        SharedChannelOutput Out();
    }
}