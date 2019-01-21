//////////////////////////////////////////////////////////////////////
//                                                                  //
//  JCSP ("CSP for Java") Libraries                                 //
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
using CSPlang;

namespace PlugAndPlay
{
    /**
     * This process generates a constant stream of <tt>Integer</tt>s at a regular rate.
     * <H2>Process Diagram</H2>
     * <PRE>
     *    _____________
     *   |             | out
     *   | Regular (n) |-->--
     *   |_____________|
     * </PRE>
     * <H2>Description</H2>
     * This process generates a constant stream of <tt>Integer</tt>s at a regular rate
     * &ndash; at least, it does its best!
     * If the consumption of data is less than the set rate, that rate cannot be sustained.
     * If the consumption failure is only temporary, the set rate will be restored
     * when consumption resumes.
     * <P>
     * The interval (in msecs) defining the output flow rate is given by a constructor argument.
     * <H2>Channel Protocols</H2>
     * <TABLE BORDER="2">
     *   <TR>
     *     <TH COLSPAN="3">Output Channels</TH>
     *   </TR>
     *   <TR>
     *     <TH>out</TH>
     *     <TD>java.lang.Integer</TD>
     *     <TD>
     *       A constant value is sent down this channel at a regular rate.
     *     </TD>
     *   </TR>
     * </TABLE>
     *
     * @see jcsp.plugNplay.FixedDelay
     * @see jcsp.plugNplay.Regulate
     *
     * @author P.H. Welch
     */


    public class Regular : IamCSProcess
    {
        readonly private ChannelOutput Out;
        private int N;
        readonly private long interval;

        public Regular(ChannelOutput Out, int n, long interval)
        {
            this.Out = Out;
            this.N = n;
            this.interval = interval;
        }

        public void run()
        {
            CSTimer timer = new CSTimer();
            long timeout = timer.read(); // read the (absolute) time once only

            while (true)
            {
                N++;
                Out.write(N);
                timeout += interval; // set the next (absolute) timeOut
                timer.after(timeout); // wait until that (absolute) timeOut
            }
        }
    }
}