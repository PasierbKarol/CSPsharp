using System;
using CSPlang;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;
using PlugAndPlay;
using PlugAndPlay.Ints;

namespace NetworkedCommsTime___RunNumbers
{
    class RunNumbers
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Numbers started!");

            var numbersNodeIP = "127.0.0.1";
            var consumeNodeIP = "127.0.0.2";
            var prefixNodeIP = "127.0.0.3";
            var deltaNodeIP = "127.0.0.4";
            var successorNodeIP = "127.0.0.5";

            var numbersNodeAddr = new TCPIPNodeAddress(numbersNodeIP, 3000);
            Node.getInstance().init(numbersNodeAddr);
            var consumeNodeAddr = new TCPIPNodeAddress(consumeNodeIP, 3300);
            var numbers2network = NetChannel.one2net(consumeNodeAddr, 50);
            Console.WriteLine("network2consume location = " + numbers2network.getLocation().ToString());

            Console.WriteLine("Sending signal to Consume...");
            numbers2network.write(0); // signal from the numbers;

            Console.WriteLine("Sent signal to Consume");

            //====================== Running the test

            One2OneChannel P2D = Channel.one2one();
            One2OneChannel D2S = Channel.one2one();
            One2OneChannel S2P = Channel.one2one();

            new CSPParallel(
                new IamCSProcess[] {
                    new Prefix (0, S2P.In(), P2D.Out()),
                    new Delta2 (P2D.In(),numbers2network, D2S.Out()),
                    new Successor (D2S.In(), S2P.Out()),
                }
            ).run();
        }
    }
}
