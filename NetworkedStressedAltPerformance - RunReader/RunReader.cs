using System;
using CSPlang;
using CSPlang.Any2;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;
using StressedAlt_PerformanceTesting;

namespace NetworkedStressedAltPerformance___RunReader
{
    class RunReader
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Run Reader started");

            var readerNodeIP = "127.0.0.1";
            var writersNodeIP = "127.0.0.2";

            var readerNodeAddr = new TCPIPNodeAddress(writersNodeIP, 3300);
            Node.getInstance().init(readerNodeAddr);
            var network2Reader = NetChannel.net2one();
            Console.WriteLine("network2Reader location = " + network2Reader.getLocation().ToString());

            Console.WriteLine("Waiting for read from the Wrtiters...");
            var a = network2Reader.read(); // signal from the numbers;

            Console.WriteLine("Read signal from Writers");


            //====================== Running the test

            int nChannels = 100;
            int nWritersPerChannel = 200;
            int nMessages = 2;
            int writerID = 0;

           // Any2OneChannel[] any2OneChannelsNumber = Channel.any2oneArray(nChannels);

            Console.WriteLine("TEST: " + nChannels + " Channels, " + nWritersPerChannel + " Writers, " + nMessages + " messages");
            for (int i = 0; i < 10; i++)
            {
                new CSPParallel(
                    new IamCSProcess[] {
                        new NetworkedStressedReaderPerformance(Channel.getInputArray(network2Reader),nMessages, nChannels, nWritersPerChannel)
                    }
                ).run();
            }
            Console.WriteLine("Finished all");
        }
    }
}
