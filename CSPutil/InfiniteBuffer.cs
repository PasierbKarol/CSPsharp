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
     * This is used to create a buffered object channel that always accepts and
     * never loses any input.
     * <H2>Description</H2>
     * <TT>InfiniteBuffer</TT> is an implementation of <TT>ChannelDataStore</TT> that yields
     * a <I>FIFO</I> buffered semantics for a channel.  When empty, the channel blocks readers.
     * However, its capacity is <I>infinite</I> (expanding to whatever is needed so far as
     * the underlying memory system will permit).  So, it <I>never</I> gets full and blocks
     * a writer.
     * See the static
     * {@link jcsp.lang.Channel#createOne2One(jcsp.util.ChannelDataStore) <TT>create</TT>}
     * methods of {@link jcsp.lang.Channel} etc.
     * <P>
     * The <TT>getState</TT> method returns <TT>EMPTY</TT> or <TT>NONEMPTYFULL</TT>, but
     * never <TT>FULL</TT>.
     * <P>
     * An initial size for the buffer can be specified during construction.
     *
     * @see jcsp.util.ZeroBuffer
     * @see jcsp.util.CSPBuffer
     * @see jcsp.util.OverWriteOldestBuffer
     * @see jcsp.util.OverWritingBuffer
     * @see jcsp.util.OverFlowingBuffer
     * @see jcsp.lang.Channel
     *
     * @author P.D.Austin
     */

    [Serializable]
    public class InfiniteBuffer : ChannelDataStore
    {
        /** The default size of the buffer */
        private static readonly int DEFAULT_SIZE = 8;

        /** The initial size of the buffer */
        private int initialSize;

        /** The storage for the buffered Objects */
        private Object[] buffer;

        /** The number of Objects stored in the InfiniteBuffer */
        private int counter = 0;

        /** The index of the oldest element (when counter > 0) */
        private int firstIndex = 0;

        /** The index of the next free element (when counter < buffer.Length) */
        private int lastIndex = 0;

        /**
         * Construct a new <TT>InfiniteBuffer</TT> with the default size (of 8).
         */
        public InfiniteBuffer() : this(DEFAULT_SIZE)
        {

        }

        /**
         * Construct a new <TT>InfiniteBuffer</TT> with the specified initial size.
         *
         * @param initialSize the number of <TT>Object</TT>s
         * the <TT>InfiniteBuffer</TT> can initially store.
         * @throws BufferSizeError if <TT>size</TT> is zero or negative.  Note: no action
         * should be taken to <TT>try</TT>/<TT>catch</TT> this exception
         * - application code generating it is in error and needs correcting.
         */
        public InfiniteBuffer(int initialSize)
        {
            if (initialSize <= 0)
                throw new BufferSizeError
                    ("\n*** Attempt to create a buffered channel with an initially negative or zero capacity");
            this.initialSize = initialSize;
            buffer = new Object[initialSize];
        }

        /**
         * Returns the oldest <TT>Object</TT> from the <TT>InfiniteBuffer</TT> and removes it.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         *
         * @return the oldest <TT>Object</TT> from the <TT>InfiniteBuffer</TT>
         */
        public Object get()
        {
            Object value = buffer[firstIndex];
            buffer[firstIndex] = null;
            firstIndex = (firstIndex + 1) % buffer.Length;
            counter--;
            return value;
        }

        /**
         * Returns the oldest object from the buffer but does not remove it.
         * 
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         *
         * @return the oldest <TT>Object</TT> from the <TT>CSPBuffer</TT>
         */
        public Object startGet()
        {
            return buffer[firstIndex];
        }

        /**
         * Removes the oldest object from the buffer.     
         */
        public void endGet()
        {
            buffer[firstIndex] = null;
            firstIndex = (firstIndex + 1) % buffer.Length;
            counter--;
        }

        /**
         * Puts a new <TT>Object</TT> into the <TT>InfiniteBuffer</TT>.
         * <P>
         * <I>Implementation note:</I> if <TT>InfiniteBuffer</TT> is full, a new internal
         * buffer with double the capacity is constructed and the old data copied across.
         *
         * @param value the <TT>Object</TT> to put into the <TT>InfiniteBuffer</TT>
         */
        public void put(Object value)
        {
            if (counter == buffer.Length)
            {
                Object[] temp = buffer;
                buffer = new Object[buffer.Length * 2];
                Array.Copy(temp, firstIndex, buffer, 0, temp.Length - firstIndex);
                Array.Copy(temp, 0, buffer, temp.Length - firstIndex, firstIndex);
                firstIndex = 0;
                lastIndex = temp.Length;
            }

            buffer[lastIndex] = value;
            lastIndex = (lastIndex + 1) % buffer.Length;
            counter++;
        }

        /**
         * Returns the current state of the <TT>InfiniteBuffer</TT>.
         *
         * @return the current state of the <TT>InfiniteBuffer</TT> (<TT>EMPTY</TT> or
         * <TT>NONEMPTYFULL</TT>)
         */
        public int getState()
        {
            if (counter == 0)
                return ChannelDataStoreState.EMPTY;
            else
                return ChannelDataStoreState.NONEMPTYFULL;
        }

        /**
         * Returns a new (and <TT>EMPTY</TT>) <TT>InfiniteBuffer</TT> with the same
         * creation parameters as this one.
         * <P>
         * <I>Note: Only the initial size and structure of the </I><TT>InfiniteBuffer</TT><I>
         * is cloned, not any stored data.</I>
         *
         * @return the cloned instance of this <TT>InfiniteBuffer</TT>.
         */
        public Object clone()
        {
            return new InfiniteBuffer(initialSize);
        }

        public void removeAll()
        {
            counter = 0;
            firstIndex = 0;
            lastIndex = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                //Null the objects so they can be garbage collected:
                buffer[i] = null;
            }
        }

        public object Clone()
        {
            return new InfiniteBuffer(initialSize);
        }
    }
}