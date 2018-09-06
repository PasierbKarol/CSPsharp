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

namespace CSPlang.Shared
{
    /**
     * <p>
     * Implements a client end of a Connection which can have multiple
     * client processes.
     * </p>
     * <p>
     * This object cannot itself be shared between concurrent processes
     * but duplicate objects can be generated that can be used by
     * multiple concurrent processes. This can be achieved using
     * the <code>{@link #duplicate()}</code> method.
     * </p>
     * <p>
     * The reply from the server can be ALTed over.
     * </p>
     *
     *
     */
    public class SharedAltingConnectionClient : AltingConnectionClientImpl, SharedConnectionClient
    {
        private ChannelInput synchIn;
        private ChannelOutput synchOut;
        private ConnectionWithSharedAltingClient parent;

        protected internal SharedAltingConnectionClient(AltingChannelInput fromServer,
            ChannelInput synchIn,
            ChannelOutput openToServer,
            ChannelOutput reqToServer,
            SharedChannelOutput synchOut,
            ChannelOutput backToClient,
            ConnectionWithSharedAltingClient
                parent) : base(fromServer, openToServer, reqToServer, backToClient)
        {

            this.synchIn = synchIn;
            this.synchOut = synchOut;
            this.parent = parent;
        }

        protected void claim()
        {
            synchOut.write(null);
        }

        protected void release()
        {
            synchIn.read();
        }

        /**
         * <p>
         * Returns a <code>SharedConnectionClient</code> object that is
         * a duplicate of the object on which this method is called.
         * </p>
         * <p>
         * This allows a process using a <code>SharedAltingConnectionClient</code>
         * object to pass references to the connection client to multiple
         * processes.
         * </p>
         * <p>
         * The object returned can be cast into a
         * <code>SharedConnectionClient</code>  object.
         * </p>
         *
         * @return  a duplicate <code>SharedAltingConnectionClient</code> object.
         */
        public SharedConnectionClient duplicate()
        {
            return parent.client();
        }
    }
}