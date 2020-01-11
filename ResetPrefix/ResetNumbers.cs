using CSPlang;
using PlugAndPlay;

namespace ResetPrefix
{
    class ResetNumbers : IamCSProcess
    {
        private ChannelOutput outChannel;
        private ChannelInput resetChannel;
        private int initialValue = 0;

        public ResetNumbers(AltingChannelInput resetChannel, int initialValue, ChannelOutput outChannel)
        {
            this.resetChannel = resetChannel;
            this.initialValue = initialValue;
            this.outChannel = outChannel;
        }

        public void run()
        {
            One2OneChannel a = Channel.one2one();
            One2OneChannel b = Channel.one2one();
            One2OneChannel c = Channel.one2one();

            IamCSProcess[] testList =
            {
                new ResetPrefix(prefixValue: initialValue,
                    outChannel: a.Out(),
                    inChannel: c.In(),
                    resetChannel: resetChannel),

                //Used plugNplay Delta2 instead of creating GCopy
                new Delta2(In: a.In(),
                    Out1: outChannel,
                    Out2: b.Out()),

                new Successor(In: b.In(),
                    Out: c.Out())
            };
            new CSPParallel(testList).run();
        }
    }
}