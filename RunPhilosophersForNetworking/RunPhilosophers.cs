using System;
using System.Collections.Generic;
using CSPlang;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;

namespace RunPhilosophersForNetworking
{
    class RunPhilosophers
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Please enter IP address for this Node.");
            var philosopherNodeIP = Console.ReadLine();
            Console.WriteLine("Please enter IP address for Canteen.");
            var canteenNodeIP = Console.ReadLine();
            

            var philosopherNodeAddr = new TCPIPNodeAddress(philosopherNodeIP, 3002);
            Node.getInstance().init(philosopherNodeAddr);

            var gotOne = NetChannel.net2any();
            Console.WriteLine("gotOne location = " + gotOne.getLocation().ToString());

            var canteenAddress = new TCPIPNodeAddress(canteenNodeIP, 3000);
            var getOne = NetChannel.any2net(canteenAddress, 51);
            Console.WriteLine("getOne location = " + getOne.getLocation().ToString());

            getOne.write(0);
            Console.WriteLine("Wrote signal to the canteen");

            List<Philosopher> philosophersList = new List<Philosopher>();
            for (int i = 0; i < 4; i++)
            {
                philosophersList.Add(new Philosopher(philosopherId: i, service: getOne, deliver: gotOne));
            }

            IamCSProcess[] network = philosophersList.ToArray();

            new CSPParallel(network).run();
        }
    }
}