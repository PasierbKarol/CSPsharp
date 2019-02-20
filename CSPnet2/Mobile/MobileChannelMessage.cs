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
using CSPnet2.NetChannel;

namespace CSPnet2.Mobile
{
/**
 * @author Kevin
 */

    [Serializable]
    sealed class MobileChannelMessage
    {
        static readonly int REQUEST = 1;

        static readonly int CHECK = 2;

        static readonly int CHECK_RESPONSE = 3;

        int type = -1;

        Boolean ready = false;

        NetChannelLocation inputLocation = null;
    }
}