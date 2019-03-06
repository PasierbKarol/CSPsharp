// copyright 2012-13 Jon Kerridge
// Let's Do It In Parallel

using CSPlang;
using CSPnet2.NetChannels;
using PlugAndPlay;

namespace RunChefForPhilosophersNetworking
{
    class Kitchen : IamCSProcess
    {
        NetChannelOutput supply;

        public Kitchen(NetChannelOutput supply)
        {
            this.supply = supply;
        }
        public void run()
        {
            One2OneChannel console = Channel.one2one();
            Chef chef = new Chef(supply: supply,
                toConsole: console.Out());
            GConsole chefConsole = new GConsole(toConsole: console.In(),
                frameLabel: "Chef");
            IamCSProcess[] network = {chef, chefConsole};
            new CSPParallel(network).run();
        }
    }
}