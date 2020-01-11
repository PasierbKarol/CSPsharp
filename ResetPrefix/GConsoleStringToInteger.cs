using System;
using CSPlang;

namespace ResetPrefix
{
    class GConsoleStringToInteger : IamCSProcess
    {
        ChannelInput inChannel;
        ChannelOutput outChannel;

        public GConsoleStringToInteger(ChannelInput inChannel, ChannelOutput outChannel)
        {
            this.inChannel = inChannel;
            this.outChannel = outChannel;
        }

        public void run()
        {
            int input = Int32.Parse(inChannel.read().ToString());
            Console.WriteLine(input + " - It is an Int");
            outChannel.write(input);
        }
    }
}