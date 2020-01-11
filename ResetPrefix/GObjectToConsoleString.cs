using System;
using CSPlang;

namespace ResetPrefix
{
    class GObjectToConsoleString : IamCSProcess
    {
        ChannelInput inChannel;
        ChannelOutput outChannel;

        public GObjectToConsoleString(ChannelInput inChannel, ChannelOutput outChannel)
        {
            this.inChannel = inChannel;
            this.outChannel = outChannel;
        }

        public void run()
        {
            string input = inChannel.read().ToString();
            Console.WriteLine(input);
            outChannel.write(input);
        }
    }
}