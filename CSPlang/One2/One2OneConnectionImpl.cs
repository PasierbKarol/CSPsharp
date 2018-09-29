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
using CSPutil;

namespace CSPlang
{

    /**
     * This class is an implementation of <code>One2OneConnection</code>.
     * Each end is safe to be used by one thread at a time.
     *
     *
     */
    class One2OneConnectionImpl : AbstractConnectionImpl, One2OneConnection
    {
    private AltingConnectionClient connectionClient;
    private AltingConnectionServer connectionServer;

    /**
     * Initializes all the attributes to necessary values.
     * Channels are created using the static factory in the
     * <code>ChannelServer</code> inteface.
     *
     * Constructor for One2OneConnectionImpl.
     */
    public One2OneConnectionImpl() : base()
    {
        
        One2OneChannel chanToServer = StandardChannelFactory.getDefaultInstance().createOne2One(new CSPBuffer(1));
        One2OneChannel chanFromServer = StandardChannelFactory.getDefaultInstance().createOne2One(new CSPBuffer(1));

            //create the client and server objects
        connectionClient = new AltingConnectionClientImpl(chanFromServer.In(),
        chanToServer.Out(),
        chanToServer.Out(),
        chanFromServer.Out());
        connectionServer = new AltingConnectionServerImpl(chanToServer.In(),
        chanToServer.In());
    }

    /**
     * Returns the <code>AltingConnectionClient</code> that can
     * be used by a single process at any instance.
     *
     * This method will always return the same
     * <code>AltingConnectionClient</code> object.
     * <code>One2OneConnection</code> is only intendended to have two ends.
     *
     * @return the <code>AltingConnectionClient</code> object.
     */
    public AltingConnectionClient client()
    {
        return connectionClient;
    }

    /**
     * Returns the <code>AltingConnectionServer</code> that can
     * be used by a single process at any instance.
     *
     * This method will always return the same
     * <code>AltingConnectionServer</code> object.
     * <code>One2OneConnection</code> is only intendended to have two ends.
     *
     * @return the <code>AltingConnectionServer</code> object.
     */
    public AltingConnectionServer server()
    {
        return connectionServer;
    }
    }
}