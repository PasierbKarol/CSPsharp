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


/**
 * This implements an any-to-one integer channel with user-definable buffering,
 * safe for use by many writers and one reader.
 * <H2>Description</H2>
 * <TT>BufferedAny2OneChannelIntImpl</TT> implements an any-to-one integer channel with
 * user-definable buffering.  It is safe for use by many writing processes
 * but only one reader.  Writing processes compete with each other to use
 * the channel.  Only the reader and one writer will
 * actually be using the channel at any one time.  This is taken care of by
 * <TT>BufferedAny2OneChannelIntImpl</TT> -- user processes just read from or write to it.
 * <P>
 * The reading process may {@link Alternative <TT>ALT</TT>} on this channel.
 * The writing process is committed (i.e. it may not back off).
 * <P>
 * The constructor requires the user to provide
 * the channel with a <I>plug-in</I> driver conforming to the
 * {@link jcsp.util.ints.ChannelDataStoreInt <TT>ChannelDataStoreInt</TT>}
 * interface.  This allows a variety of different channel semantics to be
 * introduced -- including buffered channels of user-defined capacity
 * (including infinite), overwriting channels (with various overwriting
 * policies) etc..
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
 * @see jcsp.lang.BufferedOne2OneChannelIntImpl
 * @see jcsp.lang.BufferedOne2AnyChannelIntImpl
 * @see jcsp.lang.BufferedAny2AnyChannelIntImpl
 * @see jcsp.util.ints.ChannelDataStoreInt
 *
 * @author P.D.Austin
 * @author P.H.Welch
 */

using CSPlang.Any2;

namespace CSPlang
{
    class BufferedAny2OneChannelIntImpl : Any2OneIntImpl
    {
        public BufferedAny2OneChannelIntImpl(ChannelDataStoreInt data) : base(new BufferedOne2OneChannelIntImpl(data))
        {

        }

    }
}