using System;
using CSPlang;
using CSPutil;


namespace ConsumerProducer
{
    public class Producer : IamCSProcess
    {
        ChannelOutput outChannel;

        public Producer(ChannelOutput outChannel)
        {
            this.outChannel = outChannel;
        }

        public void run()
        {
            int i = 1000;

            while (i > 0)
            {
                Console.Write("\nEnter next number (-100, 100):\t");
                i = Console.Read();
                outChannel.write(i);
            }
        }
    }
}