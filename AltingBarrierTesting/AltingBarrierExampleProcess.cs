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
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using CSPlang;

namespace AltingBarrierTesting
{

    public class AltingBarrierExampleProcess : IamCSProcess
    {

        private readonly AltingChannelInput input;
        private readonly AltingBarrier barrier;
        private readonly ChannelOutput output;
        int num = 0;

        public AltingBarrierExampleProcess(
            AltingChannelInput input, AltingBarrier barrier, ChannelOutput output, int num
        )
        {
            this.input = input;
            this.barrier = barrier;
            this.output = output;
            this.num = num;
        }

        public void run()
        {
            int n = 1 * num;

            CSTimer timer = new CSTimer();
            Random rnd = new Random();
            int randomTimeout = rnd.Next(1, 10) * 500;
            Alternative barrierGroupAlt = new Alternative(new Guard[] { input, barrier});

            const int INPUT = 0, GROUP = 1, TIMER = 2;

            output.write(n.ToString());

            
            while (true)
            {
                bool pending = input.pending();
                while (!pending)
                {
                    Debug.WriteLine((char)num + " pending = " + pending);

                    n++;
                    output.write((char)num + " " + n.ToString()); // work with the barrier
                    pending = input.pending();
                }
                input.read(); // must consume the input
                output.write("Read click " + n.ToString()); // work with the barrier

                Boolean group = true;
                while (group)
                {
                    switch (barrierGroupAlt.priSelect())
                    {
                        case INPUT:
                            Debug.WriteLine((char)num + "barier input");
                            string x = input.read().ToString(); // must consume the input
                            Console.WriteLine(x);
                            group = false; // end barrier working
                            break;
                        case GROUP:
                            n--; // work with the barrier
                            output.write(n.ToString()); // work with the barrier
                            Debug.WriteLine((char)num + "barier wrote " + n);
                            break;                       
                    }
                }

            }

        }

    }
}