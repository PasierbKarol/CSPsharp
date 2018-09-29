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
using CSPlang.Any2;
using CSPlang.Shared;
using CSPutil;

namespace CSPlang
{

    /**
     * This class is an implementation of <code>One2AnyConnection</code>.
     * Each end is safe to be used by one thread at a time.
     *
     *
     */
    class One2AnyConnectionImpl : One2AnyConnection
    {
        private AltingConnectionClient altingClient;
        private One2OneChannel chanToServer;
        private One2OneChannel chanFromServer;
        private Any2OneChannel chanSynch;

        /**
         * Initializes all the attributes to necessary values.
         * Channels are created using the static factory in the
         * <code>ChannelServer</code> interface.
         *
         * Constructor for One2OneConnectionImpl.
         */
        public One2AnyConnectionImpl() : base()
        {

            chanToServer = StandardChannelFactory.getDefaultInstance().createOne2One(new CSPBuffer(1));
            chanFromServer = StandardChannelFactory.getDefaultInstance().createOne2One(new CSPBuffer(1));
            chanSynch = StandardChannelFactory.getDefaultInstance().createAny2One(new CSPBuffer(1));

            //create the client and server objects
            altingClient = new AltingConnectionClientImpl(chanFromServer.In(),
            chanToServer.Out(),
            chanToServer.Out(),
            chanFromServer.Out());
        }

        /**
         * Returns the <code>AltingConnectionClient</code> that can
         * be used by a single process at any instance.
         *
         * Each call to this method will return the same object reference.
         *
         * @return the <code>AltingConnectionClient</code> object.
         */
        public AltingConnectionClient client()
        {
            return altingClient;
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
            return new SharedConnectionServerImpl(chanToServer.In(),
            chanToServer.In(), chanSynch.In(),
            chanSynch.Out(), this);
        }


    }
}