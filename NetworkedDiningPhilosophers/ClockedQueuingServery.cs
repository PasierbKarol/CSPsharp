using System;
using CSPlang;
using CSPlang.Any2;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;
using PlugAndPlay;


namespace NetworkedDiningPhilosophers
{
    // copyright 2012-13 Jon Kerridge
    // Let's Do It In Parallel


    class ClockedQueuingServery : IamCSProcess
    {
        ChannelInput service;
        ChannelOutput deliver;
        ChannelInput supply;

        public ClockedQueuingServery(ChannelInput service, ChannelOutput deliver, ChannelInput supply)
        {
            this.service = service;
            this.deliver = deliver;
            this.supply = supply;
        }

        public void run()
        {
            Any2OneChannel console = Channel.any2one();

            Clock clock = new Clock(toConsole: console.Out());

            QueuingCanteen servery = new QueuingCanteen(service: service,
                deliver: deliver,
                supply: supply,
                toConsole: console.Out());
            GConsole serveryConsole = new GConsole(toConsole: console.In(),
                frameLabel: "Clocked Queuing Servery");
            IamCSProcess[] network = {servery, serveryConsole, clock};
            new CSPParallel(network).run();
        }
    }
}