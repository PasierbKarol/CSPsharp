using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using CSPlang.Any2;
using CSPlang.Shared;

namespace CSPlang.One2
{
    public class One2AnyImpl : One2AnyChannel, ChannelInternals
    {
        private ChannelInternals channel;
        /** The mutex on which readers must synchronize */
        private readonly CSPMutex _readCspMutex = new CSPMutex();

        internal One2AnyImpl(ChannelInternals _channel)
        {
            channel = _channel;
        }

        public SharedChannelInput In()
        {
            return new SharedChannelInputImpl(this, 0);
        }

        public ChannelOutput Out()
        {
            return new ChannelOutputImpl(channel, 0);
        }

        public void endRead()
        {
            channel.endRead();
            _readCspMutex.ReleaseMutex();

        }

        public Object read()
        {
            _readCspMutex.claim();
            //A poison exception might be thrown, hence the try/finally:		
            try
            {
                return channel.read();
            }
            finally
            {
                _readCspMutex.ReleaseMutex();
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
            _readCspMutex.claim();
            channel.readerPoison(strength);
            _readCspMutex.ReleaseMutex();
        }

        public Object startRead()
        {
            _readCspMutex.claim();
            try
            {
                return channel.startRead();
            }
            catch (RuntimeException e)
            {
                channel.endRead();
                _readCspMutex.ReleaseMutex();
                throw e;
            }

        }

        //begin never used
        public void write(Object obj)
        {
            channel.write(obj);
        }

        public void writerPoison(int strength)
        {
            channel.writerPoison(strength);
        }
        //end never used


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
