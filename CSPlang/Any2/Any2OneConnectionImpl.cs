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
using CSPlang.Alting;
using CSPlang.Shared;

namespace CSPlang.Any2
{
    /**
 * This class is an implementation of <code>Any2OneConnection</code>.
 * Each end is safe to be used by one thread at a time.
 *
 *
 */

    class Any2OneConnectionImpl : Any2OneConnection
    {
        private AltingConnectionServer connectionServer;
        private One2OneChannel chanToServer;
        private One2OneChannel chanFromServer;
        private Any2OneChannel chanSynch;

        /**
         * Initializes all the attributes to necessary values.
         * Channels are created using the static factory in the
         * <code>ChannelServer</code> inteface.
         *
         * Constructor for One2OneConnectionImpl.
         */
        public Any2OneConnectionImpl() : base()
        {
            chanToServer = ConnectionServer.FACTORY.createOne2One(new Buffer(1));
            chanFromServer = ConnectionServer.FACTORY.createOne2One(new Buffer(1));
            chanSynch = ConnectionServer.FACTORY.createAny2One(new Buffer(1));
            //create the server object - client object created when accessed
            connectionServer = new AltingConnectionServerImpl(chanToServer.In(), chanToServer.In());
        }

        /**
         * Returns the <code>AltingConnectionClient</code> that can
         * be used by a single process at any instance.
         *
         * @return the <code>AltingConnectionClient</code> object.
         */
        public SharedAltingConnectionClient client()
        {
            return new SharedAltingConnectionClient(chanFromServer.In(),
            chanSynch.In(),
            chanToServer.Out(),
            chanToServer.Out(),
            chanSynch.Out(),
            chanFromServer.Out),
            this);
        }

        /**
         * Returns the <code>AltingConnectionServer</code> that can
         * be used by a single process at any instance.
         *
         * @return the <code>AltingConnectionServer</code> object.
         */
        public AltingConnectionServer server()
        {
            return connectionServer;
        }

    }
}
