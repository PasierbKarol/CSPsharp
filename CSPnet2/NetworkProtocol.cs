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

namespace CSPnet2
{
    /**
     * This class defines the constants used within the Link interactions. This is the network protocol for JCSP. This is an
     * internal class to JCSP, used specifically between Links and the Links and the networked constructs.
     * 
     * @author Kevin Chalmers
     */
    sealed class NetworkProtocol
    {
        /**
         * Empty constructor. This is a static set of values
         */
        private NetworkProtocol()
        {
        }

        /**
         * A SEND message from an output end to an input end
         */
        internal /*static*/ const byte SEND = 1;

        /**
         * An ACKnowledgment that releases an output end after a write
         */
        internal /*static*/ const byte ACK = 2;

        /**
         * An ENROLLment from a client end of a NetBarrier to a server end
         */
        internal /*static*/ const byte ENROLL = 3;

        /**
         * A RESIGNation of a client end of a NetBarrier from a server end
         */
        internal /*static*/ const byte RESIGN = 4;

        /**
         * A SYNChronization message sent from a client end of a NetBarrier to a server end when the client's local
         * processes have all synchronised
         */
        internal /*static*/ const byte SYNC = 5;

        /**
         * RELEASEs a waiting client end of a NetBarrier when the server end has completely been synced with
         */
        internal /*static*/ const byte RELEASE = 6;

        /**
         * Rejects a message sent from a NetBarrier.
         */
        internal /*static*/ const byte REJECT_BARRIER = 7;

        /**
         * Rejects a message sent from a NetChannelOutput
         */
        internal /*static*/ const byte REJECT_CHANNEL = 8;

        /**
         * Signifies that a Link has been lost
         */
        internal /*static*/ const byte LINK_LOST = 9;

        /**
         * Mobility message. Still to be defined
         */
        internal /*static*/ const byte MOVED = 10;

        /**
         * Mobility message. Still to be defined
         */
        internal /*static*/ const byte ARRIVED = 11;

        /**
         * A POISON message sent to poison a channel end
         */
        internal /*static*/ const byte POISON = 12;

        /**
         * An Asynchronous send operation
         */
        internal /*static*/ const byte ASYNC_SEND = 13;

        /**
         * The initial message sent from a client connection end to a server end
         */
        internal /*static*/ const byte OPEN = 14;

        /**
         * The subsequent communications from a client connection before closing
         */
        internal /*static*/ const byte REQUEST = 15;

        /**
         * The reply from the server end of a connection
         */
        internal /*static*/ const byte REPLY = 16;

        /**
         * A reply from the server end of a connection which also closes the connection
         */
        internal /*static*/ const byte REPLY_AND_CLOSE = 17;

        /**
         * An asynchronous open message
         */
        internal /*static*/ const byte ASYNC_OPEN = 18;

        /**
         * An asynchronous request to a connection server
         */
        internal /*static*/ const byte ASYNC_REQUEST = 19;

        /**
         * An asynchronous reply from the server
         */
        internal /*static*/ const byte ASYNC_REPLY = 20;

        /**
         * An asynchronous reply and close
         */
        internal /*static*/ const byte ASYNC_REPLY_AND_CLOSE = 21;

        /**
         * An acknowledgement of the initial OPEN or REQUEST by a client connection end
         */
        internal /*static*/ const byte REQUEST_ACK = 22;

        /**
         * An acknowledgement of a connection server REPLY
         */
        internal /*static*/ const byte REPLY_ACK = 23;

        /**
         * Rejects a message from a networked connection
         */
        internal /*static*/ const byte REJECT_CONNECTION = 24;
    }
}