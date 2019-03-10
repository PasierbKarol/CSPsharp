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
            var chefNodeIP = "127.0.0.1";
            var canteenNodeIP = "127.0.0.2";
            var philosopherNodeIP = "127.0.0.3";

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
