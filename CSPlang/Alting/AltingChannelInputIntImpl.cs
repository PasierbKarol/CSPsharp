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

        public void endRead()
        {
            channel.endRead();
        }

        public int read()
        {
            return channel.read();
        }

        public int startRead()
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
    }
}
