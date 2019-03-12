using System;
using CSPlang;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;
using PlugAndPlay;

namespace NetworkedCommsTime___RunDelta
{
    class RunDelta
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Delta Starts!");
            //This process starts after Prefix

            var consumeNodeIP = "127.0.0.2";
            var prefixNodeIP = "127.0.0.3";
            var deltaNodeIP = "127.0.0.4";
            var successorNodeIP = "127.0.0.5";


            var deltaNodeAddr = new TCPIPNodeAddress(deltaNodeIP, 3000);
            Node.getInstance().init(deltaNodeAddr);
            var prefix2delta = NetChannel.net2one();

            //we need to notify prefix that delta exists before it can connect to it
            var prefixNodeAddr = new TCPIPNodeAddress(prefixNodeIP, 3000);
            var delta2prefix = NetChannel.one2net(prefixNodeAddr, 50);
            delta2prefix.write(0); //notify prefix delta is ready
           


            var consumeNodeAddr = new TCPIPNodeAddress(consumeNodeIP, 3300);
            var delta2consume = NetChannel.one2net(consumeNodeAddr, 50);

            //first write to the consume to make sure network works
            delta2consume.write(0);


            //Please start successor process now
            Console.WriteLine("Please start successor process now");
            prefix2delta.read();//when the signal from prefix comes back it means successor is already running

            var successorNodeAddr = new TCPIPNodeAddress(successorNodeIP, 3000);
            var delta2successor = NetChannel.one2net(successorNodeAddr, 50);

            Console.WriteLine("delta2consume location = " + delta2consume.getLocation().ToString());
            Console.WriteLine("delta2successor location = " + delta2successor.getLocation().ToString());

            Console.WriteLine("\nNetwork is good to run. Please refer to Consume process window");


            new CSPParallel(
                new IamCSProcess[] {
                    new Delta2 (prefix2delta ,delta2consume, delta2successor)                }
            ).run();
        }
    }
}
