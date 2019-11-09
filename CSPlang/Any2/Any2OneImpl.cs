using System;
using CSPlang.Any2;
using CSPlang.Shared;

namespace CSPlang
{
    internal class Any2OneImpl : ChannelInternals, Any2OneChannel
    {
        private ChannelInternals channel;
        private readonly Object writeMonitor = new Object();

        internal Any2OneImpl(ChannelInternals _channel)
        {
            channel = _channel;
        }

        //Begin never used:
        public void endRead()
        {
            channel.endRead();
        }

        public Object read()
        {
            return channel.read();
        }

        public Boolean readerDisable()
        {
            return channel.readerDisable();
        }

        public Boolean readerEnable(Alternative alt)
        {
            return channel.readerEnable(alt);
        }

        public Boolean readerPending()
        {
            return channel.readerPending();
        }

        public Object startRead()
        {
            return channel.startRead();
        }
        //End never used

        public void write(Object obj)
        {
            /*synchronized*/
            lock (writeMonitor)
            {
                channel.write(obj);
            }
        }

        public void writerPoison(int strength)
        {
            /*synchronized*/
            lock (writeMonitor)
            {
                channel.writerPoison(strength);
            }
        }

        public AltingChannelInput In()
        {
            return new AltingChannelInputImpl(channel, 0);
        }

        public SharedChannelOutput Out()
        {
            return new SharedChannelOutputImpl(this, 0);
        }

        public bool writerEnable(Alternative alt)
        {
            throw new NotImplementedException();
        }

        public bool writerDisable()
        {
            throw new NotImplementedException();
        }

        public bool writerPending()
        {
            throw new NotImplementedException();
        }

        public void readerPoison(int strength)
        {
            channel.readerPoison(strength);

        }
    }
}