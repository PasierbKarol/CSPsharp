using CSPlang.Any2;

namespace CSPlang
{
    internal class Any2AnyChannelImpl : Any2AnyImpl
    {
        public Any2AnyChannelImpl() : base(new One2OneChannelImpl())
        {
        }
    }
}