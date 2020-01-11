using CSPlang;

namespace ResetPrefix
{
    class ResetUser : IamCSProcess
    {
        private ChannelOutput resetValue;
        private ChannelOutput toConsole;
        private ChannelInput fromConverter;
        private ChannelOutput toClearOutput;

        public ResetUser(ChannelOutput resetValue, ChannelOutput toConsole, AltingChannelInput fromConverter,
            ChannelOutput toClearOutput)
        {
            this.resetValue = resetValue;
            this.toConsole = toConsole;
            this.fromConverter = fromConverter;
            this.toClearOutput = toClearOutput;
        }

        public void run()
        {
            toConsole.write("Please input reset values\n");
            while (true)
            {
                var v = fromConverter.read();
                toClearOutput.write("\n");
                resetValue.write(v);
            }
        }
    }
}