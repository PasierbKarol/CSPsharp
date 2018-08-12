using CSPlang.Any2;

namespace CSPlang
{

    class PoisonableAny2OneChannelIntImpl : Any2OneIntImpl
    {
        PoisonableAny2OneChannelIntImpl(int _immunity) : base(new PoisonableOne2OneChannelIntImpl(_immunity))
        {

        }
    }
}