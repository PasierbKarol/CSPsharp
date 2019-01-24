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
namespace TestingUtilities
{

    class StressedAlt
    {
        public static String TITLE = "Stressed Alt";
        public static String DESCR =
            "Shows the fairness of an Alt at a high level of stress from the writing channels. Many writers " +
            "will be writing to each of the channels (each is an Any-One channel) with no delay between writes. " +
            "The ALT will be well behaved under such stress, still exhibiting fairness and no loss of data.\n" +
            "\n" +
            "Every 10000 cycles the reader will display the values read from each of the channels. If the Alt " +
            "is serving the channels fairly the numbers will all be increasing together (though maybe wrapping around " +
            "when the 2^31 limit for positive integers is reached). If the Alt is not serving them fairly then " +
            "there will be an imbalance in the rate of increase between the channels.";

        public static void Main(String[] args)
        {
            int nChannels = 8;
            int nWritersPerChannel = 8;

            //Any2OneChannel[] c = Channel.any2oneArray (nChannels, new OverWriteOldestBuffer (1));
            Any2OneChannel[] c = Channel.any2oneArray(nChannels);

            StressedWriter[] writers = new StressedWriter[nChannels * nWritersPerChannel];

            for (int channel = 0; channel < nChannels; channel++)
            {
                for (int i = 0; i < nWritersPerChannel; i++)
                {
                    writers[(channel * nWritersPerChannel) + i] = new StressedWriter(c[channel].Out(), channel, i);
                }
            }

            new CSPParallel(
                new IamCSProcess[] {
                    new CSPParallel (writers),
                    new StressedReader (Channel.getInputArray(c), nWritersPerChannel)
                }
            ).run();

        }
    }
}



