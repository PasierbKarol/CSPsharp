using CSPlang.Any2;
using CSPutil;

namespace CSPlang
{

    class PoisonableBufferedAny2AnyChannelInt : Any2AnyIntImpl
    {
        internal PoisonableBufferedAny2AnyChannelInt(ChannelDataStoreInt _data, int _immunity) :
            base(new PoisonableBufferedAny2AnyChannelInt(_data, _immunity))
        {

        }
    }
}