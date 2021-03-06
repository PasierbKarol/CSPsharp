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
using System.Collections.Generic;
using CSPlang;

namespace CSPnet2
{
/**
 * A message received or to be sent via a Link. This is an internal structure to JCSP, and is an object encapsulation of
 * the messages sent between nodes
 * 
 * @author Kevin Chalmers
 */
    sealed class NetworkMessage
    {
        /**
         * The message type, as described in NetworkProtocol.
         */
        internal int type = (int) -1;

        /**
         * The first attribute of the message.
         */
        internal int attr1 = -1;

        /**
         * The second attribute of the message
         */
        internal int attr2 = -1;

        /**
         * Data sent in the message if relevant.
         */

        internal string jsonData = null;

        /**
         * ChannelOutput to the Link so that acknowledgements can be sent.
         */
        internal ChannelOutput toLink = null;

        public static explicit operator NetworkMessage(LinkedListNode<NetworkMessage> v)
        {
            throw new NotImplementedException();
        }
    }
}