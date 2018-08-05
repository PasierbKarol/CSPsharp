using CSPlang.Shared;

namespace CSPlang
{
    public interface One2AnyChannel
    {
        /**
 * Returns the input end of the channel.
 */
        SharedChannelInput In();

        /**
         * Returns the output end of the channel.
         */
        ChannelOutput Out();
    }
}