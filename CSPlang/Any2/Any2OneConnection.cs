namespace CSPlang.Any2
{
    public interface Any2OneConnection : ConnectionWithSharedAltingClient
    {
        /**
 * Returns a client end of the connection. This may only be
 * safely used by a single process but further calls will
 * return new clients which may be used by other processes.
 *
 * @return a new <code>SharedAltingConnectionClient</code> object.
 */
        public SharedAltingConnectionClient client();

        /**
         * Returns the server end of the connection.
         *
         * @return the instance of the <code>AltingConnectionServer</code>.
         */
        public AltingConnectionServer server();
    }
}