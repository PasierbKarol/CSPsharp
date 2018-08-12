using CSPlang.Any2;

namespace CSPlang
{

    class PoisonableBufferedAny2AnyChannelInt : Any2AnyIntImpl
    {
        PoisonableBufferedAny2AnyChannelInt(ChannelDataStoreInt _data, int _immunity) :  base(new PoisonableBufferedAny2AnyChannelInt(_data, _immunity))
        {
            
        }
    }
}