using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang
{
    public class StandardChannelFactory : ChannelFactory, ChannelArrayFactory, BufferedChannelFactory, BufferedChannelArrayFactory
    {
        private static StandardChannelFactory defaultInstance = new StandardChannelFactory();

        /**
         * Constructs a new factory.
         */
        public StandardChannelFactory()
        {
            base();
        }

        /**
         * Returns a default instance of a channel factory.
         */
        public static StandardChannelFactory getDefaultInstance()
        {
            return defaultInstance;
        }

        /**
         * Constructs and returns a <code>One2OneChannel</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelFactory#createOne2One()
         */
        public One2OneChannel createOne2One()
        {
            return new One2OneChannelImpl();
        }

        /**
         * Constructs and returns an <code>Any2OneChannel</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelFactory#createAny2One()
         */
        public Any2OneChannel createAny2One()
        {
            return new Any2OneChannelImpl();
        }

        /**
         * Constructs and returns a <code>One2AnyChannel</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelFactory#createOne2Any()
         */
        public One2AnyChannel createOne2Any()
        {
            return new One2AnyChannelImpl();
        }

        /**
         * Constructs and returns an <code>Any2AnyChannel</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelFactory#createAny2Any()
         */
        public Any2AnyChannel createAny2Any()
        {
            return new Any2AnyChannelImpl();
        }

        /**
         * Constructs and returns an array of <code>One2OneChannel</code>
         * objects.
         *
         * @param	n	the size of the array of channels.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelArrayFactory#createOne2One(int)
         */
        public One2OneChannel[] createOne2One(int n)
        {
            One2OneChannel[] toReturn = new One2OneChannel[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createOne2One();
            return toReturn;
        }

        /**
         * Constructs and returns an array of <code>Any2OneChannel</code>
         * objects.
         *
         * @param	n	the size of the array of channels.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelArrayFactory#createAny2One(int)
         */
        public Any2OneChannel[] createAny2One(int n)
        {
            Any2OneChannel[] toReturn = new Any2OneChannel[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createAny2One();
            return toReturn;
        }

        /**
         * Constructs and returns an array of <code>One2AnyChannel</code>
         * objects.
         *
         * @param	n	the size of the array of channels.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelArrayFactory#createOne2Any(int)
         */
        public One2AnyChannel[] createOne2Any(int n)
        {
            One2AnyChannel[] toReturn = new One2AnyChannel[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createOne2Any();
            return toReturn;
        }

        /**
         * Constructs and returns an array of <code>Any2AnyChannel</code>
         * objects.
         *
         * @param	n	the size of the array of channels.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelArrayFactory#createAny2Any(int)
         */
        public Any2AnyChannel[] createAny2Any(int n)
        {
            Any2AnyChannel[] toReturn = new Any2AnyChannel[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createAny2Any();
            return toReturn;
        }

        /**
         * <p>Constructs and returns a <code>One2OneChannel</code> object which
         * uses the specified <code>ChannelDataStore</code> object as a buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStore</code> to use.
         * @return the buffered channel.
         *
         * @see jcsp.lang.BufferedChannelFactory#createOne2One(jcsp.util.ChannelDataStore)
         * @see jcsp.util.ChannelDataStore
         */
        public One2OneChannel createOne2One(ChannelDataStore buffer)
        {
            return new BufferedOne2OneChannel(buffer);
        }

        /**
         * <p>Constructs and returns a <code>Any2OneChannel</code> object which
         * uses the specified <code>ChannelDataStore</code> object as a buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStore</code> to use.
         * @return the buffered channel.
         *
         * @see jcsp.lang.BufferedChannelFactory#createAny2One(jcsp.util.ChannelDataStore)
         * @see jcsp.util.ChannelDataStore
         */
        public Any2OneChannel createAny2One(ChannelDataStore buffer)
        {
            return new BufferedAny2OneChannel(buffer);
        }

        /**
         * <p>Constructs and returns a <code>One2AnyChannel</code> object which
         * uses the specified <code>ChannelDataStore</code> object as a buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStore</code> to use.
         * @return the buffered channel.
         *
         * @see jcsp.lang.BufferedChannelFactory#createOne2Any(jcsp.util.ChannelDataStore)
         * @see jcsp.util.ChannelDataStore
         */
        public One2AnyChannel createOne2Any(ChannelDataStore buffer)
        {
            return new BufferedOne2AnyChannel(buffer);
        }

        /**
         * <p>Constructs and returns a <code>Any2AnyChannel</code> object which
         * uses the specified <code>ChannelDataStore</code> object as a buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStore</code> to use.
         * @return the buffered channel.
         *
         * @see jcsp.lang.BufferedChannelFactory#createAny2Any(jcsp.util.ChannelDataStore)
         * @see jcsp.util.ChannelDataStore
         */
        public Any2AnyChannel createAny2Any(ChannelDataStore buffer)
        {
            return new BufferedAny2AnyChannel(buffer);
        }

        /**
         * <p>Constructs and returns an array of <code>One2OneChannel</code> objects
         * which use the specified <code>ChannelDataStore</code> object as a
         * buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel. This is why an array of buffers is not required.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStore</code> to use.
         * @param	n	    the size of the array of channels.
         * @return the array of buffered channels.
         *
         * @see jcsp.lang.BufferedChannelArrayFactory#createOne2One(jcsp.util.ChannelDataStore,int)
         * @see jcsp.util.ChannelDataStore
         */
        public One2OneChannel[] createOne2One(ChannelDataStore buffer, int n)
        {
            One2OneChannel[] toReturn = new One2OneChannel[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createOne2One(buffer);
            return toReturn;
        }

        /**
         * <p>Constructs and returns an array of <code>Any2OneChannel</code> objects
         * which use the specified <code>ChannelDataStore</code> object as a
         * buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel. This is why an array of buffers is not required.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStore</code> to use.
         * @param	n	    the size of the array of channels.
         * @return the array of buffered channels.
         *
         * @see jcsp.lang.BufferedChannelArrayFactory#createAny2One(jcsp.util.ChannelDataStore,int)
         * @see jcsp.util.ChannelDataStore
         */
        public Any2OneChannel[] createAny2One(ChannelDataStore buffer, int n)
        {
            Any2OneChannel[] toReturn = new Any2OneChannel[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createAny2One(buffer);
            return toReturn;
        }

        /**
         * <p>Constructs and returns an array of <code>One2AnyChannel</code> objects
         * which use the specified <code>ChannelDataStore</code> object as a
         * buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel. This is why an array of buffers is not required.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStore</code> to use.
         * @param	n	    the size of the array of channels.
         * @return the array of buffered channels.
         *
         * @see jcsp.lang.BufferedChannelArrayFactory#createOne2Any(jcsp.util.ChannelDataStore,int)
         * @see jcsp.util.ChannelDataStore
         */
        public One2AnyChannel[] createOne2Any(ChannelDataStore buffer, int n)
        {
            One2AnyChannel[] toReturn = new One2AnyChannel[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createOne2Any(buffer);
            return toReturn;
        }

        /**
         * <p>Constructs and returns an array of <code>Any2AnyChannel</code> objects
         * which use the specified <code>ChannelDataStore</code> object as a
         * buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel. This is why an array of buffers is not required.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStore</code> to use.
         * @param	n	    the size of the array of channels.
         * @return the array of buffered channels.
         *
         * @see jcsp.lang.BufferedChannelArrayFactory#createAny2Any(jcsp.util.ChannelDataStore,int)
         * @see jcsp.util.ChannelDataStore
         */
        public Any2AnyChannel[] createAny2Any(ChannelDataStore buffer, int n)
        {
            Any2AnyChannel[] toReturn = new Any2AnyChannel[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createAny2Any(buffer);
            return toReturn;
        }
    }
}
