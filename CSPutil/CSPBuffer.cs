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
     * This is used to create a buffered object channel that never loses data.
     * <H2>Description</H2>
     * <TT>CSPBuffer</TT> is an implementation of <TT>ChannelDataStore</TT> that yields
     * a blocking <I>FIFO</I> buffered semantics for a channel.
     * See the static
     * {@link jcsp.lang.Channel#createOne2One(jcsp.util.ChannelDataStore) <TT>create</TT>}
     * methods of {@link jcsp.lang.One2OneChannel} etc.
     * <P>
     * The <TT>getState</TT> method returns <TT>EMPTY</TT>, <TT>NONEMPTYFULL</TT> or
     * <TT>FULL</TT> according to the state of the buffer.
     *
     * @see jcsp.util.ZeroBuffer
     * @see jcsp.util.OverWriteOldestBuffer
     * @see jcsp.util.OverWritingBuffer
     * @see jcsp.util.OverFlowingBuffer
     * @see jcsp.util.InfiniteBuffer
     * @see jcsp.lang.Channel
     *
     * @author P.D.Austin
     */


    [Serializable]
    public class CSPBuffer : ChannelDataStore
    {


        /** The storage for the buffered Objects */
        private readonly Object[] buffer;

        /** The number of Objects stored in the CSPBuffer */
        private int counter = 0;

        /** The index of the oldest element (when counter > 0) */
        private int firstIndex = 0;

        /** The index of the next free element (when counter < buffer.Length) */
        private int lastIndex = 0;

        /**
         * Construct a new <TT>CSPBuffer</TT> with the specified size.
         *
         * @param size the number of <TT>Object</TT>s the <TT>CSPBuffer</TT> can store.
         * @throws BufferSizeError if <TT>size</TT> is negative.  Note: no action
         * should be taken to <TT>try</TT>/<TT>catch</TT> this exception
         * - application code generating it is in error and needs correcting.
         */
        public CSPBuffer(int size)
        {
            if (size < 0)
                throw new BufferSizeError("\n*** Attempt to create a buffered channel with negative capacity");
            buffer = new Object[size + 1]; // the extra one is a subtlety needed by
                                           // the current channel algorithms.
        }

        /**
         * Returns the oldest <TT>Object</TT> from the <TT>CSPBuffer</TT> and removes it.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         *
         * @return the oldest <TT>Object</TT> from the <TT>CSPBuffer</TT>
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
         * Puts a new <TT>Object</TT> into the <TT>CSPBuffer</TT>.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>FULL</TT>.
         *
         * @param value the <TT>Object</TT> to put into the <TT>CSPBuffer</TT>
         */
        public void put(Object value)
        {
            buffer[lastIndex] = value;
            lastIndex = (lastIndex + 1) % buffer.Length;
            counter++;
        }

        /**
         * Returns the current state of the <TT>CSPBuffer</TT>.
         *
         * @return the current state of the <TT>CSPBuffer</TT> (<TT>EMPTY</TT>,
         * <TT>NONEMPTYFULL</TT> or <TT>FULL</TT>)
         */
        public int getState()
        {
            if (counter == 0)
            {
                return ChannelDataStoreState.EMPTY;
            }
            if (counter == buffer.Length)
            {
                return ChannelDataStoreState.FULL;
            }

            return ChannelDataStoreState.NONEMPTYFULL;

        }

        /**
         * Returns a new (and <TT>EMPTY</TT>) <TT>CSPBuffer</TT> with the same
         * creation parameters as this one.
         * <P>
         * <I>Note: Only the size and structure of the </I><TT>CSPBuffer</TT><I> is
         * cloned, not any stored data.</I>
         *
         * @return the cloned instance of this <TT>CSPBuffer</TT>
         */
        public object clone()
        {
            return new CSPBuffer(buffer.Length - 1);
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
            return new CSPBuffer(buffer.Length - 1);
        }
    }
}