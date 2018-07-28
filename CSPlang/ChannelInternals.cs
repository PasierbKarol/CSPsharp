using System;

namespace CSPlang
{
    public interface ChannelInternals
    {
        /*public*/ Object read();
        /*public*/ void write(Object obj);

        /*public*/ Object startRead();
        /*public*/ void endRead();

        /*public*/ Boolean readerEnable(Alternative alt);
        /*public*/ Boolean readerDisable();
        /*public*/ Boolean readerPending();

        /*//For Symmetric channel, later:
        /*public*/ Boolean writerEnable(Alternative alt);
        /*public*/ Boolean writerDisable();
        /*public*/ Boolean writerPending();
       

        /*public*/ void readerPoison(int strength);
        /*public*/ void writerPoison(int strength);
    }
}