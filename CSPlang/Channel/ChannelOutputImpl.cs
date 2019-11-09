using System;

namespace CSPlang
{
    public class ChannelOutputImpl : ChannelOutput
    {
        private ChannelInternals channel;
        private int immunity;

        internal ChannelOutputImpl(ChannelInternals _channel, int _immunity)
        {
            channel = _channel;
            immunity = _immunity;
        }

        public void write(Object _object)
        {
            //Debug.WriteLine("Writing channel output " + _object.ToString(), "DEBUG");
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