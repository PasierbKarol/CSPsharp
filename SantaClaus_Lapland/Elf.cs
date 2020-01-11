using System;
using CSPlang;

namespace SantaClaus_Lapland
{
    class Elf : IamCSProcess
    {
        int number;

        Bucket[] groups;

        // vestibule connections
        ChannelOutput needToConsult;
        ChannelInput joinGroup;

        // santa connections
        ChannelOutput consult;
        ChannelInput consulting;
        ChannelOutput negotiating;
        ChannelInput consulted;

        int workingTime = 3000;

        public Elf(int number, Bucket[] groups, ChannelOutput needToConsult, ChannelInput joinGroup,
            ChannelOutput consult, ChannelInput consulting, ChannelOutput negotiating, ChannelInput consulted,
            int workingTime)
        {
            this.number = number;
            this.groups = groups;
            this.needToConsult = needToConsult;
            this.joinGroup = joinGroup;
            this.consult = consult;
            this.consulting = consulting;
            this.negotiating = negotiating;
            this.consulted = consulted;
            this.workingTime = workingTime;
        }

        public void run()
        {
            Random rng = new Random();
            CSTimer timer = new CSTimer();
            while (true)
            {
                Console.WriteLine("\tElf " + number + ": working, :)");
                timer.sleep(workingTime + rng.Next(workingTime));
                needToConsult.write(number);
                int group = (int)joinGroup.read();
                groups[group].fallInto();
                // process will wait until flushed by vestibule
                // it is guaranteed by vestibule that three Elf proceses will be started
                consult.write(number);
                Console.WriteLine("\tElf " + number + ": need to consult Santa, :(");
                consulting.read();
                Console.WriteLine("\tElf " + number + ": about these toys ... ???");
                negotiating.write(1);
                consulted.read();
                Console.WriteLine("\tElf " + number + ": OK ... we will build it, bye, :(");
            }
        }
    }
}