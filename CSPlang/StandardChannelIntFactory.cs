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

using CSPlang.Any2;
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
     * <code>RiskyChannelIntFactory</code> to construct the underlying
     * raw channels.</p>
     */

    public class StandardChannelIntFactory : ChannelIntFactory, ChannelIntArrayFactory, BufferedChannelIntFactory, BufferedChannelIntArrayFactory
    {
        public StandardChannelIntFactory() : base()
        {
        }

        /**
         * Constructs and returns a <code>One2OneChannelInt</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelIntFactory#createOne2One()
         */
        public One2OneChannelInt createOne2One()
        {
            return new One2OneChannelIntImpl();
        }

        /**
         * Constructs and returns an <code>Any2OneChannelInt</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelIntFactory#createAny2One()
         */
        public Any2OneChannelInt createAny2One()
        {
            return new Any2OneChannelIntImpl();
        }

        /**
         * Constructs and returns a <code>One2AnyChannelInt</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelIntFactory#createOne2Any()
         */
        public One2AnyChannelInt createOne2Any()
        {
            return new One2AnyChannelIntImpl();
        }

        /**
         * Constructs and returns an <code>Any2AnyChannelInt</code> object.
         *
         * @return the channel object.
         *
         * @see jcsp.lang.ChannelIntFactory#createAny2Any()
         */
        public Any2AnyChannelInt createAny2Any()
        {
            return new Any2AnyChannelIntImpl();
        }

        /**
         * Constructs and returns an array of <code>One2OneChannelInt</code>
         * objects.
         *
         * @param	n	the size of the array of channels.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelIntArrayFactory#createOne2One(int)
         */
        public One2OneChannelInt[] createOne2One(int n)
        {
            One2OneChannelInt[] toReturn = new One2OneChannelInt[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createOne2One();
            return toReturn;
        }

        /**
         * Constructs and returns an array of <code>Any2OneChannelInt</code>
         * objects.
         *
         * @param	n	the size of the array of channels.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelIntArrayFactory#createAny2One(int)
         */
        public Any2OneChannelInt[] createAny2One(int n)
        {
            Any2OneChannelInt[] toReturn = new Any2OneChannelInt[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createAny2One();
            return toReturn;
        }

        /**
         * Constructs and returns an array of <code>One2AnyChannelInt</code>
         * objects.
         *
         * @param	n	the size of the array of channels.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelIntArrayFactory#createOne2Any(int)
         */
        public One2AnyChannelInt[] createOne2Any(int n)
        {
            One2AnyChannelInt[] toReturn = new One2AnyChannelInt[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createOne2Any();
            return toReturn;
        }

        /**
         * Constructs and returns an array of <code>Any2AnyChannelInt</code>
         * objects.
         *
         * @param	n	the size of the array of channels.
         * @return the array of channels.
         *
         * @see jcsp.lang.ChannelIntArrayFactory#createAny2Any(int)
         */
        public Any2AnyChannelInt[] createAny2Any(int n)
        {
            Any2AnyChannelInt[] toReturn = new Any2AnyChannelInt[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createAny2Any();
            return toReturn;
        }

        /**
         * <p>Constructs and returns a <code>One2OneChannelInt</code> object which
         * uses the specified <code>ChannelDataStoreInt</code> object as a buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStoreInt</code> to use.
         * @return the buffered channel.
         *
         * @see jcsp.lang.BufferedChannelIntFactory#createOne2One(jcsp.util.ints.ChannelDataStoreInt)
         * @see jcsp.util.ints.ChannelDataStoreInt
         */

        public One2OneChannelInt createOne2One(ChannelDataStoreInt buffer)
        {
            return new BufferedOne2OneChannelIntImpl(buffer);
        }

        /**
         * <p>Constructs and returns a <code>Any2OneChannelInt</code> object which
         * uses the specified <code>ChannelDataStoreInt</code> object as a buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStoreInt</code> to use.
         * @return the buffered channel.
         *
         * @see jcsp.lang.BufferedChannelIntFactory#createAny2One(jcsp.util.ints.ChannelDataStoreInt)
         * @see jcsp.util.ints.ChannelDataStoreInt
         */

        public Any2OneChannelInt createAny2One(ChannelDataStoreInt buffer)
        {
            return new BufferedAny2OneChannelIntImpl(buffer);
        }

        /**
         * <p>Constructs and returns a <code>One2AnyChannelInt</code> object which
         * uses the specified <code>ChannelDataStoreInt</code> object as a buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStoreInt</code> to use.
         * @return the buffered channel.
         *
         * @see jcsp.lang.BufferedChannelIntFactory#createOne2Any(jcsp.util.ints.ChannelDataStoreInt)
         * @see jcsp.util.ints.ChannelDataStoreInt
         */
        public One2AnyChannelInt createOne2Any(ChannelDataStoreInt buffer)
        {
            return new BufferedOne2AnyChannelIntImpl(buffer);
        }

        /**
         * <p>Constructs and returns a <code>Any2AnyChannelInt</code> object which
         * uses the specified <code>ChannelDataStoreInt</code> object as a buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStoreInt</code> to use.
         * @return the buffered channel.
         *
         * @see jcsp.lang.BufferedChannelIntFactory#createAny2Any(jcsp.util.ints.ChannelDataStoreInt)
         * @see jcsp.util.ints.ChannelDataStoreInt
         */

        public Any2AnyChannelInt createAny2Any(ChannelDataStoreInt buffer)
        {
            return new BufferedAny2AnyChannelIntImpl(buffer);
        }

        /**
         * <p>Constructs and returns an array of <code>One2OneChannelInt</code> objects
         * which use the specified <code>ChannelDataStoreInt</code> object as a
         * buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel. This is why an array of buffers is not required.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStoreInt</code> to use.
         * @param	n	    the size of the array of channels.
         * @return the array of buffered channels.
         *
         * @see jcsp.lang.BufferedChannelIntArrayFactory#createOne2One(jcsp.util.ints.ChannelDataStoreInt,int)
         * @see jcsp.util.ints.ChannelDataStoreInt
         */

        public One2OneChannelInt[] createOne2One(ChannelDataStoreInt buffer, int n)
        {
            One2OneChannelInt[] toReturn = new One2OneChannelInt[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createOne2One(buffer);
            return toReturn;
        }

        /**
         * <p>Constructs and returns an array of <code>Any2OneChannelInt</code> objects
         * which use the specified <code>ChannelDataStoreInt</code> object as a
         * buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel. This is why an array of buffers is not required.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStoreInt</code> to use.
         * @param	n	    the size of the array of channels.
         * @return the array of buffered channels.
         *
         * @see jcsp.lang.BufferedChannelIntArrayFactory#createAny2One(jcsp.util.ints.ChannelDataStoreInt,int)
         * @see jcsp.util.ints.ChannelDataStoreInt
         */
        public Any2OneChannelInt[] createAny2One(ChannelDataStoreInt buffer, int n)
        {
            Any2OneChannelInt[] toReturn = new Any2OneChannelInt[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createAny2One(buffer);
            return toReturn;
        }

        /**
         * <p>Constructs and returns an array of <code>One2AnyChannelInt</code> objects
         * which use the specified <code>ChannelDataStoreInt</code> object as a
         * buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel. This is why an array of buffers is not required.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStoreInt</code> to use.
         * @param	n	    the size of the array of channels.
         * @return the array of buffered channels.
         *
         * @see jcsp.lang.BufferedChannelIntArrayFactory#createOne2Any(jcsp.util.ints.ChannelDataStoreInt,int)
         * @see jcsp.util.ints.ChannelDataStoreInt
         */

        public One2AnyChannelInt[] createOne2Any(ChannelDataStoreInt buffer, int n)
        {
            One2AnyChannelInt[] toReturn = new One2AnyChannelInt[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createOne2Any(buffer);
            return toReturn;
        }

        /**
         * <p>Constructs and returns an array of <code>Any2AnyChannelInt</code> objects
         * which use the specified <code>ChannelDataStoreInt</code> object as a
         * buffer.
         * </p>
         * <p>The buffer supplied to this method is cloned before it is inserted into
         * the channel. This is why an array of buffers is not required.
         * </p>
         *
         * @param	buffer	the <code>ChannelDataStoreInt</code> to use.
         * @param	n	    the size of the array of channels.
         * @return the array of buffered channels.
         *
         * @see jcsp.lang.BufferedChannelIntArrayFactory#createAny2Any(jcsp.util.ints.ChannelDataStoreInt,int)
         * @see jcsp.util.ints.ChannelDataStoreInt
         */
        public Any2AnyChannelInt[] createAny2Any(ChannelDataStoreInt buffer, int n)
        {
            Any2AnyChannelInt[] toReturn = new Any2AnyChannelInt[n];
            for (int i = 0; i < n; i++)
                toReturn[i] = createAny2Any(buffer);
            return toReturn;
        }
    }
}