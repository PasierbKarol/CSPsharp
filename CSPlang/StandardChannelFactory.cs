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
using System.Collections.Generic;
using System.Text;
using CSPlang.Any2;
using CSPlang.One2;
using CSPutil;

namespace CSPlang
{

    /**
 * <p>This class acts as a Factory for creating
 * channels. It can create non-buffered and buffered channels
 * and also arrays of non-buffered and buffered channels.</p>
 *
 * <p>The Channel objects created by this Factory are formed of
 * separate objects for the read and write ends. Therefore the
 * <code>ChannelInput</code> object cannot be cast into the
 * <code>ChannelOutput</code> object and vice-versa.</p>
 *
 * <p>The current implementation uses an instance of the
 * <code>RiskyChannelFactory</code> to construct the underlying
 * raw channels.</p>
 *
 *
 */



    public class StandardChannelFactory : ChannelFactory, ChannelArrayFactory, BufferedChannelFactory, BufferedChannelArrayFactory
    {
        private static StandardChannelFactory defaultInstance = new StandardChannelFactory();

        /**
         * Constructs a new factory.
         */
        public StandardChannelFactory() : base()
        {
            
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
