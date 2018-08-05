using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang.Alting
{
    class AltingChannelInputIntImpl : AltingChannelInputInt
    {
        private ChannelInternalsInt channel;
        private int immunity;

        AltingChannelInputIntImpl(ChannelInternalsInt _channel, int _immunity)
        {
            channel = _channel;
            immunity = _immunity;
        }


        public Boolean pending()
        {
            return channel.readerPending();
        }

        Boolean disable()
        {
            return channel.readerDisable();
        }

        Boolean enable(Alternative alt)
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
