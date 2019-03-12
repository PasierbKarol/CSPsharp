using System;
using CSPlang;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;
using PlugAndPlay;

namespace NetworkedCommsTime___RunSuccessor
{
    class RunSuccessor
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Successor Starts!");
            //This Process runs after Delta

            var prefixNodeIP = "127.0.0.3";
            var successorNodeIP = "127.0.0.5";

            var successorNodeAddr = new TCPIPNodeAddress(successorNodeIP, 3000);
            Node.getInstance().init(successorNodeAddr);
            var delta2sucessor = NetChannel.net2one();

            var prefixNodeAddr = new TCPIPNodeAddress(prefixNodeIP, 3000);
            var successor2prefix = NetChannel.one2net(prefixNodeAddr, 51); //channel number 50 is already taken for delta2prefix
            Console.WriteLine("successor2prefix location = " + successor2prefix.getLocation().ToString());

            //write to prefix successor is running so it can notify delta about it
            successor2prefix.write(0);

            Console.WriteLine("Network is good to run. Please refer to Consume process window");
            new CSPParallel(
                new IamCSProcess[] {
                    new Successor (delta2sucessor, successor2prefix)
                }
            ).run();
        }
    }
}
