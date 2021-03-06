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

namespace CSPlang
{

    /**
     * Defines a wrapper to go around a channel output end. This wrapper allows a channel end to be given
     * away without any risk of the user of that end casting it to a channel input because they cannot
     * gain access to the actual channel end.
     *
     *
     */
    public class ChannelOutputWrapper : ChannelOutput
    {
        /**
         * The actual channel end.
         */
        private ChannelOutput Out;

        /**
         * Creates a new wrapper for the given channel end.
         *
         * @param out the existing channel end.
         */
        public ChannelOutputWrapper(ChannelOutput Out)
        {
            this.Out = Out;
        }

        /**
         * Writes a value to the channel.
         *
         * @param o the value to write.
         * @see jcsp.lang.ChannelOutput
         */
        public void write(Object o)
        {
            Out.write(o);
        }

        public void poison(int strength)
        {
            Out.poison(strength);
        }

    }
}