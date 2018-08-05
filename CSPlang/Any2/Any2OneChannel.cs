using CSPlang.Shared;

namespace CSPlang.Any2
{
    public interface Any2OneChannel
    {
        /**
     * Returns the input end of the channel.
     */
        AltingChannelInput In();

        /**
         * Returns the output end of the channel.
         */
        SharedChannelOutput Out();
    }
}