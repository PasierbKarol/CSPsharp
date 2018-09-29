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

namespace CSPutil
{

    /**
     * Static factory for creating filtered channels.
     *
     *
     */
    public class FilteredChannel
    {
        /**
         * Default factory for creating the channels.
         */
        private static FilteredChannelFactory factory = new FilteredChannelFactory();

        /**
         * Private constructor to prevent any instances of this static factory from being created.
         */
        private FilteredChannel()
        {
            //private constructor to stop construction
        }

        /**
         * Creates a new One2One filtered channel.
         *
         * @return the created channel.
         */
        public static FilteredOne2OneChannel createOne2One()
        {
            return (FilteredOne2OneChannel) factory.createOne2One();
        }

        /**
         * Creates a new Any2One filtered channel.
         *
         * @return the created channel.
         */
        public static FilteredAny2OneChannel createAny2One()
        {
            return (FilteredAny2OneChannel) factory.createAny2One();
        }

        /**
         * Creates a new One2Any filtered channel.
         *
         * @return the created channel.
         */
        public static FilteredOne2AnyChannel createOne2Any()
        {
            return (FilteredOne2AnyChannel) factory.createOne2Any();
        }

        /**
         * Creates a new Any2Any filtered channel.
         *
         * @return the created channel.
         */
        public static FilteredAny2AnyChannel createAny2Any()
        {
            return (FilteredAny2AnyChannel) factory.createAny2Any();
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
        public static FilteredOne2OneChannel[] createOne2One(int n)
        {
            return (FilteredOne2OneChannel[]) factory.createOne2One(n);
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
        public static FilteredAny2OneChannel[] createAny2One(int n)
        {
            return (FilteredAny2OneChannel[]) factory.createAny2One(n);
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
        public static FilteredOne2AnyChannel[] createOne2Any(int n)
        {
            return (FilteredOne2AnyChannel[]) factory.createOne2Any(n);
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
        public static FilteredAny2AnyChannel[] createAny2Any(int n)
        {
            return (FilteredAny2AnyChannel[]) factory.createAny2Any(n);
        }

        /**
         * Creates a new One2One filtered channel with a given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        public static FilteredOne2OneChannel createOne2One(ChannelDataStore buffer)
        {
            return (FilteredOne2OneChannel) factory.createOne2One(buffer);
        }

        /**
         * Creates a new Any2One filtered channel with a given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        public static FilteredAny2OneChannel createAny2One(ChannelDataStore buffer)
        {
            return (FilteredAny2OneChannel) factory.createAny2One(buffer);
        }

        /**
         * Creates a new One2Any filtered channel with a given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        public static FilteredOne2AnyChannel createOne2Any(ChannelDataStore buffer)
        {
            return (FilteredOne2AnyChannel) factory.createOne2Any(buffer);
        }

        /**
         * Creates a new Any2Any filtered channel with a given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        public static FilteredAny2AnyChannel createAny2Any(ChannelDataStore buffer)
        {
            return (FilteredAny2AnyChannel) factory.createAny2Any(buffer);
        }

        /**
         * Constructs and returns an array of <code>One2OneChannel</code>
         * objects using a given buffer.
         *
         * @param	n	the size of the array of channels.
         * @param buffer the buffer implementation to use.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelArrayFactory#createOne2One(int)
         */
        public static FilteredOne2OneChannel[] createOne2One(ChannelDataStore buffer, int n)
        {
            return (FilteredOne2OneChannel[]) factory.createOne2One(buffer, n);
        }

        /**
         * Constructs and returns an array of <code>Any2OneChannel</code>
         * objects with a given buffer.
         *
         * @param	n	the size of the array of channels.
         * @param buffer the buffer implementation to use.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelArrayFactory#createAny2One(int)
         */
        public static FilteredAny2OneChannel[] createAny2One(ChannelDataStore buffer, int n)
        {
            return (FilteredAny2OneChannel[]) factory.createAny2One(buffer, n);
        }

        /**
         * Constructs and returns an array of <code>One2AnyChannel</code>
         * objects with a given buffer.
         *
         * @param	n	the size of the array of channels.
         * @param buffer the buffer implementation to use.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelArrayFactory#createOne2Any(int)
         */
        public static FilteredOne2AnyChannel[] createOne2Any(ChannelDataStore buffer, int n)
        {
            return (FilteredOne2AnyChannel[]) factory.createOne2Any(buffer, n);
        }

        /**
         * Constructs and returns an array of <code>Any2AnyChannel</code>
         * objects with a given buffer.
         *
         * @param	n	the size of the array of channels.
         * @param buffer the buffer implementation to use.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelArrayFactory#createAny2Any(int)
         */
        public static FilteredAny2AnyChannel[] createAny2Any(ChannelDataStore buffer, int n)
        {
            return (FilteredAny2AnyChannel[]) factory.createAny2Any(buffer, n);
        }
    }
}