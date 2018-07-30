using CSPlang.Any2;

namespace CSPlang
{
    internal class PoisonableAny2AnyChannelImpl : Any2AnyImpl
    {
        PoisonableAny2AnyChannelImpl(int _immunity) : base(new PoisonableOne2OneChannelImpl(_immunity))
        {
            
        }
    }
}