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
using System.Threading.Tasks;
using CSPlang;

namespace CommsTimeSymetricTesting
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("Test of communication between JCSP processes");
            Console.WriteLine("Based on occam CommsTime.occ by Peter Welch, University of Kent at Canterbury");
            Console.WriteLine("Ported into Java by Oyvind Teig");
            Console.WriteLine("Now using the JCSP library (phw/pda1)");
            Console.WriteLine("This version uses *symmetric* channels");
            Console.WriteLine();

            int nIterations = 10000;
            Console.WriteLine(nIterations + " iterations per timing ...\n");

            //One2OneChannelSymmetricInt a = Channel.one2oneSymmetricInt();
            //One2OneChannelSymmetricInt b = Channel.one2oneSymmetricInt();
            //One2OneChannelSymmetricInt c = Channel.one2oneSymmetricInt();
            //One2OneChannelSymmetricInt d = Channel.one2oneSymmetricInt();


            //new Parallel(
            //    new CSProcess[] {
            //        new PrefixInt (0, c.in(), a.out()),
            //        new Delta2Int (a.in(), d.out(), b.out()),
            //        new SuccessorInt (b.in(), c.out()),
            //        new Consume (nIterations, d.in())
            //    }
            //).run();


            Console.WriteLine("\n\n\nOnly gets here if all above parallel processes fail ...\n\n\n");

        }
    }
}
