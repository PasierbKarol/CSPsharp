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

namespace CSPlang
{

    /**
     * This defines the interface for accepting CALL channels.
     * <H2>Description</H2>
     * <TT>ChannelAccept</TT> defines the interface for accepting CALL channels.
     * The interface contains only one method - {@link #accept <TT>accept</TT>}.
     *
     * <H2>Example</H2>
     * See the explanations and examples documented in the CALL channel super-classes
     * (listed below).
     *
     * @see jcsp.lang.One2OneCallChannel
     * @see jcsp.lang.Any2OneCallChannel
     * @see jcsp.lang.One2AnyCallChannel
     * @see jcsp.lang.Any2AnyCallChannel
     *
     * @author P.H.Welch
     */

    public interface ChannelAccept
    {
        /**
         * This is invoked by a <I>server</I> when it commits to accepting a CALL
         * from a <I>client</I>.  The parameter supplied must be a reference to this <I>server</I>
         * - see the <A HREF="One2OneCallChannel.html#Accept">example</A> from {@link One2OneCallChannel}.
         * It will not complete until a CALL has been made.  If the derived CALL channel has set
         * the <TT>selected</TT> field in the way defined by the standard
         * <A HREF="One2OneCallChannel.html#One2OneFooChannel">calling sequence</A>,
         * the value returned by this method will indicate which method was called.
         *
         * @param server the <I>server</I> process receiving the CALL.
         */
        int accept(IamCSProcess server);
    }
}
