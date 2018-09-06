namespace CSPlang
{

    public class ChannelOutputIntImpl : ChannelOutputInt {

    private ChannelInternalsInt channel;
    private int immunity;

        internal ChannelOutputIntImpl(ChannelInternalsInt _channel, int _immunity)
    {
        channel = _channel;
        immunity = _immunity;
    }

    public void write(int objectToWrite) {
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