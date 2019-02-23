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

namespace CSPnet2.NetConnection
{
    /**
     * @author Kevin Chalmers
     */
    sealed class ConnectionDataState
{
    internal /*static*/ const byte INACTIVE = 0;

    internal /*static*/ const byte CLIENT_STATE_CLOSED = 1;

    internal /*static*/ const byte CLIENT_STATE_OPEN = 2;

    internal /*static*/ const byte CLIENT_STATE_MADE_REQ = 3;

    internal /*static*/ const byte SERVER_STATE_CLOSED = 4;

    internal /*static*/ const byte SERVER_STATE_OPEN = 5;

    internal /*static*/ const byte SERVER_STATE_RECEIVED = 6;

    internal /*static*/ const byte DESTROYED = 7;

    internal /*static*/ const byte BROKEN = 8;
}
}