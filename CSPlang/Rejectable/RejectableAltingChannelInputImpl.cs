using System;

namespace CSPlang
{

    class RejectableAltingChannelInputImpl : RejectableAltingChannelInput
    {

        private ChannelInternals channel;
        private int immunity;

        internal RejectableAltingChannelInputImpl(ChannelInternals _channel, int _immunity)
        {
            channel = _channel;
            immunity = _immunity;
        }


        public override Boolean pending()
        {
            return channel.readerPending();
        }

        public override Boolean disable()
        {
            return channel.readerDisable();
        }

        public override Boolean enable(Alternative alt)
        {
            return channel.readerEnable(alt);
        }

        public void endRead()
        {
            channel.endRead();
        }

        public Object read()
        {
            return channel.read();
        }

        public Object startRead()
        {
            return channel.startRead();
        }

        public void poison(int strength)
        {
            if (strength > immunity)
            {
                channel.readerPoison(strength);
            }
        }


        public override void reject()
        {
            channel.readerPoison(Int32.MaxValue);
        }
    }
}