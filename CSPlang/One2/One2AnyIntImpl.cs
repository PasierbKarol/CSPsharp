using System;

namespace CSPlang
{

    class One2AnyIntImpl : One2AnyChannelInt, ChannelInternalsInt
    {

        private ChannelInternalsInt channel;

        /** The mutex on which readers must synchronize */
        private readonly CSPMutex readMutex = new CSPMutex();

        One2AnyIntImpl(ChannelInternalsInt _channel)
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
            readMutex.release();

        }

        public int read()
        {
            readMutex.claim();
            //A poison exception might be thrown, hence the try/finally:		
            try
            {
                return channel.read();
            }
            finally
            {
                readMutex.release();
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
            readMutex.claim();
            channel.readerPoison(strength);
            readMutex.release();
        }

        public int startRead()
        {
            readMutex.claim();
            try
            {
                return channel.startRead();
            }
            catch (RuntimeException e)
            {
                channel.endRead();
                readMutex.release();
                throw e;
            }

        }

        //begin never used
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