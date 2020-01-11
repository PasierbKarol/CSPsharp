// copyright 2012-13 Jon Kerridge
// Let's Do It In Parallel

using CSPlang;

namespace Dining_Philosophers
{
    class PhilosopherBehaviour : IamCSProcess
    {
        int id = -1;
        ChannelOutput service;
        ChannelInput deliver;
        ChannelOutput toConsole;

        public PhilosopherBehaviour(ChannelOutput service, ChannelInput deliver, ChannelOutput toConsole, int id)
        {
            this.service = service;
            this.deliver = deliver;
            this.toConsole = toConsole;
            this.id = id;
        }

        public void run()
        {
            CSTimer tim = new CSTimer();
            toConsole.write("Starting ... \n");
            while (true)
            {
                toConsole.write("Thinking ... \n");
                if (id > 0)
                {
                    tim.sleep(3000);
                }
                else
                {
                    // Philosopher 0, has a 0.1 second think
                    tim.sleep(100);
                }

                toConsole.write("Need a chicken ...\n");
                service.write(id);
                int gotOne = (int)deliver.read();
                if (gotOne > 0)
                {
                    toConsole.write("Eating ... \n");
                    tim.sleep(2000);
                    toConsole.write("Brrrp ... \n");
                }
                else
                {
                    toConsole.write("                   Oh dear No chickens left \n");
                }
            }
        }
    }
}