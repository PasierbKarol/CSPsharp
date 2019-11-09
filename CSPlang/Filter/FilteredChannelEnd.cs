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
using CSPlang.Shared;

namespace CSPutil
{
    /**
     * Static factory for creating channel end wrappers that support filtering.
     */
    public class FilteredChannelEnd
    {
        private static readonly FilteredChannelEndFactory factory = new FilteredChannelEndFactory();

        /**
         * Private constructor to prevent any instances of this static factory from being created.
         */
        private FilteredChannelEnd()
        {
            // No one's creating one of these
        }

        /**
         * Creates a new filtered input channel end around an existing input channel end. The channel end
         * can be used as a guard in an <code>Alternative</code>.
         *
         * @param in the existing channel end to create a filtered form of.
         * @return the new channel end with filtering ability.
         */
        public static FilteredAltingChannelInput createFiltered(AltingChannelInput In)
        {
            return factory.createFiltered(In);
        }

        /**
         * Creates a new filtered input channel end around an existing input channel end.
         *
         * @param in the existing channel end to create a filtered form of.
         * @return the new channel end with filtering ability.
         */
        public static FilteredChannelInput createFiltered(ChannelInput In)
        {
            return factory.createFiltered(In);
        }

        /**
         * Creates a new filtered input channel end around an existing input channel end that can be
         * shared by multiple processes.
         *
         * @param in the existing channel end to create a filtered form of,
         * @return the new channel end with filtering ability.
         */
        public static FilteredSharedChannelInput createFiltered(SharedChannelInput In)
        {
            return factory.createFiltered(In);
        }

        /**
         * Creates a new filtered output channel end around an existing output channel end.
         *
         * @param out the existing channel end to create a filtered form of.
         */
        public static FilteredChannelOutput createFiltered(ChannelOutput Out)
        {
            return factory.createFiltered(Out);
        }

        /**
         * Creates a new filtered output channel end around an existing output channel end that can be
         * shared by multiple processes.
         *
         * @param out the existing channel end to create a filtered form of.
         * @return the new channel end with filtering ability.
         */
        public static FilteredSharedChannelOutput createFiltered(SharedChannelOutput Out)
        {
            return factory.createFiltered(Out);
        }
    }
}