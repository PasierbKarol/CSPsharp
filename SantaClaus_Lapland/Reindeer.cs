using System;
using CSPlang;

namespace SantaClaus_Lapland
{
    class Reindeer : IamCSProcess
    {
        int number = -1;

        AltingBarrier stable;

        ChannelOutput harness;
        ChannelInput harnessed;
        ChannelInput returned;
        ChannelInput unharness;

        int holidayTime = 8000;

        public Reindeer(int number, AltingBarrier stable, ChannelOutput harness, ChannelInput harnessed, ChannelInput returned, ChannelInput unharness, int holidayTime)
        {
            this.number = number;
            this.stable = stable;
            this.harness = harness;
            this.harnessed = harnessed;
            this.returned = returned;
            this.unharness = unharness;
            this.holidayTime = holidayTime;
        }

        public void run()
        {
            Random rng = new Random();
            CSTimer timer = new CSTimer();
            while (true)
            {
                Console.WriteLine("\t\tReindeer" + number + ": on holiday ... wish you were here, :)");
                timer.sleep(holidayTime + rng.Next(holidayTime));
                Console.WriteLine("\t\tReindeer " + number + ": back from holiday ... ready for work, :(");
                stable.sync();
                harness.write(number);
                harnessed.read();
                Console.WriteLine("\t\tReindeer " + number + ": delivering toys ... la-di-da-di-da-di-da, :)");
                returned.read();
                Console.WriteLine("\t\tReindeer " + number + ": all toys delivered ... want a holiday, :(");
                unharness.read();
            }
        }
    }
}