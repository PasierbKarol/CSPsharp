// copyright 2012-13 Jon Kerridge
// Let's Do It In Parallel

using CSPlang;
using PlugAndPlay;

namespace Dining_Philosophers
{
    class Philosopher : IamCSProcess
    {
        ChannelOutput service;
        ChannelInput deliver;
        int philosopherId;

        public Philosopher(ChannelOutput service, ChannelInput deliver, int philosopherId)
        {
            this.service = service;
            this.deliver = deliver;
            this.philosopherId = philosopherId;
        }

        public void run()
        {
            One2OneChannel console = Channel.one2one();
            PhilosopherBehaviour philosopher = new PhilosopherBehaviour(service: service,
                deliver: deliver,
                toConsole: console.Out(),
                id: philosopherId);

            GConsole philosopherConsole = new GConsole(toConsole: console.In(),
                frameLabel: "Philosopher " + philosopherId);
            IamCSProcess[] network = { philosopher, philosopherConsole };
            new CSPParallel(network).run();
        }
    }
}