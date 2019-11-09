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

namespace CSPlang.Alting
{
    /**
     * This class wraps an ALTable channel so that only the reading part is
     * available to the caller.  Writes are impossible unless you subclass
     * this (and use getChannel()) or keep a reference to the original
     * channel.  <p>
     *
     * Note that usually you do not need the absolute guarantee that this class
     * provides - you can usually just cast the channel to an AltingChannelInput,
     * which prevents you from <I>accidentally</I> writing to the channel.  This
     * class mainly exists for use by some of the jcsp.net classes, where the
     * absolute guarantee that you cannot write to it is important.
     *
     * @see jcsp.lang.AltingChannelInput
     *
     *
     */
    public class AltingChannelInputWrapper : AltingChannelInput
    {
        public AltingChannelInputWrapper(AltingChannelInput channel)
        {
            this.channel = channel;
        }

        /**
		 * This constructor does not wrap a channel.
		 * The underlying channel can be set by calling
		 * <code>setChannel(AltingChannelInput)</code>.
		 *
		 */
        protected AltingChannelInputWrapper()
        {
            this.channel = null;
        }

        /**
		 * The real channel which this object wraps.
		 *
		 * This used to be a final field but this caused problems
		 * when sub-classes wanted to be serializable. Added a
		 * protected mutator.
		 */
        private AltingChannelInput channel;

        /**
		 * Get the real channel.
		 *
		 * @return The real channel.
		 */
        protected AltingChannelInput getChannel()
        {
            return channel;
        }

        /**
		 * Sets the real channel to be used.
		 *
		 * @param chan the real channel to be used.
		 */
        protected void setChannel(AltingChannelInput chan)
        {
            this.channel = chan;
        }

        /**
		 * Read an Object from the channel.
		 *
		 * @return the object read from the channel
		 */
        public override Object read()
        {
            return channel.read();
        }

        /**
		 * Begins an extended rendezvous
		 * 
		 * @see ChannelInput.beginExtRead
		 * @return The object read from the channel
		 */
        public override Object startRead()
        {
            return channel.startRead();
        }

        /**
		 * Ends an extended rendezvous
		 * 
		 * @see ChannelInput.endExtRead
		 */
        public override void endRead()
        {
            channel.endRead();
        }

        /**
		 * Returns whether there is data pending on this channel.
		 * <P>
		 * <I>Note: if there is, it won't go away until you read it.  But if there
		 * isn't, there may be some by the time you check the result of this method.</I>
		 *
		 * @return state of the channel.
		 */
        public override Boolean pending()
        {
            return channel.pending();
        }

        /**
		 * Returns true if the event is ready.  Otherwise, this enables the guard
		 * for selection and returns false.
		 * <P>
		 * <I>Note: this method should only be called by the Alternative class</I>
		 *
		 * @param alt the Alternative class that is controlling the selection
		 * @return true if and only if the event is ready
		 */
        public override Boolean enable(Alternative alt)
        {
            return channel.enable(alt);
        }

        /**
		 * Disables the guard for selection. Returns true if the event was ready.
		 * <P>
		 * <I>Note: this method should only be called by the Alternative class</I>
		 *
		 * @return true if and only if the event was ready
		 */
        public override Boolean disable()
        {
            return channel.disable();
        }

        public override void poison(int strength)
        {
            channel.poison(strength);
        }
    }
}
