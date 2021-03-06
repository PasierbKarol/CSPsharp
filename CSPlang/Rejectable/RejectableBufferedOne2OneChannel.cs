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

using CSPutil;

namespace CSPlang
{

    /**
     * <p>This implements a one-to-one object channel with user-definable buffering,
     * for use by a single writer and single reader. Refer to {@link One2OneChannel} for a
     * description of this behaviour.</p>
     *
     * <p>Additionally, this channel supports a <code>reject</code> operation. The reader may call
     * the reject method to force any current writer to abort with a
     * <code>ChannelDataRejectedException</code>. Subsequent read and write attempts will immediately cause a
     * <code>ChannelDataRejectedException</code>.</p>
     *
     * <p>Note that the <code>reject</code> operation cannot be called concurrently to a read.</p>
     *
     *
     * 
     * @deprecated This channel is superceded by the poison mechanisms, please see {@link PoisonException}
     */
    public class RejectableBufferedOne2OneChannel : RejectableChannel
    {
        BufferedOne2OneChannel innerChannel;

        /**
         * Constructs a new channel.
         *
         * @param buffer the buffer implementation to use.
         */
        public RejectableBufferedOne2OneChannel(ChannelDataStore buffer)
        {
            innerChannel = (BufferedOne2OneChannel) Channel.one2one(buffer);
        }

        public RejectableAltingChannelInput inAlt()
        {
            return new RejectableAltingChannelInputImpl(innerChannel, 0);
        }

        public RejectableChannelInput In() {
            return new RejectableChannelInputImpl(innerChannel, 0);
        }

        public RejectableChannelOutput Out() {
            return new RejectableChannelOutputImpl(innerChannel, 0);
        }

    }
}