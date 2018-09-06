using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang
{
    /**
 * <p>This interface should be implemented by classes that wish to
 * act as connection servers and to accept requests from
 * <code>ConnectionClient</code> objects.</p>
 *
 * <p>The server can call <code>request()</code> to allow a client
 * to establish a connection to the server and to obtain the client's
 * initial request. This should block until a client establishes a
 * connection.</p>
 *
 * <p>Once a request has been received, the server should reply to the client.
 * If the server wants to close the connection then the server should call
 * <code>replyAndClose(Object)</code> or alternatively
 * <code>reply(Object, Boolean)</code> with the <code>Boolean</code> set to
 * <code>true</code>. If the server wants to keep the connection open, then it
 * should call <code>reply(Object)</code> or alternatively
 * <code>reply(Object, Boolean)</code> with the <code>Boolean</code> set to
 * <code>false</code>.  The <code>reply(Object, Boolean)</code> method is
 * provided for convenience in closing connections programatically.</p>
 *
 *
 */
    public interface ConnectionServer
    {
        /**
     * The factory for creating channels within servers.
     */
        static StandardChannelFactory FACTORY = new StandardChannelFactory();

        /**
         * <p>Receives a request from a client. This will block until the client
         * calls its <code>request(Object)</code> method. Implementations may
         * make this ALTable.</p>
         *
         * <p>After this method has returned, the server should call one of the
         * reply methods. Performing any external process synchronization
         * between these method calls could be potentially hazardous and could
         * lead to deadlock.</p>
         *
         * @return the <code>Object</code> sent by the client.
         */
          Object request(); // throws IllegalStateException; //No equivalent in C# - KP

        /**
         * <p>Sends some data back to the client after a request
         * has been received but keeps the connection open. After calling
         * this method, the server should call <code>recieve()</code>
         * to receive a further request.</p>
         *
         * <p>A call to this method is equivalent to a call to
         * <code>reply(Object, Boolean)</code> with the Boolean set to
         * <code>false</code>.</p>
         *
         * @param	data	the data to send to the client.
         */
          void reply(Object data); // throws IllegalStateException; //No equivalent in C# - KP


        /**
         * <p>Sends some data back to the client after a request
         * has been received. The <code>Boolean</code> close parameter
         * indicates whether the connection should be closed after this
         * reply has been sent.</p>
         *
         * <p>This method should not block.</p>
         *
         * @param data	  the data to send back to client.
         * @param close  <code>Boolean</code> that should be <code>true</code>
         *                iff the connection should be dropped after the reply
         *                has been sent.
         */
        void reply(Object data, Boolean close);

        /**
         * <p>Sends some data back to the client and closes the connection.</p>
         *
         * <p>A call to this method is equivalent to a call to
         * <code>reply(Object, Boolean)</code> with the Boolean set to
         * <code>true</code>.</p>
         *
         * @param data	the data to send back to client.
         */
          void replyAndClose(Object data); // throws IllegalStateException; //No equivalent in C# - KP
    }
}
