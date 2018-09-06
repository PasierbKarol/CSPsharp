using System;

namespace CSPlang
{

    class RejectableChannelInputImpl : ChannelInputImpl, RejectableChannelInput
    {

        public RejectableChannelInputImpl(ChannelInternals _channel, int _immunity) : base(_channel, _immunity)
        {
            
        }

        public void reject()
        {
            base.poison(Int32.MaxValue);
        }
    }
}
