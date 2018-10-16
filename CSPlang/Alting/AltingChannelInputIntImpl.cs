using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang.Alting
{
    class AltingChannelInputIntImpl : AltingChannelInputInt
    {
        private ChannelInternalsInt channel;
        private int immunity;

        internal AltingChannelInputIntImpl(ChannelInternalsInt _channel, int _immunity)
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

        public override void endRead()
        {
            channel.endRead();
        }

        public override int read()
        {
            return channel.read();
        }

        public override int startRead()
        {
            return channel.startRead();
        }

        public override void poison(int strength)
        {
            if (strength > immunity)
            {
                channel.readerPoison(strength);
            }
        }
    }
}
