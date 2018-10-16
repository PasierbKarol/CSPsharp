using System;
using CSPlang;
using CSPutil;


namespace ConsumerProducer
{
    public class Producer : IamCSProcess
    {
        ChannelOutputInt outChannel;

        public Producer(ChannelOutputInt outChannel)
        {
            this.outChannel = outChannel;
        }

        public void run()
        {
            int i = 1000;

            while (i > 0)
            {
                Console.Write("\nEnter next number (-100, 100):\t");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out i))
                {
                    Console.WriteLine("You have entered invalid number");
                    continue;                }
                else
                {

                    outChannel.write(i);
                }
            }
        }
    }
}