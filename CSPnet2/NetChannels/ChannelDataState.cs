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

namespace CSPnet2.NetChannels
{
    /**
     * Represents the state of the networked channel. For information on networked channels, see the relevant documentation.
     * 
     * @see NetChannelInput
     * @see jcsp.net2.NetChannelOutput
     * @author Kevin Chalmers
     */
    sealed class ChannelDataState
    {
        private ChannelDataState()
        {
        }

        /**
         * Signifies that the channel has not been activated yet.
         */
        internal /*static*/ const byte INACTIVE = 0;

        /**
         * Signifies that the channel has been started and is a input end.
         */
        internal /*static*/ const byte OK_INPUT = 1;

        /**
         * Signified that the channel has been started and is a output end.
         */
        internal /*static*/ const byte OK_OUTPUT = 2;

        /**
         * Signifies that the channel has been destroyed.
         */
        internal /*static*/ const byte DESTROYED = 3;

        /**
         * Signifies that the channel is broken. This is from the original JCSP model, and may be unnecessary as Destroyed
         * and Poisoned may cover this.
         */
        internal /*static*/ const byte BROKEN = 4;

        /**
         * Signifies that the channel has recently moved and has yet to be reestablished at a new location.
         */
        internal /*static*/ const byte MOVING = 5;

        /**
         * Signifies that the channel has moved to a new location and that this new location is available.
         */
        internal /*static*/ const byte MOVED = 6;

        /**
         * Signifies that the channel has been poisoned.
         */
        internal /*static*/ const byte POISONED = 7;
    }
}