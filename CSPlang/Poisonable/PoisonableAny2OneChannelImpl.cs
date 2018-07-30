using CSPlang.Any2;

namespace CSPlang
{
    internal class PoisonableAny2OneChannelImpl : Any2OneImpl
    {
        internal PoisonableAny2OneChannelImpl(int _immunity) : base(new PoisonableOne2OneChannelImpl(_immunity))
        {
            
        }
    }
}