namespace CSPlang
{
    public interface One2OneChannel
    {
        /**
 * Returns the input channel end.
 */
        public AltingChannelInput in();

        /**
         * Returns the output channel end.
         */
        public ChannelOutput out();
    }
}