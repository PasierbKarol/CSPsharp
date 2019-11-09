using CSPlang.Any2;
using CSPutil;

namespace CSPlang
{

    class PoisonableBufferedAny2OneChannelInt : Any2OneIntImpl
    {
        internal PoisonableBufferedAny2OneChannelInt(ChannelDataStoreInt _data, int _immunity) :
            base(new PoisonableBufferedOne2OneChannelInt(_data, _immunity))
        {

        }
    }
}