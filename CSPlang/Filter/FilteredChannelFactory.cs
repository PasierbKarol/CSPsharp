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

using CSPlang;
using CSPlang.Any2;

namespace CSPutil
{

    /**
     * <p>This class is used for constructing Filtered Channels.
     * The objects returned by instances of this class will implement
     * the appropriate Filtered Channel interfaces even though the return
     * types are not declared as being Filtered Channels. This is so
     * that this class can implement the <code>ChannelFactory</code> and
     * <code>ChannelArrayFactory</code> interfaces. Instances of this class
     * can therefore be used in place of the standard channel factory classes.</p>
     *
     * <p>A set of read and/or write filters can be specified so that all of the channels created by this
     * factory will have the same buffering properties.</p>
     *
     *
     */
    public class FilteredChannelFactory: ChannelFactory,ChannelArrayFactory,BufferedChannelFactory,BufferedChannelArrayFactory
    {
    /**
     * Underlying factory for creating the base channels.
     */
    private StandardChannelFactory factory;

    /**
     * Read filters to install in channels created by this factory.
     */
    private Filter[] readFilters;

    /**
     * Write filters to install in channels created by this factory.
     */
    private Filter[] writeFilters;

    /**
     * All channels constructed with a Factory constructed with this
     * constructor will default to having no pre-installed filters.
     *
     */
    public FilteredChannelFactory()
    {
        factory = new StandardChannelFactory();
    }

    /**
     * <p>All channels constructed with this Factory instance will have the
     * specified <code>Filter</code> objects inserted into them. The same
     * instances of the filters will be inserted into each channel.</p>
     *
     * <p>Either of the parameters may be <code>null</code> if read/write filters are not required.</p>
     *
     * @param readFilters optional read filters to install in new channels.
     * @param writeFilters optional write filters to install in new channels.
     */
    public FilteredChannelFactory(Filter[] readFilters, Filter[] writeFilters) : this()
    {
        
        this.readFilters = readFilters;
        this.writeFilters = writeFilters;
    }

    /**
     * Installs the filters currently set for this factory into the read/write channel ends supplied.
     *
     * @param readFiltered optional control interface for the read end of a filtered channel.
     * @param writeFiltered optional control interface for the write end of a filtered channel.
     */
    private void installFilters(ReadFiltered readFiltered, WriteFiltered writeFiltered)
    {
        if (readFilters != null)
            for (int i = 0; i < readFilters.Length; i++)
                readFiltered.addReadFilter(readFilters[i]);
        if (writeFilters != null)
            for (int i = 0; i < writeFilters.Length; i++)
                writeFiltered.addWriteFilter(writeFilters[i]);
    }

    /**
     * Creates a new One2One channel with the filtering options set for this factory.
     *
     * @return the created channel with the filters installed.
     */
    public One2OneChannel createOne2One()
    {
        FilteredOne2OneChannelImpl toReturn = new FilteredOne2OneChannelImpl(factory.createOne2One());
        installFilters(toReturn.inFilter(), toReturn.outFilter());
        return toReturn;
    }

    /**
     * Creates a new Any2One channel with the filtering options set for this factory.
     *
     * @return the created channel with the filters installed.
     */
    public Any2OneChannel createAny2One()
    {
        FilteredAny2OneChannelImpl toReturn = new FilteredAny2OneChannelImpl(factory.createAny2One());
        installFilters(toReturn.inFilter(), toReturn.outFilter());
        return toReturn;
    }

    /**
     * Creates a new One2Any channel with the filtering options set for this factory.
     *
     * @return the created channel with the filters installed.
     */
    public One2AnyChannel createOne2Any()
    {
        FilteredOne2AnyChannelImpl toReturn = new FilteredOne2AnyChannelImpl(factory.createOne2Any());
        installFilters(toReturn.inFilter(), toReturn.outFilter());
        return toReturn;
    }

    /**
     * Creates a new Any2Any channel with the filtering options set for this factory.
     *
     * @return the created channel with the filters installed.
     */
    public Any2AnyChannel createAny2Any()
    {
        FilteredAny2AnyChannelImpl toReturn = new FilteredAny2AnyChannelImpl(factory.createAny2Any());
        installFilters(toReturn.inFilter(), toReturn.outFilter());
        return toReturn;
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
        {
            toReturn[i] = createOne2One();
        }

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
     * Creates a new One2One channel with the filtering options set for this factory and the specified
     * data buffer.
     *
     * @param buffer the buffer implementation to use.
     * @return the created filtered channel.
     */
    public One2OneChannel createOne2One(ChannelDataStore buffer)
    {
        FilteredOne2OneChannelImpl toReturn =
            new FilteredOne2OneChannelImpl(factory.createOne2One(buffer));
        installFilters(toReturn.inFilter(), toReturn.outFilter());
        return toReturn;
    }

    /**
     * Creates a new Any2One channel with the filtering options set for this factory and the specified
     * data buffer.
     *
     * @param buffer the buffer implementation to use.
     * @return the created filtered channel.
     */
    public Any2OneChannel createAny2One(ChannelDataStore buffer)
    {
        FilteredAny2OneChannelImpl toReturn =
            new FilteredAny2OneChannelImpl(factory.createAny2One(buffer));
        installFilters(toReturn.inFilter(), toReturn.outFilter());
        return toReturn;
    }

    /**
     * Creates a new One2Any channel with the filtering options set for this factory and the specified
     * data buffer.
     *
     * @param buffer the buffer implementation to use.
     * @return the created filtered channel.
     */
    public One2AnyChannel createOne2Any(ChannelDataStore buffer)
    {
        FilteredOne2AnyChannelImpl toReturn =
            new FilteredOne2AnyChannelImpl(factory.createOne2Any(buffer));
        installFilters(toReturn.inFilter(), toReturn.outFilter());
        return toReturn;
    }

    /**
     * Creates a new Any2Any channel with the filtering options set for this factory and the specified
     * data buffer.
     *
     * @param buffer the buffer implementation to use.
     * @return the created filtered channel.
     */
    public Any2AnyChannel createAny2Any(ChannelDataStore buffer)
    {
        FilteredAny2AnyChannelImpl toReturn =
            new FilteredAny2AnyChannelImpl(factory.createAny2Any(buffer));
        installFilters(toReturn.inFilter(), toReturn.outFilter());
        return toReturn;
    }

    /**
     * Constructs and returns an array of <code>One2OneChannel</code>
     * objects with a given buffering behaviour.
     *
     * @param	n	the size of the array of channels.
     * @param buffer the buffer implementation to use.
     * @return the array of channels.
     *
     * @see jcsp.lang.ChannelArrayFactory#createOne2One(int)
     */
    public One2OneChannel[] createOne2One(ChannelDataStore buffer, int n)
    {
        One2OneChannel[] toReturn = new One2OneChannel[n];
        for (int i = 0; i < n; i++)
            toReturn[i] = createOne2One(buffer);
        return toReturn;
    }

    /**
     * Constructs and returns an array of <code>Any2OneChannel</code>
     * objects with a given buffering behaviour.
     *
     * @param	n	the size of the array of channels.
     * @param buffer the buffer implementation to use.
     * @return the array of channels.
     *
     * @see jcsp.lang.ChannelArrayFactory#createAny2One(int)
     */
    public Any2OneChannel[] createAny2One(ChannelDataStore buffer, int n)
    {
        Any2OneChannel[] toReturn = new Any2OneChannel[n];
        for (int i = 0; i < n; i++)
            toReturn[i] = createAny2One(buffer);
        return toReturn;
    }

    /**
     * Constructs and returns an array of <code>One2AnyChannel</code>
     * objects with a given buffering behaviour.
     *
     * @param	n	the size of the array of channels.
     * @param buffer the buffer implementation to use.
     * @return the array of channels.
     *
     * @see jcsp.lang.ChannelArrayFactory#createOne2Any(int)
     */
    public One2AnyChannel[] createOne2Any(ChannelDataStore buffer, int n)
    {
        One2AnyChannel[] toReturn = new One2AnyChannel[n];
        for (int i = 0; i < n; i++)
            toReturn[i] = createOne2Any(buffer);
        return toReturn;
    }

    /**
     * Constructs and returns an array of <code>Any2AnyChannel</code>
     * objects with a given buffering behaviour.
     *
     * @param	n	the size of the array of channels.
     * @param buffer the buffer implementation to use.
     * @return the array of channels.
     *
     * @see jcsp.lang.ChannelArrayFactory#createAny2Any(int)
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