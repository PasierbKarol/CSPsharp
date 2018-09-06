using CSPlang.Any2;

namespace CSPlang
{

    class PoisonableAny2AnyChannelIntImpl : Any2AnyIntImpl
    {
        internal PoisonableAny2AnyChannelIntImpl(int _immunity) : base(new PoisonableOne2OneChannelIntImpl(_immunity))
        {

        }
    }
}