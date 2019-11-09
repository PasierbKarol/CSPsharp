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

using CSPlang.Shared;
using CSPutil;

namespace CSPlang.Any2
{
    /**
     * This class is an implementation of <code>Any2AnyConnection</code>.
     * Each end is safe to be used by one thread at a time.
     */
    public class Any2AnyConnectionImpl : AbstractConnectionImpl, Any2AnyConnection
    {
        private One2OneChannel chanToServer;
        private One2OneChannel chanFromServer;
        private Any2OneChannel chanClientSynch;
        private Any2OneChannel chanServerSynch;

        /**
         * Initializes all the attributes to necessary values.
         * Channels are created using the static factory in the
         * <code>ChannelServer</code> inteface.
         *
         * Constructor for One2OneConnectionImpl.
         */
        public Any2AnyConnectionImpl() : base()
        {
            chanToServer = StandardChannelFactory.getDefaultInstance().createOne2One(new CSPBuffer(1));
            chanFromServer = StandardChannelFactory.getDefaultInstance().createOne2One(new CSPBuffer(1));
            chanClientSynch = StandardChannelFactory.getDefaultInstance().createAny2One(new CSPBuffer(1));
            chanServerSynch = StandardChannelFactory.getDefaultInstance().createAny2One(new CSPBuffer(1));
        }

        /**
         * Returns a <code>SharedAltingConnectionClient</code> object for this
         * connection. This method can be called multiple times to return a new
         * <code>SharedAltingConnectionClient</code> object each time. Any object
         * created can only be used by one process at a time but the set of
         * objects constructed can be used concurrently.
         *
         * @return a new <code>SharedAltingConnectionClient</code> object.
         */
        public SharedAltingConnectionClient client()
        {
            return new SharedAltingConnectionClient(
                    chanFromServer.In(),
                    chanClientSynch.In(),
                    chanToServer.Out(),
                    chanToServer.Out(),
                    chanClientSynch.Out(),
                    chanFromServer.Out(),
                    this);
        }

        /**
         * Returns a <code>SharedConnectionServer</code> object for this
         * connection. This method can be called multiple times to return a new
         * <code>SharedConnectionServer</code> object each time. Any object
         * created can only be used by one process at a time but the set of
         * objects constructed can be used concurrently.
         *
         * @return a new <code>SharedConnectionServer</code> object.
         */
        public SharedConnectionServer server()
        {
            return new SharedConnectionServerImpl(
                    chanToServer.In(),
                    chanToServer.In(),
                    chanServerSynch.In(),
                    chanServerSynch.Out(),
                    this);
        }
    }
}