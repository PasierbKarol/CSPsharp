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

namespace CSPutil
{
    /**
 * This is the interface for object channel plug-ins that define their buffering
 * characteristics.
 * <H2>Description</H2>
 * <TT>ChannelDataStore</TT> defines the interface to the logic used by
 * channels defined in the <TT>jcsp.lang</TT> package to manage the data
 * being communicated.
 * <P>
 * This enables that logic to be varied by creating channels specifying
 * a particular implementation of this interface.  This reduces the number of
 * classes that would otherwise need to be defined.  The default channel
 * constructor (with no parameters) uses the <TT>ZeroBuffer</TT> implementation,
 * which gives the standard CSP semantics -- no buffering and full synchronisation
 * between reading and writing processes.
 * See the static
 * {@link jcsp.lang.Channel#createOne2One(jcsp.util.ChannelDataStore) <TT>create</TT>}
 * methods of {@link jcsp.lang.Channel} etc.
 * <P>
 * <I>Note: instances of </I><TT>ChannelDataStore</TT><I> implementations are
 * used by the various channel classes within </I><TT>jcsp.lang</TT><I>
 * in a thread-safe way.  They are not intended for any other purpose.
 * Developers of new </I><TT>ChannelDataStore</TT><I> implementations,
 * therefore, do not need to worry about thread safety (e.g. by making its
 * methods </I><TT>synchronized</TT><I>).  Also, developers can assume that
 * the documented pre-conditions for invoking the </I><TT>get</TT><I>
 * and </I><TT>put</TT><I> methods will be met.</I>
 *
 * @see jcsp.util.ZeroBuffer
 * @see jcsp.util.CSPBuffer
 * @see jcsp.util.OverWriteOldestBuffer
 * @see jcsp.util.OverWritingBuffer
 * @see jcsp.util.OverFlowingBuffer
 * @see jcsp.util.InfiniteBuffer
 * @see jcsp.lang.Channel
 *
 * @author P.D.Austin
 */


    public interface ChannelDataStore : ICloneable
    {
        /** Indicates that the <TT>ChannelDataStore</TT> is empty
     * -- it can accept only a <TT>put</TT>.
     */
        readonly static int EMPTY = 0;

        /**
         * Indicates that the <TT>ChannelDataStore</TT> is neither empty nor full
         * -- it can accept either a <TT>put</TT> or a <TT>get</TT> call.
         */
        readonly static int NONEMPTYFULL = 1;

        /** Indicates that the <TT>ChannelDataStore</TT> is full
         * -- it can accept only a <TT>get</TT>.
         */
        readonly static int FULL = 2;

        /**
         * Returns the current state of the <TT>ChannelDataStore</TT>.
         *
         * @return the current state of the <TT>ChannelDataStore</TT> (<TT>EMPTY</TT>,
         * <TT>NONEMPTYFULL</TT> or <TT>FULL</TT>)
         */
        abstract int getState();

        /**
         * Puts a new <TT>Object</TT> into the <TT>ChannelDataStore</TT>.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>FULL</TT>.
         *
         * @param value the <TT>Object</TT> to put into the <TT>ChannelDataStore</TT>
         */
        abstract void put(Object value);

        /**
         * Returns an <TT>Object</TT> from the <TT>ChannelDataStore</TT>.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         *
         * @return an <TT>Object</TT> from the <TT>ChannelDataStore</TT>
         */
        abstract Object get();

        /**
         * Begins an extended read on the buffer, returning the data for the extended read
         * 
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         * 
         * The exact behaviour of this method depends on your buffer.  When a process performs an
         * extended rendezvous on a buffered channel, it will first call this method, then the
         * {@link endGet} method.  
         * 
         * A FIFO buffer would implement this method as returning the value from the front of the buffer
         * and the next call would remove the value.  An overflowing buffer would do the same.
         * 
         * However, for an overwriting buffer it is more complex.  Refer to the documentation for
         * {@link OverWritingBuffer#startGet} and {@link OverWriteOldestBuffer#startGet}
         * for details  
         * 
         * @return The object to be read from the channel at the beginning of the extended rendezvous 
         */
        abstract Object startGet();

        /**
         * Ends an extended read on the buffer.
         * 
         * The channels guarantee that this method will be called exactly once after each beginExtRead call.
         * During the period between startGet and endGet, it is possible that {@link put} will be called,
         * but not {@link get}. 
         *
         * @see endGet
         */
        abstract void endGet();


        /**
         * Returns a new (and <TT>EMPTY</TT>) <TT>ChannelDataStore</TT> with the same
         * creation parameters as this one.
         * <P>
         * <I>Note: Only the size and structure of the </I><TT>ChannelDataStore</TT><I> should
         * be cloned, not any stored data.</I>
         *
         * @return the cloned instance of this <TT>ChannelDataStore</TT>.
         */
        abstract Object clone();

        /**
         * Deletes all items in the buffer, leaving it empty. 
         *
         */

        abstract void removeAll();
    }
}