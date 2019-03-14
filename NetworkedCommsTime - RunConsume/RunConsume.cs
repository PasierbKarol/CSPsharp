using System;
using CommsTimeTesting;
using CSPlang;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;

namespace NetworkedCommsTime___RunConsume
{
    class RunConsume
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consume started!");

            Console.WriteLine("Please enter IP address for this node.");
            var consumeNodeIP = Console.ReadLine();
            Console.WriteLine("Please enter IP address for Numbers.");
            var numbersNodeIP = Console.ReadLine();
            Console.WriteLine("Please enter IP address for Prefix.");
            var prefixNodeIP = Console.ReadLine();
            Console.WriteLine("Please enter IP address for Delta.");
            var deltaNodeIP = Console.ReadLine();
            Console.WriteLine("Please enter IP address for Successor.");
            var successorNodeIP = Console.ReadLine();

            var consumeNodeAddr = new TCPIPNodeAddress(consumeNodeIP, 3300);
            Node.getInstance().init(consumeNodeAddr);

            var network2Consume = NetChannel.net2one();
            Console.WriteLine("network2consume location = " + network2Consume.getLocation().ToString());

            Console.WriteLine("Waiting for read from the numbers... Please start other processes nowi");
            network2Consume.read();

            Console.WriteLine("Read signal from numbers");
           

            //====================== Running the test
            int nLoops = 10000;
            Console.WriteLine(nLoops + " loops ...\n");


            new CSPParallel(
                new IamCSProcess[] {
                    new Consume (nLoops, network2Consume)
                }
            ).run();
        }
    }
}
