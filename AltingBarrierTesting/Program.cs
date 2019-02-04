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

namespace AltingBarrierTesting
{
    public class Program
    {
        public static void Main(String[] argv)
        {
            int nUnits = 8;

            Console.WriteLine("Enter number of units: ");
            nUnits =  Int32.Parse(Console.ReadLine());

            // make the buttons

            One2OneChannel[] a = Channel.one2oneArray(nUnits);

            One2OneChannel[] b = Channel.one2oneArray(nUnits);

            ProcessesArray processesArray =
                new ProcessesArray(
                     nUnits, Channel.getInputArray(b),
                    Channel.getOutputArray(a)
                );

            // construct an array of front-ends to a single alting barrier

            AltingBarrier[] group = AltingBarrier.create(nUnits);

            // make the gadgets
            AltingBarrierExampleProcess[] barriers = new AltingBarrierExampleProcess[nUnits];
            for (int i = 0; i < barriers.Length; i++)
            {
                barriers[i] = new AltingBarrierExampleProcess(a[i].In(), group[i], b[i].Out(), i+65);
            }

            // run everything

            new CSPParallel(
                new IamCSProcess[]
                {
                    processesArray, new CSPParallel(barriers)
                }
            ).run();
        }
    }
}