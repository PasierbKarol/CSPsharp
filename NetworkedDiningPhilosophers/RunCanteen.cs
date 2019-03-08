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
            Console.WriteLine("cooked location = " + cooked.getLocation().ToString());

            var getOne = NetChannel.net2one();
            Console.WriteLine("getOne location = " + getOne.getLocation().ToString());

            Console.WriteLine("Waiting for read from the philosophers...");
            var a = getOne.read(); // signal from the philosophers;
            Console.WriteLine("Read signal from philosophers.\nCreating philosophers channel.");
            var philosopherAddr = new TCPIPNodeAddress(philosopherNodeIP, 3002);
            Console.WriteLine("Philosophers Address in canteen created.");
            var gotOne = NetChannel.one2net(philosopherAddr, 50);
            Console.WriteLine("Philosophers channel in canteen created.");


            IamCSProcess[] processList =
            {
                new ClockedQueuingServery(service: getOne, deliver: gotOne, supply: cooked)
            };
            new CSPParallel(processList).run();
        }
    }
}