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

            var numbersNodeIP = "127.0.0.1";
            var consumeNodeIP = "127.0.0.2";
            var prefixNodeIP = "127.0.0.3";
            var deltaNodeIP = "127.0.0.4";
            var successorNodeIP = "127.0.0.5";

            var consumeNodeAddr = new TCPIPNodeAddress(consumeNodeIP, 3300);
            Node.getInstance().init(consumeNodeAddr);

            var network2Consume = NetChannel.net2one();
            Console.WriteLine("network2consume location = " + network2Consume.getLocation().ToString());

            Console.WriteLine("Waiting for read from the numbers... Please start other processes now");
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
