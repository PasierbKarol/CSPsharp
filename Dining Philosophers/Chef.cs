// copyright 2012-13 Jon Kerridge
// Let's Do It In Parallel

using CSPlang;

namespace Dining_Philosophers
{
    class Chef : IamCSProcess
    {
        ChannelOutput supply;
        ChannelOutput toConsole;

        public Chef(ChannelOutput supply, ChannelOutput toConsole)
        {
            this.supply = supply;
            this.toConsole = toConsole;
        }

        public void run()
        {
            CSTimer tim = new CSTimer();
            int CHICKENS = 4;

            toConsole.write("Starting ... \n");
            while (true)
            {
                toConsole.write("Cooking ... \n"); // cook 4 chickens;
                tim.after(tim.read() + 2000); // this takes 2 seconds to cook;
                toConsole.write(CHICKENS + "chickens ready ... \n");
                supply.write(CHICKENS);
                toConsole.write("Taking chickens to Canteen ... \n");
                supply.write(0);
            }
        }
    }
}