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

namespace CSPnet2.Barriers
{
    /**
     * @author Kevin Chalmers
     */
    sealed class BarrierDataState
    {
        private BarrierDataState()
        {
        }

        /**
         * Barrier is inactive. It has not been initialised yet.
         */
        internal /*static*/ const byte INACTIVE = 0;

        /**
         * Barrier is in OK state, and is a server end. Has been initialised.
         */
        internal /*static*/ const byte OK_SERVER = 1;

        /**
         * Barrier is in OK state, and is a client end. Has been initialised
         */
        internal /*static*/ const byte OK_CLIENT = 2;

        /**
         * Barrier is broken.
         */
        internal /*static*/ const byte BROKEN = 3;

        /**
         * Barrier has been destroyed
         */
        internal /*static*/ const byte DESTROYED = 4;

        /**
         * Barrier has resigned from the server front end.
         */
        internal /*static*/ const byte RESIGNED = 5;
    }
}