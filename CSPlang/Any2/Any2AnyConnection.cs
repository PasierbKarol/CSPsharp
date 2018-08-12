namespace CSPlang.Any2
{
    public interface Any2AnyConnection : ConnectionWithSharedAltingClient, ConnectionWithSharedAltingServer
    {
        /**
     * Returns a reference to the client end of the connection for use by the client processes.
     */
        SharedAltingConnectionClient client();

        /**
         * Returns a reference to the server end of the connection for use by the server processes.
         */
        SharedConnectionServer server();
    }
}