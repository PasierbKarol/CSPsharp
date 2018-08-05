namespace CSPlang
{
    public interface One2OneChannel
    {
        /**
 * Returns the input channel end.
 */
        AltingChannelInput In();

        /**
         * Returns the output channel end.
         */
        ChannelOutput Out();
    }
}