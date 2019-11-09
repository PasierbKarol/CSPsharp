namespace CSPlang
{
    public class RejectableChannelOutputImpl : ChannelOutputImpl, RejectableChannelOutput
    {
        public RejectableChannelOutputImpl(ChannelInternals _channel, int _immunity) : base(_channel, _immunity)
        {

        }
    }
}
