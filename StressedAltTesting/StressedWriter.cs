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
    public class StressedWriter: IamCSProcess
    {
        private readonly ChannelOutput Out;
        private readonly int channel;
        private readonly int writer;
        private readonly String id;

        public StressedWriter(ChannelOutput Out, int channel, int writer)
        {
            this.Out = Out;
            this.channel = channel;
            this.writer = writer;
            this.id = "channel " + channel + " writer " + writer;
        }

        public void run()
        {
            int n = 0;
            StressedPacket stressedPacketA = new StressedPacket(writer);
            StressedPacket stressedPacketB = new StressedPacket(writer);
            while (true)
            {
                // for (int i = 0; i < writer; i++) System.out.print ("  ");
                // System.out.println (id + " " + n);
                stressedPacketA.n = n;
                    Out.write(stressedPacketA);
                n++;
                // for (int i = 0; i < writer; i++) System.out.print ("  ");
                // System.out.println (id + " " + n);
                stressedPacketB.n = n;
                    Out.write(stressedPacketB);
                n++;
            }
        }
    }
}



