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
using CSPlang.Shared;

namespace CSPutil
{
    /**
     * This wraps up an Any2OneChannel object so that its
     * input and output ends are separate objects. Both ends of the channel
     * have filtering enabled.
     */
    class FilteredAny2OneChannelImpl : FilteredAny2OneChannel
    {
        private FilteredAltingChannelInput _In;
        private FilteredSharedChannelOutput _Out;

        public FilteredAny2OneChannelImpl(Any2OneChannel chan)
        {
            _In = new FilteredAltingChannelInput(chan.In());
            _Out = new FilteredSharedChannelOutputWrapper(chan.Out());
        }

        public AltingChannelInput In()
        {
            return _In;
        }

        public SharedChannelOutput Out()
        {
            return _Out;
        }

        public ReadFiltered inFilter()
        {
            return _In;
        }

        public WriteFiltered outFilter()
        {
            return _Out;
        }
    }
}