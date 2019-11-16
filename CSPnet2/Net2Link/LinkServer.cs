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
using CSPnet2.NetNode;

namespace CSPnet2.Net2Link
{
    /**
     * Abstract class defining the LinkServer.
     * 
     * @author Kevin Chalmers
     */
    public abstract class LinkServer : IamCSProcess
    {
        /**
         * @param address
         * @//throws ArgumentException 
         * @//throws JCSPNetworkException
         */
        public static /*final*/ void start(NodeAddress address) //TODO how to throw exception here?
        //throws ArgumentException , JCSPNetworkException
        {
            Node.logger.log(typeof(LinkServer), "Attempting to start Link Server on " + address);
            LinkServer linkServer = address.createLinkServer();
            //ProcessManager linkServProc = new ProcessManager(linkServer);
            //linkServProc.SetPriority(Link.LINK_PRIORITY);
            //linkServProc.start();
            new ProcessManager(linkServer).start();
            Node.logger.log(typeof(LinkServer), "Link Server started on " + address);
        }

        /**
         * @param nodeID
         * @return The Link connected to the Node with the corresponding NodeID, or null if no such Node exists
         */
        protected /*final*/ Link requestLink(NodeID nodeID)
        {
            return LinkManager.getInstance().requestLink(nodeID);
        }

        /**
         * @param link
         * @return True if the Link to the Node was successfully registered, false otherwise
         */
        protected /*final*/ Boolean registerLink(Link link)
        {
            return LinkManager.getInstance().registerLink(link);
        }

        public abstract void run();

    }
}