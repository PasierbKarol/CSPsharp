namespace CSPlang
{
    public interface One2OneChannel
    {
        /**
 * Returns the input channel end.
 */
        AltingChannelInput _in();

        /**
         * Returns the output channel end.
         */
        ChannelOutput _out();
    }
}