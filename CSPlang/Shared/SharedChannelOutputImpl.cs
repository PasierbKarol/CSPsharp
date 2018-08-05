using System;
using CSPlang.Shared;

namespace CSPlang.Any2
{
    internal class SharedChannelOutputImpl : SharedChannelOutput
    {
        private ChannelInternals channel;
        private int immunity;

        internal SharedChannelOutputImpl(ChannelInternals _channel, int _immunity)
        {
            channel = _channel;
            immunity = _immunity;
        }

        public void write(Object _object)
        {
            channel.write(_object);

        }

        public void poison(int strength)
        {
            if (strength > immunity)
            {
                channel.writerPoison(strength);
            }
        }
    }
}