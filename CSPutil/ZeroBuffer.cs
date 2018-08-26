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
     * This is used to create a zero-buffered object channel that never loses data.
     * <H2>Description</H2>
     * <TT>ZeroBuffer</TT> is an implementation of <TT>ChannelDataStore</TT> that yields
     * the standard <I><B>CSP</B></I> semantics for a channel -- that is zero buffered with
     * direct synchronisation between reader and writer.  Unless specified otherwise,
     * this is the default behaviour for channels.
     * See the static
     * {@link jcsp.lang.Channel#createOne2One(jcsp.util.ChannelDataStore) <TT>create</TT>}
     * methods of {@link jcsp.lang.Channel} etc.
     * <P>
     * The <TT>getState</TT> method will return <TT>FULL</TT> if there is an output
     * waiting on the channel and <TT>EMPTY</TT> if there is not.
     *
     * @see jcsp.util.CSPBuffer
     * @see jcsp.util.OverWriteOldestBuffer
     * @see jcsp.util.OverWritingBuffer
     * @see jcsp.util.OverFlowingBuffer
     * @see jcsp.util.InfiniteBuffer
     * @see jcsp.lang.One2OneChannelImpl
     * @see jcsp.lang.Any2OneChannelImpl
     * @see jcsp.lang.One2AnyChannelImpl
     * @see jcsp.lang.Any2AnyChannelImpl
     *
     * @author P.D.Austin
     */

    [Serializable]
    public class ZeroBuffer : ChannelDataStore
    {
        /** The current state */
        private int state = EMPTY;

        /** The Object */
        private Object value;

        /**
         * Returns the <TT>Object</TT> from the <TT>ZeroBuffer</TT>.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         *
         * @return the <TT>Object</TT> from the <TT>ZeroBuffer</TT>
         */
        public Object get()
        {
            state = EMPTY;
            Object o = value;
            value = null;
            return o;
        }

        /**
         * Begins an extended rendezvous - simply returns the next object in the buffer.  
         * This function does not remove the object.
         * 
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         * 
         * @return The object in the buffer. 
         */
        public Object startGet()
        {
            return value;
        }

        /**
         * Ends the extended rendezvous by clearing the buffer.
         */
        public void endGet()
        {
            value = null;
            state = EMPTY;
        }

        /**
         * Puts a new <TT>Object</TT> into the <TT>ZeroBuffer</TT>.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>FULL</TT>.
         *
         * @param value the <TT>Object</TT> to put into the <TT>ZeroBuffer</TT>
         */
        public void put(Object value)
        {
            state = FULL;
            this.value = value;
        }

        /**
         * Returns the current state of the <TT>ZeroBuffer</TT>.
         *
         * @return the current state of the <TT>ZeroBuffer</TT> (<TT>EMPTY</TT>
         * or <TT>FULL</TT>)
         */
        public int getState()
        {
            return state;
        }

        /**
         * Returns a new (and <TT>EMPTY</TT>) <TT>ZeroBuffer</TT> with the same
         * creation parameters as this one.
         * <P>
         * <I>Note: Only the size and structure of the </I><TT>ZeroBuffer</TT><I> is
         * cloned, not any stored data.</I>
         *
         * @return the cloned instance of this <TT>ZeroBuffer</TT>.
         */
        public Object clone()
        {
            return new ZeroBuffer();
        }

        public void removeAll()
        {
            state = EMPTY;
            value = null;
        }
    }
}