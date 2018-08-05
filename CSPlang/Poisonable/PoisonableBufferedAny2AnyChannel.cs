using CSPlang.Any2;
using CSPutil;

namespace CSPlang
{
    internal class PoisonableBufferedAny2AnyChannel : Any2AnyImpl
    {
        PoisonableBufferedAny2AnyChannel(ChannelDataStore _data, int _immunity) : base (new PoisonableBufferedOne2OneChannel(_data, _immunity))
        {
            
        }
    }
}