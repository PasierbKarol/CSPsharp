using System;
using CSPlang;
using CSPlang.Any2;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;


namespace NetworkedDiningPhilosophers
{
    // copyright 2012-13 Jon Kerridge
    // Let's Do It In Parallel


    class Clock : IamCSProcess
    {
        ChannelOutput toConsole;


        public Clock(ChannelOutput toConsole)
        {
            this.toConsole = toConsole;
        }
        public void run()
        {
            CSTimer tim = new CSTimer();
            int tick = 0;

            while (true)
            {
                toConsole.write("Time: " + tick + " \n");
                tim.sleep(1000);
                tick = tick + 1;
            }
        }
    }
}