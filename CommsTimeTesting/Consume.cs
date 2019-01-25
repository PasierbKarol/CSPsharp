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
using System.Collections.Generic;
using System.Text;
using CSPlang;
using CSPutil;

/**
 * @author P.H. Welch
 */
namespace CommsTimeTesting 
{
    public class Consume : IamCSProcess
    {
        private int nLoops;
        private ChannelInputInt In;

        public Consume(int nLoops, ChannelInputInt In)
        {
            this.nLoops = nLoops;
            this.In = In;
        }

        public void run()
        {

            int x = -1;
            int warm_up = 1000;
            Console.WriteLine("warming up ... ");
            for (int i = 0; i < warm_up; i++)
            {
                x = In.read();
            }
            Console.WriteLine("last number received = " + x);

            Console.WriteLine("1000 cycles completed ... timing now starting ...");

            while (true)
            {
                long t0 = CSPTimeMillis.CurrentTimeMillis();
                for (int i = 0; i < nLoops; i++)
                {
                    x = In.read();
                }
                long t1 = CSPTimeMillis.CurrentTimeMillis();

                Console.WriteLine("last number received = " + x);
                long microseconds = (t1 - t0) * 1000;
                long timePerLoop_us = (microseconds / ((long)nLoops));
                Console.WriteLine("   " + timePerLoop_us + " microseconds / iteration");
                timePerLoop_us = (microseconds / ((long)(4 * nLoops)));
                Console.WriteLine("   " + timePerLoop_us + " microseconds / communication");
                timePerLoop_us = (microseconds / ((long)(8 * nLoops)));
                Console.WriteLine("   " + timePerLoop_us + " microseconds / context switch");

            }

        }
    }
}


