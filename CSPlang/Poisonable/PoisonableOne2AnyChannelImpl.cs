using CSPlang.One2;

namespace CSPlang
{
    internal class PoisonableOne2AnyChannelImpl : One2AnyImpl
    {
        internal PoisonableOne2AnyChannelImpl(int _immunity) : base(new PoisonableOne2OneChannelImpl(_immunity))
        {
            
        }
    }
}