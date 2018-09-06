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
     * Defines an interface for a factory that can create integer carrying channels with
     * user-definable buffering semantics.
     *
     *
     */
    public interface BufferedChannelIntFactory
    {
        /**
         * Creates a new <code>One2One</code> channel with the given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        One2OneChannelInt createOne2One(ChannelDataStoreInt buffer);

        /**
         * Creates a new <code>Any2One</code> channel with the given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        Any2OneChannelInt createAny2One(ChannelDataStoreInt buffer);

        /**
         * Creates a new <code>One2Any</code> channel with the given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        One2AnyChannelInt createOne2Any(ChannelDataStoreInt buffer);

        /**
         * Creates a new <code>Any2Any</code> channel with the given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        Any2AnyChannelInt createAny2Any(ChannelDataStoreInt buffer);
    }
}