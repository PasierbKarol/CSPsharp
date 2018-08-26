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
     * <p>Factory for creating filtered channel ends around existing channel ends.</p>
     *
     * <p>An instance of this class can be created and used, or alternatively the static factory
     * <code>FilteredChannelEnd</code> may be more convenient.</p>
     *
     *
     */
    public class FilteredChannelEndFactory
    {
        /**
         * Constructs a new <code>FilteredChannelEndFactory</code>.
         */
        public FilteredChannelEndFactory() : base ()
        {
            
        }

        /**
         * Creates a new filtered channel input end around an existing channel end. The created channel end
         * can be used as a guard in an <code>Alternative</code>.
         *
         * @param in the existing channel end.
         * @return the created channel end.
         */
        public FilteredAltingChannelInput createFiltered(AltingChannelInput In)
        {
            return new FilteredAltingChannelInput(In);
        }

        /**
         * Creates a new filtered channel input end around an existing channel end.
         *
         * @param in the existing channel end.
         * @return the created channel end.
         */
        public FilteredChannelInput createFiltered(ChannelInput In)
        {
            return new FilteredChannelInputWrapper(In);
        }

        /**
         * Creates a new filtered channel input end around an existing channel end. The created channel end
         * can be shared by multiple processes.
         *
         * @param in the existing channel end.
         * @return the created channel end.
         */
        public FilteredSharedChannelInput createFiltered(SharedChannelInput In)
        {
            return new FilteredSharedChannelInputWrapper(In);
        }

        /**
         * Creates a new filtered channel output end around an existing channel end.
         *
         * @param out the existing channel end.
         * @return the created channel end.
         */
        public FilteredChannelOutput createFiltered(ChannelOutput Out)
        {
            return new FilteredChannelOutputWrapper(Out);
        }

        /**
         * Creates a new filtered channel output end around an existing channel end. The created channel end
         * can be shared by multiple processes.
         *
         * @param out the existing channel end.
         * @return the created channel end.
         */
        public FilteredSharedChannelOutput createFiltered(SharedChannelOutput Out)
        {
            return new FilteredSharedChannelOutputWrapper(Out);
        }
    }
}