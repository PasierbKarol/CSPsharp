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
     * This is used to create a zero-buffered integer channel that never loses data.
     * <H2>Description</H2>
     * <TT>ZeroBufferInt</TT> is an implementation of <TT>ChannelDataStoreInt</TT> that yields
     * the standard <I><B>CSP</B></I> semantics for a channel -- that is zero buffered with
     * direct synchronisation between reader and writer.  Unless specified otherwise,
     * this is the default behaviour for channels.
     * See the static
     * {@link jcsp.lang.ChannelInt#createOne2One(jcsp.util.ints.ChannelDataStoreInt)
     * <TT>create</TT>} methods of {@link jcsp.lang.ChannelInt} etc.
     * <P>
     * The <TT>getState</TT> method will return <TT>FULL</TT> if there is an output
     * waiting on the channel and <TT>EMPTY</TT> if there is not.
     *
     * @see jcsp.util.ints.BufferInt
     * @see jcsp.util.ints.OverWriteOldestBufferInt
     * @see jcsp.util.ints.OverWritingBufferInt
     * @see jcsp.util.ints.OverFlowingBufferInt
     * @see jcsp.util.ints.InfiniteBufferInt
     * @see jcsp.lang.ChannelInt
     *
     * @author P.D.Austin
     */

    [Serializable]
    public class ZeroBufferInt : ChannelDataStoreInt
    {
        private int state = ChannelDataStoreState.EMPTY;
        private int value;

        /**
         * Returns the <TT>int</TT> from the <TT>ZeroBufferInt</TT>.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         *
         * @return the <TT>int</TT> from the <TT>ZeroBufferInt</TT>
         */
        public int get()
        {
            state = ChannelDataStoreState.EMPTY;
            int o = value;
            return o;
        }

        /**
         * Begins an extended rendezvous - simply returns the next integer in the buffer.  
         * This function does not remove the integer.
         * 
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         * 
         * @return The integer in the buffer. 
         */
        public int startGet()
        {
            return value;
        }

        /**
         * Ends the extended rendezvous by clearing the buffer.
         */
        public void endGet()
        {
            state = ChannelDataStoreState.EMPTY;
        }

        /**
         * Puts a new <TT>int</TT> into the <TT>ZeroBufferInt</TT>.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>FULL</TT>.
         *
         * @param value the <TT>int</TT> to put into the <TT>ZeroBufferInt</TT>
         */
        public void put(int value)
        {
            state = ChannelDataStoreState.FULL;
            this.value = value;
        }

        /**
         * Returns the current state of the <TT>ZeroBufferInt</TT>.
         *
         * @return the current state of the <TT>ZeroBufferInt</TT> (<TT>EMPTY</TT>
         * or <TT>FULL</TT>)
         */
        public int getState()
        {
            return state;
        }

        /**
         * Returns a new (and <TT>EMPTY</TT>) <TT>ZeroBufferInt</TT> with the same
         * creation parameters as this one.
         * <P>
         * <I>Note: Only the size and structure of the </I><TT>ZeroBufferInt</TT><I> is
         * cloned, not any stored data.</I>
         *
         * @return the cloned instance of this <TT>ZeroBufferInt</TT>.
         */
        public Object Clone()
        {
            return new ZeroBufferInt();
        }

        public void removeAll()
        {
            state = ChannelDataStoreState.EMPTY;
        }
    }
}