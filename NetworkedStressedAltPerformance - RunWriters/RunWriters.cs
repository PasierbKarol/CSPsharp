using System;
using CSPlang;
using CSPlang.Any2;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;
using CSPnet2.TCPIP;
using CSPutil;
using StressedAlt_PerformanceTesting;

namespace NetworkedStressedAltPerformance___RunWriters
{
    class RunWriters
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Run Writers started.");
            Console.WriteLine("Please enter writer ID");

            int nChannels = 1;
            int nWritersPerChannel = 10;
            int nMessages = 2;
            int writerID = Int32.Parse(Console.ReadLine());


            var readerNodeIP = "127.0.0.10";
            var writersChannelNodeIP  = "127.0.0." + writerID;


            var readerNodeAddr = new TCPIPNodeAddress(readerNodeIP, 3300);
            var fromReader = NetChannel.net2one();

            var writersChannelNodesAddresses = new TCPIPNodeAddress(writersChannelNodeIP, 3300);
            Node.getInstance().init(writersChannelNodesAddresses);

            var readersChannelNumberForThisWriter = Int32.Parse("5" + (writerID - 1));
            var writers2network = NetChannel.any2net(readerNodeAddr, readersChannelNumberForThisWriter );

            Console.WriteLine("writers2network location = " + writers2network.getLocation().ToString());

            Console.WriteLine("Informing reader that Writer" + writerID + " is ready");
            writers2network.write(0);

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
