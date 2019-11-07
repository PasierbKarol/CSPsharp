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
     * This is used to create a buffered integer channel that never loses data.
     * <H2>Description</H2>
     * <TT>BufferInt</TT> is an implementation of <TT>ChannelDataStoreInt</TT> that yields
     * a blocking <I>FIFO</I> buffered semantics for a channel.
     * See the static 
     * {@link jcsp.lang.One2OneChannelInt#create(jcsp.util.ints.ChannelDataStoreInt)
     * <TT>create</TT>} methods of {@link jcsp.lang.One2OneChannelInt} etc.
     * <P>
     * The <TT>getState</TT> method returns <TT>EMPTY</TT>, <TT>NONEMPTYFULL</TT> or
     * <TT>FULL</TT> according to the state of the buffer.
     *
     * @see jcsp.util.ints.ZeroBufferInt
     * @see jcsp.util.ints.OverWriteOldestBufferInt
     * @see jcsp.util.ints.OverWritingBufferInt
     * @see jcsp.util.ints.OverFlowingBufferInt
     * @see jcsp.util.ints.InfiniteBufferInt
     * @see jcsp.lang.ChannelInt
     * 
     * @author P.D.Austin
     */

    [Serializable]
    public class BufferInt : ChannelDataStoreInt
    {
        /** The storage for the buffered ints */
        private readonly int[] buffer;

        /** The number of ints stored in the BufferInt */
        private int counter = 0;

        /** The index of the oldest element (when counter > 0) */
        private int firstIndex = 0;

        /** The index of the next free element (when counter < buffer.Length) */
        private int lastIndex = 0;

        /**
         * Construct a new <TT>BufferInt</TT> with the specified size.
         *
         * @param size the number of <TT>int</TT>s the <TT>BufferInt</TT> can store.
         * @throws BufferIntSizeError if <TT>size</TT> is negative.  Note: no action
         * should be taken to <TT>try</TT>/<TT>catch</TT> this exception
         * - application code generating it is in error and needs correcting.
         */
        public BufferInt(int size)
        {
            if (size < 0)
            {
                throw new BufferIntSizeError(
                    "\n*** Attempt to create a buffered channel with negative capacity"
                );
            }

            buffer = new int[size + 1]; // the extra one is a subtlety needed by
            // the current channel algorithms.
        }

        /**
         * Returns the oldest <TT>int</TT> from the <TT>BufferInt</TT> and removes it.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         *
         * @return the oldest <TT>int</TT> from the <TT>BufferInt</TT>
         */
        public int get()
        {
            int value = buffer[firstIndex];
            firstIndex = (firstIndex + 1) % buffer.Length;
            counter--;
            return value;
        }

        /**
         * Returns the oldest integer from the buffer but does not remove it.
         * 
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         *
         * @return the oldest <TT>int</TT> from the <TT>CSPBuffer</TT>
         */
        public int startGet()
        {
            return buffer[firstIndex];
        }

        /**
         * Removes the oldest integer from the buffer.     
         */
        public void endGet()
        {
            firstIndex = (firstIndex + 1) % buffer.Length;
            counter--;
        }

        /**
         * Puts a new <TT>int</TT> into the <TT>BufferInt</TT>.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>FULL</TT>.
         *
         * @param value the <TT>int</TT> to put into the <TT>BufferInt</TT>
         */
        public void put(int value)
        {
            buffer[lastIndex] = value;
            lastIndex = (lastIndex + 1) % buffer.Length;
            counter++;
        }

        /**
         * Returns the current state of the <TT>BufferInt</TT>.
         *
         * @return the current state of the <TT>BufferInt</TT> (<TT>EMPTY</TT>,
         * <TT>NONEMPTYFULL</TT> or <TT>FULL</TT>)
         */
        public int getState()
        {
            if (counter == 0)
            {
                return ChannelDataStoreState.EMPTY;
            }
            else if (counter == buffer.Length)
            {
                return ChannelDataStoreState.FULL;
            }
            else
            {
                return ChannelDataStoreState.NONEMPTYFULL;
            }
        }

        /**
         * Returns a new (and <TT>EMPTY</TT>) <TT>BufferInt</TT> with the same
         * creation parameters as this one.
         * <P>
         * <I>Note: Only the size and structure of the </I><TT>BufferInt</TT><I> is
         * cloned, not any stored data.</I>
         *
         * @return the cloned instance of this <TT>BufferInt</TT>
         */
        public Object Clone()
        {
            return new BufferInt(buffer.Length - 1);
        }

        public void removeAll()
        {
            counter = 0;
            firstIndex = 0;
            lastIndex = 0;
        }
    }
}