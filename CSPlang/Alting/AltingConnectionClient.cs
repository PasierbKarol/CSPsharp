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
     * <p>
     * This class is sub-classed by JCSP.NET classes
     * to provide <code>ConnectionClient</code> objects which can
     * have their <code>receive()</code> method alted over.
     * </p>
     * <p>
     * Although JCSP users could sub-class this class, under most
     * circumstances, there is no need. <code>AltingConnectionClient</code>
     * objects can be constructed using one of the Connection factory
     * mechanisms. See <code>{@link Connection}</code> and
     * <code>{@link StandardConnectionFactory}</code>.
     * </p>
     */
    public abstract class AltingConnectionClient : Guard, ConnectionClient
    {
        private AltingChannelInput altingChannel;

        /**
         * <p>
         * Note that this is only intended for use by JCSP, and should
         * not be called by user processes.  Users should use one of the
         * subclasses.
         * </p>
         * @param altingChannel The channel used to implement the Guard
         */
        protected AltingConnectionClient(AltingChannelInput altingChannel)
        {
            this.altingChannel = altingChannel;
        }

        /**
         * <p>
         * Returns the channel used to implement the Guard.
         * </p>
         * <p>
         * Note that this method is only intended for use by
         * JCSP, and should not be called by user processes.
         * </p>
         * <p>
         * Concrete subclasses should override this method to
         * return null, to ensure that the alting channel is
         * kept private.
         * </p>
         * @return The channel passed to the constructor.
         */
        protected AltingChannelInput getAltingChannel()
        {
            return this.altingChannel;
        }

        /**
         * <p>
         * <code>ConnectionServer</code> implementations are likely to be
         * implemented over channels. Multiple channels from the client
         * to server may be used; one could be used for the initial
         * connection while another one could be used for data requests.
         * </p>
         * <p>
         * This method allows sub-classes to specify which channel should
         * be the next one to be alted over.
         * </p>
         *
         * @param	chan	the channel to be ALTed over.
         */
        protected void setAltingChannel(AltingChannelInput chan)
        {
            this.altingChannel = chan;
        }

        /**
         * <p>
         * Returns true if the event is ready.  Otherwise, this enables the guard
         * for selection and returns false.
         * </p>
         * <p>
         * <I>Note: this method should only be called by the Alternative class</I>
         * </p>
         * @param alt the Alternative class that is controlling the selection
         * @return true if and only if the event is ready
         */
        Boolean enable(Alternative alt)
        {
            return altingChannel.enable(alt);
        }

        /**
         * <p>
         * Disables the guard for selection. Returns true if the event was ready.
         * </p>
         * <p>
         * <I>Note: this method should only be called by the Alternative class</I>
         * </p>
         * @return true if and only if the event was ready
         */
        Boolean disable()
        {
            return altingChannel.disable();
        }

        /**
         * <p>
         * Returns whether there is an open() pending on this connection. <p>
         * </p>
         * <p>
         * <i>Note: if there is, it won't go away until you accept it.  But if
         * there isn't, there may be one by the time you check the result of
         * this method.</i>
         * </p>
         * @return true only if open() will complete without blocking.
         */
        public Boolean pending()
        {
            return altingChannel.pending();
        }

        public void request(object data)
        {
            throw new NotImplementedException();
        }

        public object reply()
        {
            throw new NotImplementedException();
        }

        public bool isOpen()
        {
            throw new NotImplementedException();
        }
    }
}
