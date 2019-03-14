using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using CSPlang;
using CSPutil;
using TestingUtilities;

namespace StressedAlt_PerformanceTesting
{
    public class StressedReaderPerformance : IamCSProcess
    {
        private readonly AltingChannelInput[] c;
        private readonly int nWritersPerChannel;
        private readonly int nChannels;
        private readonly int nMessages;

        public StressedReaderPerformance(AltingChannelInput[] c, int nMessages, int nChannels, int nWritersPerChannel)
        {
            this.c = c;
            this.nMessages = nMessages;
            this.nChannels = nChannels;
            this.nWritersPerChannel = nWritersPerChannel;
        }

        public void run()
        {
            //initialize internal arrays of 2-dimensional array
            int[][] n = new int[c.Length][];
            for (int i = 0; i < n.Length; i++)
            {
                n[i] = new int[nWritersPerChannel];
            }

            //setup variables
            const int initialWait = 3000;
            int iterations = nMessages * nChannels * nWritersPerChannel;
            long t0 = 0, t1 = 0, microseconds = 0;
            Alternative alt = new Alternative(c);
            CSTimer timer = new CSTimer();
            int channelFairSelect = 0;
            StressedPacket stressedPacket;
            var csv = new StringBuilder();


            //set the timer to wait for 5 seconds to make sure that all the readers are idle in writing state
            Console.WriteLine("Waiting 3 seconds...");
            timer.after(initialWait + timer.read());
            Console.WriteLine("Waiting is over. Measuring time");


            

            //perform read and measure the time
            t0 = CSTimer.CurrentTimeMillis();
            for (int i = 0; i < iterations; i++)
            {
                channelFairSelect = alt.fairSelect();
                stressedPacket = (StressedPacket) c[channelFairSelect].read();
                n[channelFairSelect][stressedPacket.writer] = stressedPacket.n;
            }
            t1 = CSTimer.CurrentTimeMillis();
            microseconds = (t1 - t0) * 1000;
            if (microseconds > 0)
            {
                Console.WriteLine("Reading time for " + iterations + " iterations: " + microseconds);
                var newLine = string.Format("{0},{1},{2},{3},{4}", nChannels, nWritersPerChannel, nMessages, iterations, microseconds);
                csv.AppendLine(newLine);
                File.AppendAllText(@"d:\\stressedAlt_Test"+nChannels+"x"+nWritersPerChannel+".csv", csv.ToString());
            }         
        }
    }
}
