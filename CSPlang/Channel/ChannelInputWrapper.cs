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
     * Defines a wrapper to go around a channel input end. This wrapper allows a channel end to be given
     * away without any risk of the user of that end casting it to a channel output because they cannot
     * gain access to the actual channel end.
     */
    public class ChannelInputWrapper : ChannelInput
    {
        private ChannelInput In;

        /**
         * @param in the existing channel end.
         */
        public ChannelInputWrapper(ChannelInput In)
        {
            this.In = In;
        }

        /**
         * @see jcsp.lang.ChannelInput
         * @return the value read.
         */
        public Object read()
        {
            return In.read();
        }

        /**
         * Begins an extended rendezvous
         * 
         * @see ChannelInput.beginExtRead
         * @return The object read from the channel
         */
        public Object startRead()
        {
            return In.startRead();
        }

        /**
         * Ends an extended rendezvous
         * 
         * @see ChannelInput.endExtRead
         */
        public void endRead()
        {
            In.endRead();
        }

        public void poison(int strength)
        {
            In.poison(strength);
        }
    }
}