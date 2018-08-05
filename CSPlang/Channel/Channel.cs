using System;
using System.Collections.Generic;
using System.Text;
using CSPlang.Any2;
using CSPlang.One2;
using CSPlang.Shared;
using CSPutil;

namespace CSPlang
{
    /*Channel will be using other libraries
     * import jcsp.util.ChannelDataStore;
     *import jcsp.util.ints.ChannelDataStoreInt;
     */
    public class Channel
    {
        /**
        * Private constructor to stop users from instantiating this class.
        */
        private Channel()
        {
            //this class should not be instantiated
        }

        /**
         * The factory to be used by this class. The class should implement
         * ChannelFactory, ChannelArrayFactory, BufferedChannelFactory and BufferedChannelArrayFactory.
         */
        private static readonly StandardChannelFactory factory = new StandardChannelFactory();


        /* Methods that are the same as the Factory Methods */

        /**
         * Constructs and returns a <code>One2OneChannel</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelFactory#createOne2One()
         */
        public static One2OneChannel createOne2One()
        {
            return factory.createOne2One();
        }

        /**
         * Constructs and returns an <code>Any2OneChannel</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelFactory#createAny2One()
         */
        public static Any2OneChannel createAny2One()
        {
            return factory.createAny2One();
        }

        /**
         * Constructs and returns a <code>One2AnyChannel</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelFactory#createOne2Any()
         */
        public static One2AnyChannel createOne2Any()
        {
            return factory.createOne2Any();
        }

        /**
         * Constructs and returns an <code>Any2AnyChannel</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelFactory#createAny2Any()
         */
        public static Any2AnyChannel createAny2Any()
        {
            return factory.createAny2Any();
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
        public static One2OneChannel[] createOne2One(int n)
        {
            return factory.createOne2One(n);
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
        public static Any2OneChannel[] createAny2One(int n)
        {
            return factory.createAny2One(n);
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
        public static One2AnyChannel[] createOne2Any(int n)
        {
            return factory.createOne2Any(n);
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
        public static Any2AnyChannel[] createAny2Any(int n)
        {
            return factory.createAny2Any(n);
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
         * @see jcsp.lang.BufferedChannelFactory#createOne2One(ChannelDataStore)
         * @see jcsp.util.ChannelDataStore
         */
        public static One2OneChannel createOne2One(ChannelDataStore buffer)
        {
            return factory.createOne2One(buffer);
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
         * @see jcsp.lang.BufferedChannelFactory#createAny2One(ChannelDataStore)
         * @see jcsp.util.ChannelDataStore
         */
        public static Any2OneChannel createAny2One(ChannelDataStore buffer)
        {
            return factory.createAny2One(buffer);
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
         * @see jcsp.lang.BufferedChannelFactory#createOne2Any(ChannelDataStore)
         * @see jcsp.util.ChannelDataStore
         */
        public static One2AnyChannel createOne2Any(ChannelDataStore buffer)
        {
            return factory.createOne2Any(buffer);
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
         * @see jcsp.lang.BufferedChannelFactory#createAny2Any(ChannelDataStore)
         * @see jcsp.util.ChannelDataStore
         */
        public static Any2AnyChannel createAny2Any(ChannelDataStore buffer)
        {
            return factory.createAny2Any(buffer);
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
         * @see jcsp.lang.BufferedChannelArrayFactory#createOne2One(ChannelDataStore, int)
         * @see jcsp.util.ChannelDataStore
         */
        public static One2OneChannel[] createOne2One(ChannelDataStore buffer, int n)
        {
            return factory.createOne2One(buffer, n);
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
         * @see jcsp.lang.BufferedChannelArrayFactory#createAny2One(ChannelDataStore, int)
         * @see jcsp.util.ChannelDataStore
         */
        public static Any2OneChannel[] createAny2One(ChannelDataStore buffer, int n)
        {
            return factory.createAny2One(buffer, n);
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
         * @see jcsp.lang.BufferedChannelArrayFactory#createOne2Any(ChannelDataStore, int)
         * @see jcsp.util.ChannelDataStore
         */
        public static One2AnyChannel[] createOne2Any(ChannelDataStore buffer, int n)
        {
            return factory.createOne2Any(buffer, n);
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
         * @see jcsp.lang.BufferedChannelArrayFactory#createAny2Any(ChannelDataStore, int)
         * @see jcsp.util.ChannelDataStore
         */
        public static Any2AnyChannel[] createAny2Any(ChannelDataStore buffer, int n)
        {
            return factory.createAny2Any(buffer, n);
        }

        /**
         * <p>Constructs and returns an array of input channel ends, each of which can be shared by multiple
         * concurrent readers. The returned array, <code>r</code>, is constructed such that
         * <code>r[i] = c[i].in ()</code> for <code>0 <= i < c.Length</code>.
         * </p>
         *
         * @param c the array of channel to obtain input ends from.
         * @return the array of channel input ends.
         */
        public static SharedChannelInput[] getInputArray(Any2AnyChannel[] c)
        {
            SharedChannelInput[] _in = new SharedChannelInput[c.Length];
            for (int i = 0; i < c.Length; i++)
                _in[i] = c[i]._in();
            return _in;
        }

        /**
         * Constructs and returns an array of input channel ends, each of which can be used as guards
         * in an <code>Alternative</code>. The returned array, <code>r</code>, is constructed such that
         * <code>r[i] = c[i].in ()</code> for <code>0 <= i < c.Length</code>.
         *
         * @param c the array of channel to obtain input ends from.
         * @return the array of channel input ends.
         */
        public static AltingChannelInput[] getInputArray(Any2OneChannel[] c)
        {
            AltingChannelInput[] _in = new AltingChannelInput[c.Length];
            for (int i = 0; i < c.Length; i++)
                _in[i] = c[i].In();
            return _in;
        }

        /**
         * Constructs and returns an array of input channel ends, each of which can be shared by multiple
         * concurrent readers. The returned array, <code>r</code>, is constructed such that
         * <code>r[i] = c[i].in ()</code> for <code>0 <= i < c.Length</code>.
         *
         * @param c the array of channel to obtain input ends from.
         * @return the array of channel input ends.
         */
        public static SharedChannelInput[] getInputArray(One2AnyChannel[] c)
        {
            SharedChannelInput[] _in = new SharedChannelInput[c.Length];
            for (int i = 0; i < c.Length; i++)
                _in[i] = c[i].In();
            return _in;
        }

        /**
         * Constructs and returns an array of input channel ends, each of which can be used as guards
         * in an <code>Alternative</code>. The returned array, <code>r</code>, is constructed such that
         * <code>r[i] = c[i].in ()</code> for <code>0 <= i < c.Length</code>.
         *
         * @param c the array of channel to obtain input ends from.
         * @return the array of channel input ends.
         */
        public static AltingChannelInput[] getInputArray(One2OneChannel[] c)
        {
            AltingChannelInput[] _in = new AltingChannelInput[c.Length];
            for (int i = 0; i < c.Length; i++)
           _in[i] = c[i].In();
        return _in;
    }

    /**
     * Constructs and returns an array of output channel ends, each of which can be shared by multiple
     * concurrent writers. The returned array, <code>r</code>, is constructed such that
     * <code>r[i] = c[i].out ()</code> for <code>0 <= i < c.Length</code>.
     *
     * @param c the array of channel to obta_in output ends from.
     * @return the array of output _input ends.
     */
    public static SharedChannelOutput[] getOutputArray(Any2AnyChannel[] c)
    {
        SharedChannelOutput[] _in = new SharedChannelOutput[c.Length];
        for (int i = 0; i < c.Length; i++)
            _in[i] = c[i]._out();
        return _in;
    }

/**
 * Constructs and returns an array of output channel ends, each of which can be shared by multiple
 * concurrent writers. The returned array, <code>r</code>, is constructed such that
 * <code>r[i] = c[i].out ()</code> for <code>0 <= i < c.Length</code>.
 *
 * @param c the array of channel to obta_in output ends from.
 * @return the array of output _input ends.
 */
public static SharedChannelOutput[] getOutputArray(Any2OneChannel[] c)
{
    SharedChannelOutput[] _in = new SharedChannelOutput[c.Length];
    for (int i = 0; i < c.Length; i++)
            _in[i] = c[i].Out();
        return _in;
    }

    /**
     * Constructs and returns an array of output channel ends, each of which can only be used by a
     * s_ingle writer. The returned array, <code>r</code>, is constructed such that
     * <code>r[i] = c[i].out ()</code> for <code>0 <= i < c.Length</code>.
     *
     * @param c the array of channel to obta_in output ends from.
     * @return the array of output _input ends.
     */
    public static ChannelOutput[] getOutputArray(One2AnyChannel[] c)
{
    ChannelOutput[] _in = new ChannelOutput[c.Length];
    for (int i = 0; i < c.Length; i++)
            _in[i] = c[i].Out();
        return _in;
    }

    /**
     * Constructs and returns an array of output channel ends, each of which can only be used by a
     * s_ingle writer. The returned array, <code>r</code>, is constructed such that
     * <code>r[i] = c[i].out ()</code> for <code>0 <= i < c.Length</code>.
     *
     * @param c the array of channel to obta_in output ends from.
     * @return the array of output _input ends.
     */
    public static ChannelOutput[] getOutputArray(One2OneChannel[] c)
{
    ChannelOutput[] _in = new ChannelOutput[c.Length];
    for (int i = 0; i < c.Length; i++)
            _in[i] = c[i].Out();
        return _in;
    }
    
    
    //New create methods:
    
    public static One2OneChannel one2one()
{
    return new One2OneChannelImpl();
}

public static One2AnyChannel one2any()
{
    return new One2AnyChannelImpl();
}

public static Any2OneChannel any2one()
{
    return new Any2OneChannelImpl();
}

public static Any2AnyChannel any2any()
{
    return new Any2AnyChannelImpl();
}

public static One2OneChannel one2one(ChannelDataStore buffer)
{
    return new BufferedOne2OneChannel(buffer);
}

public static One2AnyChannel one2any(ChannelDataStore buffer)
{
    return new BufferedOne2AnyChannel(buffer);
}

public static Any2OneChannel any2one(ChannelDataStore buffer)
{
    return new BufferedAny2OneChannel(buffer);
}

public static Any2AnyChannel any2any(ChannelDataStore buffer)
{
    return new BufferedAny2AnyChannel(buffer);
}

public static One2OneChannel one2one(int immunity)
{
    return new PoisonableOne2OneChannelImpl(immunity);
}

public static One2AnyChannel one2any(int immunity)
{
    return new PoisonableOne2AnyChannelImpl(immunity);
}

public static Any2OneChannel any2one(int immunity)
{
    return new PoisonableAny2OneChannelImpl(immunity);
}

public static Any2AnyChannel any2any(int immunity)
{
    return new PoisonableAny2AnyChannelImpl(immunity);
}

public static One2OneChannel one2one(ChannelDataStore buffer, int immunity)
{
    return new PoisonableBufferedOne2OneChannel(buffer, immunity);
}

public static One2AnyChannel one2any(ChannelDataStore buffer, int immunity)
{
    return new PoisonableBufferedOne2AnyChannel(buffer, immunity);
}

public static Any2OneChannel any2one(ChannelDataStore buffer, int immunity)
{
    return new PoisonableBufferedAny2OneChannel(buffer, immunity);
}

public static Any2AnyChannel any2any(ChannelDataStore buffer, int immunity)
{
    return new PoisonableBufferedAny2AnyChannel(buffer, immunity);
}

public static One2OneChannel[] one2oneArray(int size)
{
    One2OneChannel[] r = new One2OneChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2one();
    }
    return r;
}

public static One2AnyChannel[] one2anyArray(int size)
{
    One2AnyChannel[] r = new One2AnyChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2any();
    }
    return r;
}

public static Any2OneChannel[] any2oneArray(int size)
{
    Any2OneChannel[] r = new Any2OneChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2one();
    }
    return r;
}

public static Any2AnyChannel[] any2anyArray(int size)
{
    Any2AnyChannel[] r = new Any2AnyChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2any();
    }
    return r;
}

public static One2OneChannel[] one2oneArray(int size, int immunity)
{
    One2OneChannel[] r = new One2OneChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2one(immunity);
    }
    return r;
}

public static One2AnyChannel[] one2anyArray(int size, int immunity)
{
    One2AnyChannel[] r = new One2AnyChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2any(immunity);
    }
    return r;
}

public static Any2OneChannel[] any2oneArray(int size, int immunity)
{
    Any2OneChannel[] r = new Any2OneChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2one(immunity);
    }
    return r;
}

public static Any2AnyChannel[] any2anyArray(int size, int immunity)
{
    Any2AnyChannel[] r = new Any2AnyChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2any(immunity);
    }
    return r;
}

public static One2OneChannel[] one2oneArray(int size, ChannelDataStore data)
{
    One2OneChannel[] r = new One2OneChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2one(data);
    }
    return r;
}

public static One2AnyChannel[] one2anyArray(int size, ChannelDataStore data)
{
    One2AnyChannel[] r = new One2AnyChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2any(data);
    }
    return r;
}

public static Any2OneChannel[] any2oneArray(int size, ChannelDataStore data)
{
    Any2OneChannel[] r = new Any2OneChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2one(data);
    }
    return r;
}

public static Any2AnyChannel[] any2anyArray(int size, ChannelDataStore data)
{
    Any2AnyChannel[] r = new Any2AnyChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2any(data);
    }
    return r;
}

public static One2OneChannel[] one2oneArray(int size, ChannelDataStore data, int immunity)
{
    One2OneChannel[] r = new One2OneChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2one(data, immunity);
    }
    return r;
}

public static One2AnyChannel[] one2anyArray(int size, ChannelDataStore data, int immunity)
{
    One2AnyChannel[] r = new One2AnyChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2any(data, immunity);
    }
    return r;
}

public static Any2OneChannel[] any2oneArray(int size, ChannelDataStore data, int immunity)
{
    Any2OneChannel[] r = new Any2OneChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2one(data, immunity);
    }
    return r;
}

public static Any2AnyChannel[] any2anyArray(int size, ChannelDataStore data, int immunity)
{
    Any2AnyChannel[] r = new Any2AnyChannel[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2any(data, immunity);
    }
    return r;
}

public static One2OneChannelInt one2oneInt()
{
    return new One2OneChannelIntImpl();
}

public static One2AnyChannelInt one2anyInt()
{
    return new One2AnyChannelIntImpl();
}

public static Any2OneChannelInt any2oneInt()
{
    return new Any2OneChannelIntImpl();
}

public static Any2AnyChannelInt any2anyInt()
{
    return new Any2AnyChannelIntImpl();
}

public static One2OneChannelInt one2oneInt(ChannelDataStoreInt buffer)
{
    return new BufferedOne2OneChannelIntImpl(buffer);
}

public static One2AnyChannelInt one2anyInt(ChannelDataStoreInt buffer)
{
    return new BufferedOne2AnyChannelIntImpl(buffer);
}

public static Any2OneChannelInt any2oneInt(ChannelDataStoreInt buffer)
{
    return new BufferedAny2OneChannelIntImpl(buffer);
}

public static Any2AnyChannelInt any2anyInt(ChannelDataStoreInt buffer)
{
    return new BufferedAny2AnyChannelIntImpl(buffer);
}

public static One2OneChannelInt one2oneInt(int immunity)
{
    return new PoisonableOne2OneChannelIntImpl(immunity);
}

public static One2AnyChannelInt one2anyInt(int immunity)
{
    return new PoisonableOne2AnyChannelIntImpl(immunity);
}

public static Any2OneChannelInt any2oneInt(int immunity)
{
    return new PoisonableAny2OneChannelIntImpl(immunity);
}

public static Any2AnyChannelInt any2anyInt(int immunity)
{
    return new PoisonableAny2AnyChannelIntImpl(immunity);
}

public static One2OneChannelInt one2oneInt(ChannelDataStoreInt buffer, int immunity)
{
    return new PoisonableBufferedOne2OneChannelInt(buffer, immunity);
}

public static One2AnyChannelInt one2anyInt(ChannelDataStoreInt buffer, int immunity)
{
    return new PoisonableBufferedOne2AnyChannelInt(buffer, immunity);
}

public static Any2OneChannelInt any2oneInt(ChannelDataStoreInt buffer, int immunity)
{
    return new PoisonableBufferedAny2OneChannelInt(buffer, immunity);
}

public static Any2AnyChannelInt any2anyInt(ChannelDataStoreInt buffer, int immunity)
{
    return new PoisonableBufferedAny2AnyChannelInt(buffer, immunity);
}

public static One2OneChannelInt[] one2oneIntArray(int size)
{
    One2OneChannelInt[] r = new One2OneChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2oneInt();
    }
    return r;
}

public static One2AnyChannelInt[] one2anyIntArray(int size)
{
    One2AnyChannelInt[] r = new One2AnyChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2anyInt();
    }
    return r;
}

public static Any2OneChannelInt[] any2oneIntArray(int size)
{
    Any2OneChannelInt[] r = new Any2OneChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2oneInt();
    }
    return r;
}

public static Any2AnyChannelInt[] any2anyIntArray(int size)
{
    Any2AnyChannelInt[] r = new Any2AnyChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2anyInt();
    }
    return r;
}

public static One2OneChannelInt[] one2oneIntArray(int size, int immunity)
{
    One2OneChannelInt[] r = new One2OneChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2oneInt(immunity);
    }
    return r;
}

public static One2AnyChannelInt[] one2anyIntArray(int size, int immunity)
{
    One2AnyChannelInt[] r = new One2AnyChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2anyInt(immunity);
    }
    return r;
}

public static Any2OneChannelInt[] any2oneIntArray(int size, int immunity)
{
    Any2OneChannelInt[] r = new Any2OneChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2oneInt(immunity);
    }
    return r;
}

public static Any2AnyChannelInt[] any2anyIntArray(int size, int immunity)
{
    Any2AnyChannelInt[] r = new Any2AnyChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2anyInt(immunity);
    }
    return r;
}

public static One2OneChannelInt[] one2oneIntArray(int size, ChannelDataStoreInt data)
{
    One2OneChannelInt[] r = new One2OneChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2oneInt(data);
    }
    return r;
}

public static One2AnyChannelInt[] one2anyIntArray(int size, ChannelDataStoreInt data)
{
    One2AnyChannelInt[] r = new One2AnyChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2anyInt(data);
    }
    return r;
}

public static Any2OneChannelInt[] any2oneIntArray(int size, ChannelDataStoreInt data)
{
    Any2OneChannelInt[] r = new Any2OneChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2oneInt(data);
    }
    return r;
}

public static Any2AnyChannelInt[] any2anyIntArray(int size, ChannelDataStoreInt data)
{
    Any2AnyChannelInt[] r = new Any2AnyChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2anyInt(data);
    }
    return r;
}

public static One2OneChannelInt[] one2oneIntArray(int size, ChannelDataStoreInt data, int immunity)
{
    One2OneChannelInt[] r = new One2OneChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2oneInt(data, immunity);
    }
    return r;
}

public static One2AnyChannelInt[] one2anyIntArray(int size, ChannelDataStoreInt data, int immunity)
{
    One2AnyChannelInt[] r = new One2AnyChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = one2anyInt(data, immunity);
    }
    return r;
}

public static Any2OneChannelInt[] any2oneIntArray(int size, ChannelDataStoreInt data, int immunity)
{
    Any2OneChannelInt[] r = new Any2OneChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2oneInt(data, immunity);
    }
    return r;
}

public static Any2AnyChannelInt[] any2anyIntArray(int size, ChannelDataStoreInt data, int immunity)
{
    Any2AnyChannelInt[] r = new Any2AnyChannelInt[size];
    for (int i = 0; i < size; i++)
    {
        r[i] = any2anyInt(data, immunity);
    }
    return r;
}
    
}
