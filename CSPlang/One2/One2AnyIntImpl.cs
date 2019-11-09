using System;
using CSPlang.Shared;

namespace CSPlang
{
    class One2AnyIntImpl : One2AnyChannelInt, ChannelInternalsInt
    {
        private ChannelInternalsInt channel;

        /** The mutex on which readers must synchronize */
        private readonly CSPMutex readMutex = new CSPMutex();

        protected One2AnyIntImpl(ChannelInternalsInt _channel)
        {
            channel = _channel;
        }

        public SharedChannelInputInt In()
        {
            return new SharedChannelInputIntImpl(this, 0);
        }

        public ChannelOutputInt Out()
        {
            return new ChannelOutputIntImpl(channel, 0);
        }

        public void endRead()
        {
            channel.endRead();
            readMutex.Release();
        }

        public int read()
        {
            readMutex.Claim();
            //A poison exception might be thrown, hence the try/finally:		
            try
            {
                return channel.read();
            }
            finally
            {
                readMutex.Release();
            }
        }

        //begin never used: //TODO check whether this can be removed
        public Boolean readerDisable()
        {
            return false;
        }

        public Boolean readerEnable(Alternative alt)
        {
            return false;
        }

        public Boolean readerPending()
        {
            return false;
        }
        //end never used

        public void readerPoison(int strength)
        {
            readMutex.Claim();
            channel.readerPoison(strength);
            readMutex.Release();
        }

        public int startRead()
        {
            readMutex.Claim();
            try
            {
                return channel.startRead();
            }
            catch (RuntimeException e)
            {
                channel.endRead();
                readMutex.Release();
                throw e;
            }

        }

        //begin never used //TODO as above
        public void write(int n)
        {
            channel.write(n);
        }

        public void writerPoison(int strength)
        {
            channel.writerPoison(strength);
        }
        //end never used
    }
}