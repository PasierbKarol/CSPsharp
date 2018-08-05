using CSPlang.Shared;

namespace CSPlang.Any2
{
    public interface Any2AnyChannel
    {
        /**
 * Returns the input end of the channel.
 */
        SharedChannelInput In();

        /**
         * Returns the output end of the channel.
         */
        SharedChannelOutput Out();
    }
}