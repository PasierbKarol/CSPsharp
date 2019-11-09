using System;

namespace CSPlang
{
    public interface ChannelInternals
    {
        Object read();
        void write(Object obj);

        Object startRead();
        void endRead();

        Boolean readerEnable(Alternative alt);
        Boolean readerDisable();
        Boolean readerPending();

        //For Symmetric channel, later:

        Boolean writerEnable(Alternative alt);
        Boolean writerDisable();
        Boolean writerPending();


        void readerPoison(int strength);
        void writerPoison(int strength);
    }
}