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

using CSPnet2.Node;

namespace CSPnet2
{

    /**
     * This abstract class defines a data structure that is a location of a networked synchronization mechanism. Currently,
     * JCSP offers two location structures - NetChannelLocation and NetBarrierLocation. See the relevant documentation for
     * more information.
     * 
     * @see NetChannelLocation
     * @see NetBarrierLocation
     * @author Kevin Chalmers
     */
    public abstract class NetLocation
    {
        /**
         * Gets the NodeID part of the location structure
         * 
         * @return the NodeID part of the NetLocation
         */
        public abstract NodeID getNodeID();

        /**
         * Gets the NodeAddress part of the location structure
         * 
         * @return The NodeAddress part of the NetLocation
         */
        public abstract NodeAddress getNodeAddress();
    }
}