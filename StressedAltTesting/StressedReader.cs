//////////////////////////////////////////////////////////////////////
//                                                                  //
//  jcspDemos Demonstrations of the JCSP ("CSP for Java") Library   //
//  Copyright (C) 1996-2018 Peter Welch, Paul Austin and Neil Brown //
//                2001-2004 Quickstone Technologies Limited         //
//                2005-2018 Kevin Chalmers                          //
//                                                                  //
//  You may use this work under the terms of either                 //
//  You may use this work under the terms of either                 //
//  1. The Apache License, Version 2.0                              //
//  2. or (at your option), the GNU Lesser General Public License,  //
//       version 2.1 or greater.                                    //
//                                                                  //
//  Full licence texts are included in the LICENCE file with        //
//  this library.                                                   //
//                                                                  //
//  Author contacts: P.H.Welch@kent.ac.uk K.Chalmers@napier.ac.uk   //
//                                                                  //
//////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using CSPlang;

/**
 * @author P.H. Welch
 */
namespace TestingUtilities
{
    public class StressedReader : IamCSProcess
    {
        private readonly AltingChannelInput[] c;
        private readonly int nWritersPerChannel;

        public StressedReader(AltingChannelInput[] c, int nWritersPerChannel)
        {
            this.c = c;
            this.nWritersPerChannel = nWritersPerChannel;
        }

        public void run()
        {
            const int seconds = 1000;
            const int initialWait = 5;
            //Console.WriteLine("\nWait (" + initialWait +
            //                    " seconds) for all the writers to get going ...");
            //CSTimer tim = new CSTimer();
            //long timeout = tim.read() + (initialWait * seconds);
            //tim.after(timeout);
            //Console.WriteLine("OK - that should be long enough ...\n");
            int[][] n = new int[c.Length][];
            for (int i = 0; i < n.Length; i++)
            {
                n[i] = new int[nWritersPerChannel];
            }

            //for (int channel = 0; channel < c.Length; channel++)
            //{
            //    for (int i = 0; i < nWritersPerChannel; i++)
            //    {
            //        StressedPacket stressedPacket = (StressedPacket)c[channel].read();
            //        n[channel][stressedPacket.writer] = stressedPacket.n;
            //        for (int chan = 0; chan < channel; chan++) Console.Write("  ");
            //        Console.WriteLine("channel " + channel +
            //                            " writer " + stressedPacket.writer +
            //                            " read " + stressedPacket.n);
            //    }
            //}
            Alternative alt = new Alternative(c);
            int counter = 0, tock = 0;
            while (true)
            {
                if (counter == 0)
                {
                    Console.Write("Tock " + tock + " : ");
                    int total = 0;
                    for (int channel = 0; channel < n.Length; channel++)
                    {
                        Console.Write(n[channel][tock % nWritersPerChannel] + " ");
                        for (int i = 0; i < nWritersPerChannel; i++)
                        {
                            total += n[channel][i];
                        }
                    }
                    Console.WriteLine(": " + total);
                    tock++;
                    counter = 10000;
                }
                counter--;
                int channelFairSelect = alt.fairSelect();
                StressedPacket stressedPacket = (StressedPacket)c[channelFairSelect].read();
                n[channelFairSelect][stressedPacket.writer] = stressedPacket.n;
                // for (int chan = 0; chan < channel; chan++) System.out.print ("  ");
                // Console.WriteLine ("channel " + channel +
                //                     " writer " + stressedPacket.writer +
                //                     " read " + stressedPacket.n);
            }
        }

    }
}
