
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

package jcsp.net2.mobile;

import java.io.Serializable;

import jcsp.net2.NetChannelLocation;
import jcsp.net2.NodeID;

/**
 * @author Kevin
 */
final class ClassRequest
    implements Serializable
{
    final NodeID originatingNode;
    final String className;
    final NetChannelLocation returnLocation;

    ClassRequest(NodeID originator, String name, NetChannelLocation response)
    {
        this.originatingNode = originator;
        this.className = name;
        this.returnLocation = response;
    }
}
