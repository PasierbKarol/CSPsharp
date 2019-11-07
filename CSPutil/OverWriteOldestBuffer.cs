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
     * This is used to create a buffered object channel that always accepts input,
     * overwriting its oldest data if full.
     * <H2>Description</H2>
     * <TT>OverWriteOldestBuffer</TT> is an implementation of <TT>ChannelDataStore</TT> that yields
     * a <I>FIFO</I> buffered semantics for a channel.  When empty, the channel blocks readers.
     * When full, a writer will overwrite the <I>oldest</I> item left unread in the channel.
     * See the static
     * {@link jcsp.lang.Channel#createOne2One(jcsp.util.ChannelDataStore) <TT>create</TT>}
     * methods of {@link jcsp.lang.Channel} etc.
     * <P>
     * The <TT>getState</TT> method returns <TT>EMPTY</TT> or <TT>NONEMPTYFULL</TT>, but
     * never <TT>FULL</TT>.
     *
     * @see jcsp.util.ZeroBuffer
     * @see jcsp.util.CSPBuffer
     * @see jcsp.util.OverWritingBuffer
     * @see jcsp.util.OverFlowingBuffer
     * @see jcsp.util.InfiniteBuffer
     * @see jcsp.lang.Channel
     *
     * @author P.D.Austin
     */

    [Serializable]
    public class OverWriteOldestBuffer : ChannelDataStore
        {
        /** The storage for the buffered Objects */
        private readonly Object[] buffer;

        /** The number of Objects stored in the CSPBuffer */
        private int counter = 0;

        /** The index of the oldest element (when counter > 0) */
        private int firstIndex = 0;

        /** The index of the next free element (when counter < buffer.Length) */
        private int lastIndex = 0;

        private Boolean valueWrittenWhileFull = false;

        /**
         * Construct a new <TT>OverWriteOldestBuffer</TT> with the specified size.
         *
         * @param size the number of <TT>Object</TT>s the <TT>OverWriteOldestBuffer</TT> can store.
         * @throws BufferSizeError if <TT>size</TT> is zero or negative.  Note: no action
         * should be taken to <TT>try</TT>/<TT>catch</TT> this exception
         * - application code generating it is in error and needs correcting.
         */
        public OverWriteOldestBuffer(int size)
        {
            if (size <= 0)
                throw new BufferSizeError
                    ("\n*** Attempt to create an overwriting buffered channel with negative or zero capacity");
            buffer = new Object[size];
        }

        /**
         * Returns the oldest <TT>Object</TT> from the <TT>OverWriteOldestBuffer</TT> and removes it.
         * <P>
         * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
         *
         * @return the oldest <TT>Object</TT> from the <TT>OverWriteOldestBuffer</TT>
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
         * Begins an extended rendezvous by the reader.  
         * 
         * The semantics of an extended rendezvous on an overwrite-oldest buffer are slightly
         * complicated, but hopefully intuitive.
         * 
         * When a reader begins an extended rendezvous, the oldest value is returned from the buffer
         * (as it would be for a call to {@link get()}).  While an extended rendezvous is ongoing, the
         * writer may (repeatedly) write to the buffer, without ever blocking.  
         * 
         * When the reader finishes an extended rendezvous, the following options are possible:
         * <ul>
         *   <li> The writer has not written to the channel during the rendezvous.  In this case,
         *   the value that was read at the start of the rendezvous is removed from the buffer. </li>
         *   <li> The writer has written to the channel during the rendezvous, but has not over-written
         *   the value that was read at the start of the rendezvous.  In this case, the value that 
         *   was read at the start of the rendezvous is removed from the buffer. </li>
         *   <li> The writer has written to the channel during the rendezvous, and has over-written
         *   (perhaps repeatedly) the value that was read at the start of the rendezvous.  In this case, 
         *   the value that was read at the start of the rendezvous is no longer in the buffer, and hence
         *   nothing is removed. </li>
         * </ul>
         * 
         * @return The oldest value in the buffer at this time
         */
        public Object startGet()
        {
            valueWrittenWhileFull = false;
            return buffer[firstIndex];
        }

        /**
         * See {@link startGet()} for a description of the semantics of this method.
         * 
         * @see startGet()
         */
        public void endGet()
        {
            if (false == valueWrittenWhileFull)
            {
                //Our data hasn't been over-written so remove it
                buffer[firstIndex] = null;
                firstIndex = (firstIndex + 1) % buffer.Length;
                counter--;
            }
        }

        /**
         * Puts a new <TT>Object</TT> into the <TT>OverWriteOldestBuffer</TT>.
         * <P>
         * If <TT>OverWriteOldestBuffer</TT> is full, the <I>oldest</I> item
         * left unread in the buffer will be overwritten.
         *
         * @param value the <TT>Object</TT> to put into the <TT>OverWriteOldestBuffer</TT>
         */
        public void put(Object value)
        {
            if (counter == buffer.Length)
            {
                firstIndex = (firstIndex + 1) % buffer.Length;
                valueWrittenWhileFull = true;
            }
            else
            {
                counter++;
            }

            buffer[lastIndex] = value;
            lastIndex = (lastIndex + 1) % buffer.Length;
        }

        /**
         * Returns the current state of the <TT>OverWriteOldestBuffer</TT>.
         *
         * @return the current state of the <TT>OverWriteOldestBuffer</TT> (<TT>EMPTY</TT> or
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
         * Returns a new (and <TT>EMPTY</TT>) <TT>OverWriteOldestBuffer</TT> with the same
         * creation parameters as this one.
         * <P>
         * <I>Note: Only the size and structure of the </I><TT>OverWriteOldestBuffer</TT><I> is
         * cloned, not any stored data.</I>
         *
         * @return the cloned instance of this <TT>OverWriteOldestBuffer</TT>.
         */
        public Object Clone()
        {
            return new OverWriteOldestBuffer(buffer.Length);
        }

        public void removeAll()
        {
            counter = 0;
            firstIndex = 0;
            lastIndex = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                //Null the objects so they can be garbage collected
                buffer[i] = null;
            }
        }
    }
}