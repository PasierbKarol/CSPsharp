using System;
using CSPlang;
using CSPlang.Any2;
using CSPnet2;
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

            Console.WriteLine("Please enter number of writers to be created");
            int nChannels = Int32.Parse(Console.ReadLine());
            int nWritersPerChannel = 10;
            int nMessages = 2;

            Console.WriteLine("Please enter IP address for the reader.");
            var readerNodeIP = Console.ReadLine();


            var readerNodeAddr = new TCPIPNodeAddress(readerNodeIP, 3300);
            Node.getInstance().init(readerNodeAddr);

            NetAltingChannelInput[] network2Reader = new NetAltingChannelInput[nChannels];
            for (int i = 0; i < nChannels; i++)
            {
                network2Reader[i] = NetChannel.net2one();
                Console.WriteLine("network2Reader location = " + network2Reader[i].getLocation().ToString());
            }
            
            string[] writersNodesIPs = new string[nChannels];
            Console.WriteLine("Wait for writers to confirm they are ready and send their IPs");
            for (int i = 0; i < nChannels; i++)
            {
                writersNodesIPs[i] =(string) network2Reader[i].read();
            }

            NetChannelOutput[] reader2allWriters = new NetChannelOutput[nChannels];
            for (int i = 0; i < writersNodesIPs.Length; i++)
            {
                var writersChannelNodeAddress = new TCPIPNodeAddress(writersNodesIPs[i], 3300);
                reader2allWriters[i] = NetChannel.one2net(writersChannelNodeAddress, 50 ); //It's going to be always first read channel in the writer
            }

            Console.WriteLine("Writing to channels to notify them they are ready to start");
            for (int i = 0; i < nChannels; i++)
            {
                reader2allWriters[i].write(0);
            }
            Console.WriteLine("Sent signal to Writers");


            //====================== Running the test

            //Console.WriteLine("TEST: " + nChannels + " Channels, " + nWritersPerChannel + " Writers, " + nMessages + " messages");
            for (int i = 0; i < 10; i++)
            {
                new CSPParallel(
                    new IamCSProcess[] {
                        new NetworkedStressedReaderPerformance(network2Reader,nMessages, nChannels, nWritersPerChannel)
                    }
                ).run();
            }
            Console.WriteLine("Finished all");
            Console.ReadKey();
        }
    }
}
