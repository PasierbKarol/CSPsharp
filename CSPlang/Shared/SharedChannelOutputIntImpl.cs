namespace CSPlang.Shared
{
    public class SharedChannelOutputIntImpl : SharedChannelOutputInt
    {

        private ChannelInternalsInt channel;
        private int immunity;

        internal SharedChannelOutputIntImpl(ChannelInternalsInt _channel, int _immunity)
        {
            channel = _channel;
            immunity = _immunity;
        }

        public void write(int objectToWrite)
        {
            channel.write(objectToWrite);
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