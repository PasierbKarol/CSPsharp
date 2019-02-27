using System;
using CSPlang;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;


namespace NetworkedDiningPhilosophers
{
    class RunCanteen
    {
        static void Main(string[] args)
        {
            var chefNodeIP = "127.0.0.1";
            var canteenNodeIP = "127.0.0.2";
            var philosopherNodeIP = "127.0.0.3";

            var canteenNodeAddr = new TCPIPNodeAddress(canteenNodeIP, 3000);
            Node.getInstance().init(canteenNodeAddr);
            var cooked = NetChannel.net2one();
            Console.WriteLine("cooked location = " + cooked.GetLocationAsString());

            var getOne = NetChannel.net2one();
            Console.WriteLine("getOne location = " + getOne.GetLocationAsString());

            getOne.read(); // signal from the philosophers;
            var philosopherAddr = new TCPIPNodeAddress(philosopherNodeIP, 3002);
            var gotOne = NetChannel.one2net(philosopherAddr, 50);

            IamCSProcess[] processList =
            {
                new ClockedQueuingServery(service: getOne, deliver: gotOne, supply: cooked)
            };
            new CSPParallel(processList).run();
        }
    }
}