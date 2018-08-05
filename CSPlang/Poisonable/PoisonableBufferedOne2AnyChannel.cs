using CSPlang.One2;
using CSPutil;

namespace CSPlang
{
    internal class PoisonableBufferedOne2AnyChannel : One2AnyImpl
    {
        private ChannelDataStore buffer;
        private int immunity;

        PoisonableBufferedOne2AnyChannel(ChannelDataStore _data, int _immunity) : base(new PoisonableBufferedOne2OneChannel(_data, _immunity))
        {
            
        }
    }
}