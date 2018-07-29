﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang.Any2
{
    class Any2AnyImpl : Any2AnyChannel, ChannelInternals
    {
        private ChannelInternals channel;
        /** The mutex on which readers must synchronize */
        private final Mutex readMutex = new Mutex();
        private final Object writeMonitor = new Object();

        public Any2AnyImpl(ChannelInternals _channel)
        {
            channel = _channel;
        }

        public SharedChannelInput in() {
            return new SharedChannelInputImpl(this,0);
        }

        public SharedChannelOutput out() { 
            return new SharedChannelOutputImpl(this,0);
        }

        public void endRead()
        {
            channel.endRead();
            readMutex.release();

        }

        public Object read()
        {
            readMutex.claim();
            //		A poison exception might be thrown, hence the try/finally:		
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
        public boolean readerDisable()
        {
            return false;
        }

        public boolean readerEnable(Alternative alt)
        {
            return false;
        }

        public boolean readerPending()
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

        public Object startRead()
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

        public void write(Object obj)
        {
            synchronized(writeMonitor) {
                channel.write(obj);
            }
        }

        public void writerPoison(int strength)
        {
            synchronized(writeMonitor) {
                channel.writerPoison(strength);
            }
        }
    }
}
