using System;
using CSPlang;

namespace PlugAndPlay
{
    public class GConsole : IamCSProcess 
    {
        private ChannelInput toConsole;
        private ChannelOutput fromConsole;
        private ChannelInput clearInputArea;
        private String frameLabel;

        public GConsole(ChannelInput toConsole, ChannelOutput fromConsole, ChannelInput clearInputArea, string frameLabel)
        {
            this.toConsole = toConsole;
            this.fromConsole = fromConsole;
            this.clearInputArea = clearInputArea;
            this.frameLabel = frameLabel;
        }

        public GConsole(ChannelInput toConsole, string frameLabel)
        {
            this.toConsole = toConsole;
            this.frameLabel = frameLabel;
        }

        public void run()
        {
            while (true)
            {
                Console.WriteLine(frameLabel + "\t" + toConsole.read());
            }

        }
    }
}
