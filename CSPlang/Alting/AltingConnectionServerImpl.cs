﻿//////////////////////////////////////////////////////////////////////
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
 * This class does not need to be used by standard JCSP users. It is exposed so that the connection
 * mechanism can be extended for custom connections.
 *
 *
 */


    public class AltingConnectionServerImpl : AltingConnectionServer
    {
        /**
     * Server state. The server is initially <tt>CLOSED</tt> the first request will take it to the
     * <tt>RECEIVED</tt> state. A reply will take it back to <tt>OPEN</tt> or <tt>CLOSED</tt> depending
     * on the mode of reply. From the <tt>OPEN</tt> or <tt>CLOSED</tt> state a further request can
     * occur.
     */
        protected internal static readonly int SERVER_STATE_CLOSED = 1;

        /**
         * Server state. The server is initially <tt>CLOSED</tt> the first request will take it to the
         * <tt>RECEIVED</tt> state. A reply will take it back to <tt>OPEN</tt> or <tt>CLOSED</tt> depending
         * on the mode of reply. From the <tt>OPEN</tt> or <tt>CLOSED</tt> state a further request can
         * occur.
         */
        protected static readonly int SERVER_STATE_OPEN = 2;

        /**
         * Server state. The server is initially <tt>CLOSED</tt> the first request will take it to the
         * <tt>RECEIVED</tt> state. A reply will take it back to <tt>OPEN</tt> or <tt>CLOSED</tt> depending
         * on the mode of reply. From the <tt>OPEN</tt> or <tt>CLOSED</tt> state a further request can
         * occur.
         */
        protected static readonly int SERVER_STATE_RECEIVED = 3;

        private int currentServerState;

        private AltingChannelInput openIn;

        private AltingChannelInput furtherRequestIn;

        private ChannelInput currentInputChannel;

        private ChannelOutput toClient = null;

        private ConnectionServerMessage msg = null;

        /**
         * Constructs a new server instance. This must be called by a subclass which is responsible for
         * creating the channels.
         */
        protected internal AltingConnectionServerImpl(AltingChannelInput openIn,
                                             AltingChannelInput furtherRequestIn) : base(openIn)
        {
            this.openIn = openIn;
            this.furtherRequestIn = furtherRequestIn;
            this.currentInputChannel = openIn;
            currentServerState = SERVER_STATE_CLOSED;
        }

        /**
         * Receives some data from a client once a connection
         * has been established. This will block until the client
         * calls <code>request(Object)</code> but by establishing
         * a connection.
         *
         * @return the <code>Object</code> sent by the client.
         */
        public Object request() //throws IllegalStateException
        {
            if (currentServerState == SERVER_STATE_RECEIVED)
            {
                throw new InvalidOperationException("Cannot call request() twice on ConnectionServer without replying to the client first.");
            }
            ConnectionClientMessage msg = (ConnectionClientMessage)currentInputChannel.read();

            if (currentServerState == SERVER_STATE_CLOSED)
            {
                if (msg is ConnectionClientOpenMessage)
                {
                    //channel to use to reply to client
                    toClient = ((ConnectionClientOpenMessage)msg).replyChannel;
                    setAltingChannel(furtherRequestIn);
                    currentInputChannel = furtherRequestIn;

                    //create a new msg for connection established
                    //don't know if client implementation will have finished with
                    //message after connection closed
                    this.msg = new ConnectionServerMessage();
                }
                else
                {
                    throw new InvalidOperationException("Invalid message received from client");
                }
            }
            currentServerState = SERVER_STATE_RECEIVED;
            return msg.data;
        }

        /**
         * Sends some data back to the client after a request
         * has been received but keeps the connection open. After calling
         * this method, the server should call <code>recieve()</code>
         * to receive a further request.
         *
         * @param	data	the data to send to the client.
         */
        public void reply(Object data)
        {
            try
            {
                reply(data, false);
            }
            catch (InvalidOperationException)
            {

                throw;
            }
        }


        /**
         * Sends some data back to the client after a request
         * has been received. The closed parameter indicates whether or not
         * the connection should be closed. The connection will be closed
         * iff close is <code>true</code>.
         *
         * @param	data	the data to send to the client.
         * @param  close   <code>boolean</code> indicating whether or not the
         *                  connection should be closed.
         */
        public void reply(Object data, Boolean close)// throws 
        {
            try
            {
                if (currentServerState != SERVER_STATE_RECEIVED)
                    throw new InvalidOperationException("Cannot call reply(Object, boolean) on a ConnectionServer that has not received an unacknowledge request.");

                //set open to true before replying
                msg.data = data;
                msg.open = !close;
                toClient.write(msg);
                if (close)
                {
                    currentServerState = SERVER_STATE_CLOSED;
                    toClient = null;
                    setAltingChannel(openIn);
                    currentInputChannel = openIn;
                }
                else
                    currentServerState = SERVER_STATE_OPEN;
            }
            catch (InvalidOperationException)
            {

                throw;
            }
        }

        /**
         * Sends some data back to the client and closes the connection.
         * This method will not block. After calling this method, the
         * server may call <code>accept()</code> in order to allow another
         * connection to this server to be established.
         *
         * If this method did not take any data to send back to the client,
         * and the server was meant to call <code>reply(Object)</code> followed
         * by a <code>close()</code>, then there would be a race hazard at the
         * client as it would not know whether the connection had
         * remained open or not.
         *
         * @param data	the data to send back to client.
         */
        public void replyAndClose(Object data) //throws 
        {
            try
            {
                reply(data, true);
            }
            catch (InvalidOperationException)
            {

                throw;
            }
        }

        protected internal int getServerState()
        {
            return currentServerState;
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