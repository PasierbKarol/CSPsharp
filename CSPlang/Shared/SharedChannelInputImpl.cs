using System;
using CSPlang.Shared;

namespace CSPlang.Any2
{
    internal class SharedChannelInputImpl : SharedChannelInput
    {
        private ChannelInternals channel;
        private int immunity;

        internal SharedChannelInputImpl(ChannelInternals _channel, int _immunity)
        {
            channel = _channel;
            immunity = _immunity;
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