using System;
using CSPlang.Shared;

namespace CSPlang.Any2
{
    class Any2AnyImpl : Any2AnyChannel, ChannelInternals
    {
        private ChannelInternals channel;
        /** The mutex on which readers must synchronize */
        private readonly CSPMutex _readCspMutex = new CSPMutex();
        private readonly Object writeMonitor = new Object();

        public Any2AnyImpl(ChannelInternals _channel)
        {
            channel = _channel;
        }

        public SharedChannelInput In()
        {
            return new SharedChannelInputImpl(this, 0);
        }

        public SharedChannelOutput Out()
        {
            return new SharedChannelOutputImpl(this, 0);
        }

        public void endRead()
        {
            channel.endRead();
            _readCspMutex.Release();

        }

        public Object read()
        {
            _readCspMutex.Claim();
            //		A poison exception might be thrown, hence the try/finally:		
            try
            {
                return channel.read();
            }
            finally
            {
                _readCspMutex.Release();
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
            _readCspMutex.Claim();
            channel.readerPoison(strength);
            _readCspMutex.Release();
        }

        public Object startRead()
        {
            _readCspMutex.Claim();
            try
            {
                return channel.startRead();
            }
            catch (RuntimeException e)
            {
                channel.endRead();
                _readCspMutex.Release();
                throw e;
            }
        }

        public void write(Object obj)
        {
            lock (writeMonitor)
            {
                channel.write(obj);
            }
        }

        public void writerPoison(int strength)
        {
            lock (writeMonitor)
            {
                channel.writerPoison(strength);
            }
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
    }
}
