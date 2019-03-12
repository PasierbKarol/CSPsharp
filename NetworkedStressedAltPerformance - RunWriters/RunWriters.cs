using System;
using CSPlang;
using CSPlang.Any2;
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

            var readerNodeIP = "127.0.0.1";
            var writersNodeIP = "127.0.0.2";

            var readerNodeAddr = new TCPIPNodeAddress(readerNodeIP, 3300);
            var writersNodeAddr = new TCPIPNodeAddress(writersNodeIP, 3300);
            Node.getInstance().init(writersNodeAddr);
            var writers2network = NetChannel.any2net(readerNodeAddr, 50);
            Console.WriteLine("writers2network location = " + writers2network.getLocation().ToString());

            Console.WriteLine("Sending signal to Reader...");
            writers2network.write(0); // signal from the numbers;

            Console.WriteLine("Sent signal to Reader");

            //====================== Running the test
            int nChannels = 10;
            int nWritersPerChannel = 10;
            int nMessages = 2;
            int writerID = 0;

            StressedWriterPerformance[] writers = new StressedWriterPerformance[nChannels * nWritersPerChannel];

            for (int channel = 0; channel < nChannels; channel++)
            {
                for (int i = 0; i < nWritersPerChannel; i++)
                {
                    writers[(channel * nWritersPerChannel) + i] = new StressedWriterPerformance(writers2network, channel, i, writerID);
                    writerID++;
                }
            }

            Console.WriteLine("TEST: " + nChannels + " Channels, " + nWritersPerChannel + " Writers, " + nMessages + " messages");
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
