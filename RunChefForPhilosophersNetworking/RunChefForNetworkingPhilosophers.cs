using System;
using CSPlang;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;

namespace RunChefForPhilosophersNetworking
{
    class RunChefForNetworkingPhilosophers
    {
        static void Main(string[] args)
        {            
            Console.WriteLine("Please enter IP address for this Node.");
            var chefNodeIP = Console.ReadLine();
            Console.WriteLine("Please enter IP address for Canteen.");
            var canteenNodeIP = Console.ReadLine();

            var chefNodeAddr = new TCPIPNodeAddress(chefNodeIP, 3003);
            Node.getInstance().init(chefNodeAddr);

            var canteenAddress = new TCPIPNodeAddress(canteenNodeIP, 3000);
            var cooked = NetChannel.one2net(canteenAddress, 50);
            Console.WriteLine("cooked location = " + cooked.getLocation());

            IamCSProcess[] processList = {new Kitchen(supply: cooked)};
            new CSPParallel(processList).run();
        }
    }
}
