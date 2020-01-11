using System;
using CSPlang;

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
                string input = Console.ReadLine();

                if (!int.TryParse(input, out i))
                {
                    Console.WriteLine("You have entered invalid number");
                }
                else
                {
                    outChannel.write(i);
                }
            }
        }
    }
}