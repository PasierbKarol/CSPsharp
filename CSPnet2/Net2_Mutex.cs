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
using System.Threading;
using CSPlang;

namespace CSPnet2
{
    /**
     * A package-visible class that implements a straightforward mutex, for use by Net2AnyChannel
     * 
     * @author Neil Brown
     */
    class Net2_Mutex
    {
        /**
         * Flag to mark the mutex as claimed
         */
        private Boolean claimed = false;

        /**
         * Claims the mutex for exclusive access
         */
        internal void claim()
        {
            lock (this)
            {
                while (this.claimed)
                {
                    try
                    {
                        Monitor.Wait(this);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        throw new ProcessInterruptedException("*** Thrown from Net2_Mutex.claim()\n" + e.ToString());
                    }
                }
                this.claimed = true;
            }
        }

        /**
         * Releases the mutex for exclusive access
         */
        internal void release()
        {
            lock (this)
            {
                this.claimed = false;
                Monitor.Pulse(this);
            }
        }
    }
}