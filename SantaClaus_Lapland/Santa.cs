using System;
using CSPlang;

namespace SantaClaus_Lapland
{
    class Santa : IamCSProcess
    {
        // vestibule channels
        ChannelOutput openForBusiness;
        ChannelOutput consultationOver;

        // reindeer channels and barrier
        ChannelInput harness;
        ChannelOutput harnessed;
        ChannelOutput returned;
        ChannelOutputList unharnessList;
        AltingBarrier stable;

        // elf channels
        ChannelInput consult;
        ChannelOutputList consulting;
        ChannelInput negotiating;
        ChannelOutputList consulted;

        int deliveryTime = 5000;
        int consultationTime = 2000;

        public Santa(ChannelOutput openForBusiness, ChannelOutput consultationOver, ChannelInput harness, ChannelOutput harnessed, ChannelOutput returned, ChannelOutputList unharnessList, AltingBarrier stable, ChannelInput consult, ChannelOutputList consulting, ChannelInput negotiating, ChannelOutputList consulted, int deliveryTime, int consultationTime)
        {
            this.openForBusiness = openForBusiness;
            this.consultationOver = consultationOver;
            this.harness = harness;
            this.harnessed = harnessed;
            this.returned = returned;
            this.unharnessList = unharnessList;
            this.stable = stable;
            this.consult = consult;
            this.consulting = consulting;
            this.negotiating = negotiating;
            this.consulted = consulted;
            this.deliveryTime = deliveryTime;
            this.consultationTime = consultationTime;
        }

        public void run()
        {
            const int REINDEER = 0;
            const int ELVES = 1;
            Random rng = new Random();
            CSTimer timer = new CSTimer();
            Alternative santaAlt = new Alternative(new Guard[] { stable, consult as Guard });
            openForBusiness.write(1);
            int index = -1;

            while (true)
            {
                index = santaAlt.priSelect();
                int[] id = new int[9]; //size of the array must be provided. For loops no greater than 9 - Karol Pasierb

                switch (index)
                {
                    case REINDEER:
                        Console.WriteLine("Santa: ho-ho-ho ... the reindeer are back");
                        for (int i = 0; i < 9; i++)
                        {
                            ;
                            id[i] = (int) harness.read();
                            Console.WriteLine("Santa: harnessing reindeer "+id[i]+" ...");
                        }

                        Console.WriteLine("Santa: mush mush ...");
                        for (int i = 0; i < 9; i++)
                        {
                            harnessed.write(1);
                        }

                        // simulate time to undertake present deliveries
                        timer.sleep(deliveryTime + rng.Next(deliveryTime));
                        Console.WriteLine("Santa: woah ... we are back home");
                        for (int i = 0; i < 9; i++)
                        {
                            returned.write(1);
                        }

                        for (int i = 0; i < 9; i++)
                        {
                            Console.WriteLine("Santa: unharnessing reindeer " + id[i] );
                            unharnessList[id[i]].write(1);
                        }

                        break;
                    case ELVES:
                        id[0] = (int) consult.read();
                        for (int i = 1; i < 3; i++)
                        {
                            id[i] = (int) consult.read();
                        }

                        Console.WriteLine("Santa: ho-ho-ho ... some elves are here!");
                        for (int i = 0; i < 3; i++)
                        {
                            consulting[id[i]].write(1);
                            Console.WriteLine("Santa: hello elf " + id[i] + " ...");
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            negotiating.read();
                        }

                        Console.WriteLine("Santa: consulting with elves ...");
                        timer.sleep(consultationTime + rng.Next(consultationTime));
                        Console.WriteLine("Santa: OK, all done - thanks!");
                        for (int i = 0; i < 3; i++)
                        {
                            consulted[id[i]].write(1);
                            Console.WriteLine("Santa: goodbye elf " + id[i] + " ...");
                        }

                        consultationOver.write(1);
                        break;
                }
            }
        }
    }
}