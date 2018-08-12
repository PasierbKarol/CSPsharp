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
using System.Collections.Generic;
using System.Text;

namespace CSPlang.Alting
{
    /**
 * This extends {@link Guard} and {@link ChannelAccept}
 * to enable a process to choose between many CALL channel (and other) events.
 * <H2>Description</H2>
 * <TT>AltingChannelAccept</TT> extends {@link Guard} and {@link ChannelAccept}
 * to enable a process
 * to choose between many CALL channel (and other) events.  The methods inherited from
 * <TT>Guard</TT> are of no concern to users of this package.
 *
 * <H2>Example</H2>
 * See the explanations and examples documented in {@link One2OneCallChannel} and
 * {@link Any2OneCallChannel}.
 *
 * @see jcsp.lang.Alternative
 * @see jcsp.lang.One2OneCallChannel
 * @see jcsp.lang.Any2OneCallChannel
 *
 * @author P.H.Welch
 */

    public abstract class AltingChannelAccept : Guard, ChannelAccept
    {

    }
}
