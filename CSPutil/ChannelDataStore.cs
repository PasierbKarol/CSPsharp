using System;

namespace CSPutil
{
    public interface ChannelDataStore : ICloneable
    {
        /** Indicates that the <TT>ChannelDataStore</TT> is empty
     * -- it can accept only a <TT>put</TT>.
     */
        final static int EMPTY = 0;

        /**
         * Indicates that the <TT>ChannelDataStore</TT> is neither empty nor full
         * -- it can accept either a <TT>put</TT> or a <TT>get</TT> call.
         */
        final static int NONEMPTYFULL = 1;

        /** Indicates that the <TT>ChannelDataStore</TT> is full
         * -- it can accept only a <TT>get</TT>.
         */
        final static int FULL = 2;

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