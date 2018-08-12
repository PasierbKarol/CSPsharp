using System;
using System.Collections.Generic;
using System.Text;
using CSPlang.Alting;

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
        private AltingConnectionServer server;
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
            server = new AltingConnectionServerImpl(chanToServer.In(), chanToServer.In());
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
            return server;
        }

    }
}
