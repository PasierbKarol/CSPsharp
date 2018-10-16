using System;
using CSPlang;
using CSPutil;


namespace ConsumerProducer
{
    public class Consumer : IamCSProcess
    {
        ChannelInputInt inChannel;

        public Consumer(ChannelInputInt inChannel)
        {
            this.inChannel = inChannel;
        }

        public void run()
        {
            int i = 1000;

            while (i > 0)
            {
                i = (int)inChannel.read();
                Console.WriteLine("\nThe input was " + i);
            }
            Console.WriteLine("Finished!");
        }
    }
}