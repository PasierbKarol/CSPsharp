using System;
using CSPlang;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;
using PlugAndPlay;

namespace NetworkedCommsTime___RunPrefix
{
    class RunPrefix
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Prefix Starts!");
            //It is the first process to run after the Consume

            Console.WriteLine("Please enter IP address for this node.");
            var prefixNodeIP = Console.ReadLine();
            Console.WriteLine("Please enter IP address for Delta.");
            var deltaNodeIP = Console.ReadLine();


            var prefixNodeAddr = new TCPIPNodeAddress(prefixNodeIP, 3000);
            Node.getInstance().init(prefixNodeAddr);
            var delta2prefix = NetChannel.net2one();

            var successor2prefix = NetChannel.net2one();

            Console.WriteLine("Please start Delta process now");
            delta2prefix.read(); //wait for the read from the delta to know that it exists            

            Console.WriteLine("Read message from Delta");

            var deltaNodeAddr = new TCPIPNodeAddress(deltaNodeIP, 3000);
            var prefix2delta = NetChannel.one2net(deltaNodeAddr, 50);
            Console.WriteLine("prefix2delta location = " + prefix2delta.getLocation().ToString());

            successor2prefix.read(); //waiting for read from successor to now it is running
            prefix2delta.write(0); //notify delta sucessor is running

            Console.WriteLine("\nNetwork is good to run. Please refer to Consume process window");

            new CSPParallel(
                new IamCSProcess[] {
                    new Prefix (0, successor2prefix, prefix2delta)                    
                }
            ).run();
        }
    }
}
