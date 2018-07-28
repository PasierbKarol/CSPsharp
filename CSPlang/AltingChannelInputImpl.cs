using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang
{
    class AltingChannelInputImpl
    {

        private ChannelInternals channel;
        private int immunity;

        AltingChannelInputImpl(ChannelInternals _channel, int _immunity)
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
    }
}
