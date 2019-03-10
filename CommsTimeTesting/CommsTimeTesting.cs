//////////////////////////////////////////////////////////////////////
//                                                                  //
//  jcspDemos Demonstrations of the JCSP ("CSP for Java") Library   //
//  Copyright (C) 1996-2018 Peter Welch, Paul Austin and Neil Brown //
//                2001-2004 Quickstone Technologies Limited         //
//                2005-2018 Kevin Chalmers                          //
//                                                                  //
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
using CSPlang;
using PlugAndPlay;
using PlugAndPlay.Ints;

/**
 * @author P.H. Welch
 */
namespace CommsTimeTesting
{
    public class CommsTimeTesting
    {
        /*public static String TITLE = "CommsTime";
        public static String DESCR =
        "Test of communication speed between JCSP processes. Based on OCCAM CommsTime.occ by Peter Welch, " +
        "University of Kent at Canterbury. Ported into Java by Oyvind Teig. Now using the JCSP library.\n" +
        "\n" +
        "A small network of four processes is created which will generate a sequence of numbers, measuring " +
        "the time taken to generate each 10000. This time is then divided to calculate the time per iteration, " +
        "the time per communication (one integer over a one-one channel) and the time for a context switch. " +
        "There are four communications per iteration and two context switches per communication. This test " +
        "forms a benchmark for the for the overheads involved.\n" +
        "\n" +
        "This version uses a PARallel delta2 component, so includes the starting and finishing of one extra" +
        "process per loop.";*/

        public static void Main(string[] args)
        {
            int nLoops = 10000;
            Console.WriteLine(nLoops + " loops ...\n");

            One2OneChannel P2D = Channel.one2one();
            One2OneChannel D2S = Channel.one2one();
            One2OneChannel S2P = Channel.one2one();
            One2OneChannel D2C = Channel.one2one();

            new CSPParallel(
                new IamCSProcess[] {
                    new Prefix (0, S2P.In(), P2D.Out()),
                    new Delta2 (P2D.In(), D2C.Out(), D2S.Out()),
                    new Successor (D2S.In(), S2P.Out()),
                    new Consume (nLoops, D2C.In())
                }
            ).run();
        }
    }
}