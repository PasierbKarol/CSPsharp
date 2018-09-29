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

namespace CSPutil
{

    /**
     * Implements a <code>One2One</code> channel that supports filtering at each end.
     *
     *
     */
    class FilteredOne2OneChannelImpl : FilteredOne2OneChannel
    {
    /**
     * The filtered input end of the channel.
     */
    private FilteredAltingChannelInput _In;

    /**
     * The filtered output end of the channel.
     */
    private FilteredChannelOutput _Out;

    /**
     * Constructs a new filtered channel based on an existing channel.
     *
     * @param chan the existing channel.
     */
    public FilteredOne2OneChannelImpl(One2OneChannel chan)
    {
        _In = new FilteredAltingChannelInput(chan.In());
        _Out = new FilteredChannelOutputWrapper(chan.Out());
    }

    public AltingChannelInput In()
    {
        return _In;
    }

    public ChannelOutput Out()
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