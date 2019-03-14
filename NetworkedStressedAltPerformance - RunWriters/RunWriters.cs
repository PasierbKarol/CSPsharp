using System;
using CSPlang;
using CSPnet2;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;
using StressedAlt_PerformanceTesting;

namespace NetworkedStressedAltPerformance___RunWriters
{
    class RunWriters
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Run Writers started.");

            int nChannels = 1;
            int nWritersPerChannel = 10;
            int nMessages = 2;
            Console.WriteLine("Please enter writer ID");
            int writerID = Int32.Parse(Console.ReadLine());


            Console.WriteLine("Please enter IP address for this node.");
            var writersChannelNodeIP = Console.ReadLine();
            Console.WriteLine("Please enter IP address for Reader.");
            var readerNodeIP = Console.ReadLine();


            var readerNodeAddr = new TCPIPNodeAddress(readerNodeIP, 3300);
            var fromReader = NetChannel.net2one();

            var writersChannelNodesAddresses = new TCPIPNodeAddress(writersChannelNodeIP, 3300);
            Node.getInstance().init(writersChannelNodesAddresses);

            var readersChannelNumberForThisWriter = Int32.Parse("5" + (writerID - 1));
            var writers2network = NetChannel.any2net(readerNodeAddr, readersChannelNumberForThisWriter );

            Console.WriteLine("writers2network location = " + writers2network.getLocation().ToString());

            Console.WriteLine("Informing reader that Writer" + writerID + " is ready and sending its IP");
            writers2network.write(GetLocalIPAddress.ConvertLocalIPAddressToString());
            //writers2network.write(writersChannelNodeIP);

            CSTimer timer = new CSTimer();
            timer.sleep(2000);
            Console.WriteLine("Waiting for the signal from Reader...");
            fromReader.read();
            
            Console.WriteLine("Read signal from Reader");

            //====================== Running the test
            

            StressedWriterPerformance[] writers = new StressedWriterPerformance[nChannels * nWritersPerChannel];

            for (int channel = 0; channel < nChannels; channel++)
            {
                for (int i = 0; i < nWritersPerChannel; i++)
                {
                    writers[(channel * nWritersPerChannel) + i] = new StressedWriterPerformance(writers2network, channel, i, writerID);
                    writerID++;
                }
            }

            //Console.WriteLine("TEST: " + nChannels + " Channels, " + nWritersPerChannel + " Writers, " + nMessages + " messages");
            for (int i = 0; i < 10; i++)
            {
                new CSPParallel(
                    new IamCSProcess[] {
                        new CSPParallel (writers),                        
                    }
                ).run();
            }
            Console.WriteLine("Finished all");

        }
    }
}
