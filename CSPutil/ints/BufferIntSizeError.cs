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
     * This is thrown if an attempt is made to create some variety of buffered channel
     * with a zero or negative sized buffer.
     *
     * <H2>Description</H2>
     * Buffered channels must have (usually non-zero) positive sized buffers.
     * The following constructions will all throw this {@link java.lang.Error}:
     * <pre>
     *   One2OneChannelInt c = ChannelInt.createOne2One
     *                     (new BufferInt (-42));
     *                     // zero allowed
     *   One2OneChannelInt c = ChannelInt.createOne2One
     *                     (new OverFlowingBufferInt (-42));
     *                     // zero not allowed
     *   One2OneChannelInt c = ChannelInt.createOne2One
     *                     (new OverWriteOldestBufferInt (-42));
     *                     // zero not allowed
     *   One2OneChannelInt c = ChannelInt.createOne2One
     *                     (new OverWritingBufferInt (-42));
     *                     // zero not allowed
     *   One2OneChannelInt c = ChannelInt.createOne2One
     *                     (new InfiniteBufferInt (-42));
     *                     // zero not allowed
     * </pre>
     * Zero-buffered non-overwriting channels are, of course, the default channel semantics.
     * The following constructions are all legal and equivalent:
     * <pre>
     *   One2OneChannelInt c = ChannelInt.createOne2One ();
     *   One2OneChannelInt c = ChannelInt.createOne2One (new ZeroBufferInt ());
     *   One2OneChannelInt c = ChannelInt.createOne2One (new BufferInt (0));
     *   // legal but less efficient
     * </pre>
     * No action should be taken to catch <TT>BufferSizeError</TT>.
     * Application code generating it is in error and needs correcting.
     *
     * @author P.H.Welch
     */

    public class BufferIntSizeError : Exception
    {
        /**
         * Constructs a new <code>BufferIntSizeError</code> with the specified detail message.
         *
         * @param s the detail message.
         */
        public BufferIntSizeError(String s) : base(s)
        {
        
        }
    }
}