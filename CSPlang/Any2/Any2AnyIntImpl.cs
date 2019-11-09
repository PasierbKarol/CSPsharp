using System;
using CSPlang.Shared;

namespace CSPlang.Any2
{
    public class Any2AnyIntImpl : Any2AnyChannelInt, ChannelInternalsInt
    {
        private ChannelInternalsInt channel;
        /** The mutex on which readers must synchronize */
        private readonly CSPMutex readMutex = new CSPMutex();
        private readonly Object writeMonitor = new Object();

        protected Any2AnyIntImpl(ChannelInternalsInt _channel)
        {
            channel = _channel;
        }

        public SharedChannelInputInt In()
        {
            return new SharedChannelInputIntImpl(this, 0);
        }

        public SharedChannelOutputInt Out()
        {
            return new SharedChannelOutputIntImpl(this, 0);
        }

        public void endRead()
        {
            channel.endRead();
            readMutex.Release();

        }

        public int read()
        {
            readMutex.Claim();
            //		A poison exception might be thrown, hence the try/readonlyly:		
            try
            {
                return channel.read();
            }
            finally
            {
                readMutex.Release();
            }
        }

        //begin never used:
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
    }
}