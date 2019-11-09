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

using CSPlang.Alting;

namespace CSPlang
{
    /**
     * This implements a one-to-one integer channel.
     * <H2>Description</H2>
     * <TT>One2OneChannelIntImpl</TT> implements a one-to-one integer channel.  Multiple
     * readers or multiple writers are not allowed -- these are catered for
     * by {@link Any2OneChannelInt},
     * {@link One2AnyChannelInt} or
     * {@link Any2AnyChannelInt}.
     * <P>
     * The reading process may {@link Alternative <TT>ALT</TT>} on this channel.
     * The writing process is committed (i.e. it may not back off).
     * <P>
     * The default semantics of the channel is that of CSP -- i.e. it is
     * zero-buffered and fully synchronised.  The reading process must wait
     * for a matching writer and vice-versa.
     * <P>
     * A factory pattern is used to create channel instances. The <tt>create</tt> methods of {@link Channel}
     * allow creation of channels, arrays of channels and channels with varying semantics such as
     * buffering with a user-defined capacity or overwriting with various policies.
     * Standard examples are given in the <TT>jcsp.util</TT> package, but
     * <I>careful users</I> may write their own.
     * <P>
     * Other static <TT>create</TT> methods allows the user to create fully
     * initialised arrays of channels, including plug-ins if required.
     *
     * @see jcsp.lang.Alternative
     * @see jcsp.lang.Any2OneChannelIntImpl
     * @see jcsp.lang.One2AnyChannelIntImpl
     * @see jcsp.lang.Any2AnyChannelIntImpl
     * @see jcsp.util.ints.ChannelDataStoreInt
     *
     * @author P.D.Austin
     * @author P.H.Welch
     */
    public interface One2OneChannelInt
    {
        AltingChannelInputInt In();
        ChannelOutputInt Out();
    }
}