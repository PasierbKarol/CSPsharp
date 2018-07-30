using CSPlang.One2;
using CSPutil;

namespace CSPlang
{
    internal class BufferedOne2AnyChannel : One2AnyImpl
    {
        /**
         * Constructs a new BufferedOne2AnyChannel with the specified ChannelDataStore.
         *
         * @param data The ChannelDataStore used to store the data for the channel
         */
        public BufferedOne2AnyChannel(ChannelDataStore data) : base(new BufferedOne2OneChannel(data))
        {
            
        }
    }
}