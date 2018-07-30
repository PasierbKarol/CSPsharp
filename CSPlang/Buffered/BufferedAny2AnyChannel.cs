using CSPlang.Any2;
using CSPutil;

namespace CSPlang
{
    internal class BufferedAny2AnyChannel : Any2AnyImpl
    {
        /**
 * Constructs a new BufferedAny2AnyChannel with the specified ChannelDataStore.
 *
 * @param data The ChannelDataStore used to store the data for the channel
 */
           
        public BufferedAny2AnyChannel(ChannelDataStore data) : base(new BufferedOne2OneChannel(data))
        {
        }
    }
}