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
using CSPlang;
using CSPnet2.NetChannel;
using CSPnet2.NetNode;

namespace CSPnet2.Mobile
{


//import jcsp.lang.ProcessManager;
//import jcsp.net2.NetChannelLocation;
//import jcsp.net2.NodeID;

/**
 * @author Kevin
 */

    [Serializable]
sealed class DynamicClassLoaderMessage{
    static
    {
        ClassManager classManager = new ClassManager();
        new ProcessManager(classManager).start();
    }

    readonly NodeID originatingNode;
    readonly NetChannelLocation requestLocation;
    readonly byte[] bytes;

    DynamicClassLoaderMessage(NodeID originator, NetChannelLocation request, byte[] classData)
    {
        this.originatingNode = originator;
        this.requestLocation = request;
        this.bytes = classData;
    }
}
}