﻿using System;
using CSPlang;
using CSPutil;


namespace ConsumerProducer
{
    public class Consumer : IamCSProcess
    {
        ChannelInput inChannel;

        public Consumer(ChannelInput inChannel)
        {
            this.inChannel = inChannel;
        }

        public void run()
        {
            int i = 1000;

            while (i > 0)
            {
                i = (int)inChannel.read();
                Console.WriteLine("The input was " + i);
            }
            Console.WriteLine("Finished!");
        }
    }
}