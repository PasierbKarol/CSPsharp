using System;
using System.Collections.Generic;
using System.Text;
using CSPlang.Alting;
using CSPlang.Shared;

namespace CSPlang.Any2
{
    class Any2OneIntImpl : ChannelInternalsInt, Any2OneChannelInt
    {
        private ChannelInternalsInt channel;
        private readonly Object writeMonitor = new Object();

        internal Any2OneIntImpl(ChannelInternalsInt _channel)
        {
            channel = _channel;
        }

        //Begin never used:
        public void endRead()
        {
            channel.endRead();
        }

        public int read()
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

        public void readerPoison(int strength)
        {
            channel.readerPoison(strength);

        }

        public int startRead()
        {
            return channel.startRead();
        }
        //End never used

        public void write(int n)
        {
            lock (writeMonitor)
            {
                channel.write(n);
            }
        }

        public void writerPoison(int strength)
        {
            lock (writeMonitor)
            {
                channel.writerPoison(strength);
            }
        }

        public AltingChannelInputInt In()
        {
            return new AltingChannelInputIntImpl(channel, 0);
        }

        public SharedChannelOutputInt Out()
        {
            return new SharedChannelOutputIntImpl(this, 0);
        }
    }
}
