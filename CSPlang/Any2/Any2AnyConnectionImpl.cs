namespace CSPlang.Any2
{
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
        public Any2AnyConnectionImpl()
        {
            super();
            chanToServer = ConnectionServer.FACTORY.createOne2One(new Buffer(1));
            chanFromServer = ConnectionServer.FACTORY.createOne2One(new Buffer(1));
            chanClientSynch = ConnectionServer.FACTORY.createAny2One(new Buffer(1));
            chanServerSynch = ConnectionServer.FACTORY.createAny2One(new Buffer(1));
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
                    chanFromServer.in (),
                    chanClientSynch.in (),
                    chanToServer.out (),
                    chanToServer.out (),
                    chanClientSynch.out (),
                    chanFromServer.out (),
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
                    chanToServer.in (),
                    chanToServer.in (),
                    chanServerSynch.in (),
                    chanServerSynch.out (),
                    this);
        }

    }
}