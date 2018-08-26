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

package jcsp.util.filter;

import jcsp.lang.*;

/**
 * This wraps up an Any2AnyChannel object so that its
 * input and output ends are separate objects. Both ends of the channel
 * have filtering enabled.
 *
 *
 */
class FilteredAny2AnyChannelImpl implements FilteredAny2AnyChannel
{
    /**
     * The input end of the channel.
     */
    private FilteredSharedChannelInput in;

    /**
     * The output end of the channel.
     */
    private FilteredSharedChannelOutput out;

    /**
     * Constructs a new filtered channel object based on an existing channel.
     */
    FilteredAny2AnyChannelImpl(Any2AnyChannel chan)
    {
        in = new FilteredSharedChannelInputWrapper(chan.in());
        out = new FilteredSharedChannelOutputWrapper(chan.out());
    }

    public SharedChannelInput in()
    {
        return in;
    }

    public SharedChannelOutput out()
    {
        return out;
    }

    public ReadFiltered inFilter()
    {
        return in;
    }

    public WriteFiltered outFilter()
    {
        return out;
    }
}
