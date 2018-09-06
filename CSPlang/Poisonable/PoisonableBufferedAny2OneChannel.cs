using CSPlang.Any2;
using CSPutil;

namespace CSPlang
{
    internal class PoisonableBufferedAny2OneChannel : Any2OneImpl
    {
        internal PoisonableBufferedAny2OneChannel(ChannelDataStore _data, int _immunity) : base(new PoisonableBufferedOne2OneChannel(_data, _immunity))
        {
            
        }
    }
}