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

namespace CSPlang
{
    /**
     * <p>This is an interface to be implemented by classes that wish
     * to act as a client to connect to a <code>ConnectionServer</code>.</p>
     *
     * <p>Users of classes implementing this interface should call
     * <code>request(Object)</code> to initiate a conversation and to send some
     * data to the server.  Implementations may decide to return immediately or
     * to wait until the server accepts the connection and then return. The
     * Connection is not guaranteed to be open until a call to <code>reply()</code>
     * has returned.  The <code>reply()</code> method should be called <I>soon</I>
     * after the call to <code>reqeust(Object)</code>. Some computation may be
     * done between the calls but any external process synchronization is
     * potentially hazardous.</p>
     *
     * <p>After calling <code>reply()</code>, clients can check whether
     * the server closed the connection by calling <code>isOpen()</code>.
     * If it returns <code>true</code>, then the connection has been kept
     * open. If the connection has been kept open then the client may assume
     * that a call to <code>request(Object)</code> will not block and that
     * the connection will <I>soon</I> be dealt with by the server.</p>
     *
     * <p>This is an example of typical code structure for using a
     * ConnectionClient:</p>
     *
     * <pre>
     * //have a variable client of type ConnectionClient
     * do {
     *     client.request(some_data);
     *     some_variable = client.receive();
     * } while (client.isOpen())
     * </pre>
     */
    public interface ConnectionClient
    {
        /**
         * <p>This method is used to send data to a <code>ConnectionServer</code> in
         * a client/server conversation. If a connection has not yet been established,
         * then this method will open the connection as necessary.</p>
         *
         * <p>Once this method has returned, the client may do some computation but
         * must then guarantee to call <code>reply()</code>. This will obtain a
         * server's response to the request. In between calling this method and
         * <code>reply()</code>, doing pure computation is safe. Performing
         * synchronization with other process is potentially hazardous.</p>
         *
         * <p>Once a server replies, if the connection has been kept open, then
         * this method should be called again to make a further request.</p>
         *
         * <p>Programs using <code>Connection</code>s need to adopt a protocol so that the server
         * knows when a conversation with a client has finished and will then drop
         * the connection.</p>
         *
         * @param data	the <code>Object</code> to send to the server.
         * @throws  IllegalStateException	if the method is called when it is
         *                                  not meant to be.
         */
        void request(Object data);// throws IllegalStateException; //TODO is there a way to add exceptions here?

        /**
         * <p>Receives some data back from the server after
         * <code>request(Object)</code> has been called.</p>
         *
         * <p>After calling this method, <code>isOpen()</code> may be called
         * to establish whether the server dropped the connection after replying.</p>
         *
         * <p>Implementations may make this operation ALTable.</p>
         *
         * @return the <code>Object</code> sent from the server.
         * @throws  IllegalStateException	if the method is called when it is
         *                                  not meant to be.
         */
        Object reply();// throws IllegalStateException; //TODO is there a way to add exceptions here?

        /**
         * <p>Returns whether the server has kept its end of the Connection open.
         * This should only be called after a call to <code>reply()</code> and
         * before any other Connection method is called.</p>
         *
         * @return <code>true</code> iff the server has kept the connection
         *          open.
         */
        Boolean isOpen();// throws IllegalStateException; //TODO is there a way to add exceptions here?
    }
}