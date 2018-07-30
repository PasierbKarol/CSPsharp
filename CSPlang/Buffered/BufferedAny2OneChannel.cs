using CSPlang.Any2;
using CSPutil;

namespace CSPlang
{
    internal class BufferedAny2OneChannel : Any2OneImpl
    {
        /**
         * Constructs a new BufferedAny2OneChannel with the specified ChannelDataStore.
         *
         * @param data The ChannelDataStore used to store the data for the channel
         */
        public BufferedAny2OneChannel(ChannelDataStore data) : base(new BufferedOne2OneChannel(data))
        {
           
        }
    }
}