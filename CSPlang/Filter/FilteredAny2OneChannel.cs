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

namespace CSPutil
{

    /**
     * Interface for an Any2One channel that supports filtering operations at each end.
     *
     * @see jcsp.lang.Any2OneChannel
     * @see jcsp.util.filter.ReadFiltered
     * @see jcsp.util.filter.WriteFiltered
     *
     *
     */
    public interface FilteredAny2OneChannel : Any2OneChannel
    {
    /**
     * Returns an interface for configuring read filters on the channel.
     */
    ReadFiltered inFilter();

    /**
     * Returns an interface for configuring write filters on the channel.
     */
    WriteFiltered outFilter();
    }
}