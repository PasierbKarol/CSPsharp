// copyright 2012-13 Jon Kerridge
// Let's Do It In Parallel

using CSPlang;
using PlugAndPlay;

namespace Dining_Philosophers
{
    public class QueuingServery : IamCSProcess
    {
        ChannelInput service;
        ChannelOutput deliver;
        ChannelInput supply;

        public QueuingServery(ChannelInput service, ChannelOutput deliver, ChannelInput supply)
        {
            this.service = service;
            this.deliver = deliver;
            this.supply = supply;
        }

        public void run()
        {
            One2OneChannel console = Channel.one2one();
            QueuingCanteen servery = new QueuingCanteen(service: service,
                deliver: deliver,
                supply: supply,
                toConsole: console.Out());
            GConsole serveryConsole = new GConsole(toConsole: console.In(),
                frameLabel: "Queuing Servery");

            IamCSProcess[] network = { servery, serveryConsole };
            new CSPParallel(network).run();
        }
    }
}