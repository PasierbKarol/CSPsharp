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

namespace CSPlang.Alting
{
    /**
 * This class does not need to be used by standard JCSP users. It is exposed so that the connection
 * mechanism can be extended for custom connections.
 *
 *
 */

    public class AltingConnectionClientImpl : AltingConnectionClient
    {
        private int currentClientState;

        private static readonly int CLIENT_STATE_CLOSED = 1;
        private static readonly int CLIENT_STATE_MADE_REQ = 2;
        private static readonly int CLIENT_STATE_OPEN = 3;

        private AltingChannelInput fromServer;

        private ChannelOutput openToServer;
        private ChannelOutput reqToServer;
        private ChannelOutput backToClient;

        private ConnectionClientMessage message = new ConnectionClientMessage();
        private ConnectionClientOpenMessage messageOpen = new ConnectionClientOpenMessage();

        /**
         * Constructs a new instance. This constructor must be called by a subclass which is responsible
         * for creating the channels used by the connection and must pass them into this constructor.
         */
        protected internal AltingConnectionClientImpl(AltingChannelInput fromServer,
                                             ChannelOutput openToServer,
                                             ChannelOutput reqToServer,
                                             ChannelOutput backToClient) : base(fromServer)
        {
            this.fromServer = fromServer;
            this.openToServer = openToServer;
            this.reqToServer = reqToServer;
            this.backToClient = backToClient;
            currentClientState = CLIENT_STATE_CLOSED;
        }

        /**
         * Sends some data over the connection to server once the
         * connection has been opened.
         *
         * @param data	the <code>Object</code> to send to the server.
         */
        public void request(Object data)// throws IllegalStateException
        {
            try
            {
                if (currentClientState == CLIENT_STATE_MADE_REQ)
                    //throw new IllegalStateException ("Cannot call request(Object) twice without calling reply().");
                    //this will Claim the use of the client
                    if (currentClientState == CLIENT_STATE_CLOSED)
                    {
                        claim();
                        messageOpen.data = data;
                        messageOpen.replyChannel = backToClient;
                        openToServer.write(messageOpen);
                    }
                    else
                    {
                        message.data = data;
                        reqToServer.write(message);
                    }
                currentClientState = CLIENT_STATE_MADE_REQ;
            }
            catch (InvalidOperationException)
            {

                throw;
            }
        }

        /**
         * Receives some data back from the server after
         * <code>request(Object)</code> has been called.
         *
         * @return the <code>Object</code> sent from the server.
         */
        public Object reply()// throws 
        {
            try
            {
                //moved it out from the if statement below and had to initialize it with empty object - KP
                ConnectionServerMessage serverReply = new ConnectionServerMessage();

                if (currentClientState != CLIENT_STATE_MADE_REQ)
                {
                    try
                    {
                        serverReply = (ConnectionServerMessage)fromServer.read();
                    }
                    catch (InvalidOperationException)
                    {
                        throw new InvalidOperationException("Cannot call reply() on a ConnectionClient that is not waiting for a reply.");
                    }

                }

                //check whether the server closed the connection
                currentClientState = serverReply.open ? CLIENT_STATE_OPEN : CLIENT_STATE_CLOSED;
                if (serverReply.open)
                    currentClientState = CLIENT_STATE_OPEN;
                else
                {
                    currentClientState = CLIENT_STATE_CLOSED;
                    release();
                }
                return serverReply.data;
            }
            catch (InvalidOperationException)
            {

                throw;
            }
        }

        /**
         * Returns whether the server has kept its end of the Connection open.
         * This should only be called after a call to <code>reply()</code> and
         * before any other Connection method is called.
         *
         * @return <code>true</code> iff the server has kept the connection
         *          open.
         */
        public Boolean isOpen()// throws 
        {
            if (currentClientState == CLIENT_STATE_MADE_REQ)
            {
                throw new InvalidOperationException("Can only call isOpen() just after a reply has been received from the server.");
            }

            return currentClientState == CLIENT_STATE_OPEN;
        }

        /**
         * This claims a lock on the client.
         * This implementation does nothing as instances of this
         * class are only meant to be used with One2?Connection objects.
         *
         */
        protected void claim()
        {
        }

        /**
         * This releases a lock on the client.
         * This implementation does nothing as instances of this
         * class are only meant to be used with One2?Connection objects.
         *
         */
        protected void release()
        {
        }

        public override bool enable(Alternative alt)
        {
            throw new NotImplementedException();
        }

        public override bool disable()
        {
            throw new NotImplementedException();
        }
    }
}
