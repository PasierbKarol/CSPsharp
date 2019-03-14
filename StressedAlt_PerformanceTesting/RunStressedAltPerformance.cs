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
/// 
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CSPlang;
using CSPlang.Any2;

/**
 * @author P.H. Welch
 */
namespace StressedAlt_PerformanceTesting
{

    public class RunStressedAltPerformance
    {

        public static void Main(String[] args)
        {
            int nChannels = 2;
            int nWritersPerChannel = 10;
            int nMessages = 2;
            int writerID = 0;

            Any2OneChannel[] any2OneChannelsNumber = Channel.any2oneArray(nChannels);

            StressedWriterPerformance[] writers = new StressedWriterPerformance[nChannels * nWritersPerChannel];

            for (int channel = 0; channel < nChannels; channel++)
            {
                for (int i = 0; i < nWritersPerChannel; i++)
                {
                    writers[(channel * nWritersPerChannel) + i] = new StressedWriterPerformance(any2OneChannelsNumber[channel].Out(), channel, i, writerID);
                    writerID++;
                }
            }

            Console.WriteLine("TEST: " + nChannels + " Channels, " + nWritersPerChannel + " Writers, " + nMessages + " messages");
            for (int i = 0; i < 10; i++)
            {
                new CSPParallel(
                    new IamCSProcess[] {
                        new CSPParallel (writers),
                        new StressedReaderPerformance(Channel.getInputArray(any2OneChannelsNumber),nMessages, nChannels, nWritersPerChannel)
                    }
                ).run();
            }
            Console.WriteLine("Finished all");
            Console.ReadKey();




        }
    }
}



