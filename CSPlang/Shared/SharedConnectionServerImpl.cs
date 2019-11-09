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
using CSPlang.Alting;

namespace CSPlang.Shared
{
    /**
     * This class does not need to be used by standard JCSP users. It is exposed so that the connection
     * mechanism can be extended for custom connections.
     */
    public class SharedConnectionServerImpl : SharedConnectionServer
    {
        private AltingConnectionServerImpl connectionServerToUse;

        private ChannelInput synchIn;
        private ChannelOutput synchOut;
        private ConnectionWithSharedAltingServer parent;

        protected internal SharedConnectionServerImpl(AltingChannelInput openIn,
            AltingChannelInput requestIn,
            ChannelInput synchIn,
            SharedChannelOutput synchOut,
            ConnectionWithSharedAltingServer parent)
        {
            connectionServerToUse = new AltingConnectionServerImpl(openIn, requestIn);
            this.synchOut = synchOut;
            this.synchIn = synchIn;
            this.parent = parent;
        }

        public Object request()
        {
            if (connectionServerToUse.getServerState() == AltingConnectionServerImpl.SERVER_STATE_CLOSED)
                synchOut.write(null);
            return connectionServerToUse.request();
        }

        public void reply(Object data)
        {
            reply(data, false);
        }

        public void reply(Object data, Boolean close)
        {
            connectionServerToUse.reply(data, close);
            if (connectionServerToUse.getServerState() == AltingConnectionServerImpl.SERVER_STATE_CLOSED)
                synchIn.read();
        }

        public void replyAndClose(Object data)
        {
            reply(data, true);
        }

        public SharedConnectionServer duplicate()
        {
            return parent.server();
        }
    }
}