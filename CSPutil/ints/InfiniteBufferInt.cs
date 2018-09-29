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
     * This is used to create a buffered integer channel that always accepts and
     * never loses any input.
     * <H2>Description</H2>
     * <TT>InfiniteBufferInt</TT> is an implementation of <TT>ChannelDataStoreInt</TT> that yields
     * a <I>FIFO</I> buffered semantics for a channel.  When empty, the channel blocks readers.
     * However, its capacity is <I>infinite</I> (expanding to whatever is needed so far as
     * the underlying memory system will permit).  So, it <I>never</I> gets full and blocks
     * a writer.
     * See the static
     * {@link jcsp.lang.ChannelInt#createOne2One(jcsp.util.ints.ChannelDataStoreInt)
     * <TT>create</TT>} methods of {@link jcsp.lang.ChannelInt} etc.
     * <P>
     * The <TT>getState</TT> method returns <TT>EMPTY</TT> or <TT>NONEMPTYFULL</TT>, but
     * never <TT>FULL</TT>.
     * <P>
     * An initial size for the buffer can be specified during construction.
     *
     * @see jcsp.util.ints.ZeroBufferInt
     * @see jcsp.util.ints.BufferInt
     * @see jcsp.util.ints.OverWriteOldestBufferInt
     * @see jcsp.util.ints.OverWritingBufferInt
     * @see jcsp.util.ints.OverFlowingBufferInt
     * @see jcsp.lang.ChannelInt
     *
     * @author P.D.Austin
     */
    //}}}

    public class InfiniteBufferInt : ChannelDataStoreInt
    {
    /** The default size of the buffer */
    private static readonly int DEFAULT_SIZE = 8;

    /** The initial size of the buffer */
    private int initialSize;

    /** The storage for the buffered ints */
    private int[] buffer;

    /** The number of ints stored in the InfiniteBufferInt */
    private int counter = 0;

    /** The index of the oldest element (when counter > 0) */
    private int firstIndex = 0;

    /** The index of the next free element (when counter < buffer.Length) */
    private int lastIndex = 0;

    /**
     * Construct a new <TT>InfiniteBufferInt</TT> with the default size (of 8).
     */
    public InfiniteBufferInt() : this(DEFAULT_SIZE)
        {
        
    }

    /**
     * Construct a new <TT>InfiniteBufferInt</TT> with the specified initial size.
     *
     * @param initialSize the number of <TT>int</TT>s
     * the <TT>InfiniteBufferInt</TT> can initially  store.
     * @throws BufferIntSizeError if <TT>initialSize</TT> is zero or negative.  Note: no action
     * should be taken to <TT>try</TT>/<TT>catch</TT> this exception
     * - application code generating it is in error and needs correcting.
     */
    public InfiniteBufferInt(int initialSize)
    {
        if (initialSize <= 0)
            throw new BufferIntSizeError
                ("\n*** Attempt to create a buffered channel with an initially negative or zero capacity");
        this.initialSize = initialSize;
        buffer = new int[initialSize];
    }

    /**
     * Returns the oldest <TT>int</TT> from the <TT>InfiniteBufferInt</TT> and removes it.
     * <P>
     * <I>Pre-condition</I>: <TT>getState</TT> must not currently return <TT>EMPTY</TT>.
     *
     * @return the oldest <TT>int</TT> from the <TT>InfiniteBufferInt</TT>
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
     * Puts a new <TT>int</TT> into the <TT>InfiniteBufferInt</TT>.
     * <P>
     * <I>Implementation note:</I> if <TT>InfiniteBufferInt</TT> is full, a new internal
     * buffer with double the capacity is constructed and the old data copied across.
     *
     * @param value the <TT>int</TT> to put into the <TT>InfiniteBufferInt</TT>
     */
    public void put(int value)
    {
        if (counter == buffer.Length)
        {
            int[] temp = buffer;
            buffer = new int[buffer.Length * 2];
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
     * Returns the current state of the <TT>InfiniteBufferInt</TT>.
     *
     * @return the current state of the <TT>InfiniteBufferInt</TT> (<TT>EMPTY</TT> or
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
     * Returns a new (and <TT>EMPTY</TT>) <TT>InfiniteBufferInt</TT> with the same
     * creation parameters as this one.
     * <P>
     * <I>Note: Only the initial size and structure of the </I><TT>InfiniteBufferInt</TT><I>
     * is cloned, not any stored data.</I>
     *
     * @return the cloned instance of this <TT>InfiniteBufferInt</TT>.
     */
    public Object Clone()
    {
        return new InfiniteBufferInt(initialSize);
    }

    public void removeAll()
    {
        counter = 0;
        firstIndex = 0;
        lastIndex = 0;
    }
    }
}