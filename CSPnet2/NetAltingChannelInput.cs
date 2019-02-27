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
using CSPlang.Alting;
using CSPnet2.NetChannels;

namespace CSPnet2
{
/**
 * A NetChannelInput that may be used as a guard. This class describes the abstract interface of such a channel. To
 * create an instance of this class, use the standard NetChannel factory, or the CNS. For information on the usage of
 * this object, see AltingChannelInput
 * 
 * @see AltingChannelInput
 * @see jcsp.lang.ChannelInput
 * @see NetChannelInput
 * @see NetChannel
 * @author Quickstone Technologies
 */
    public abstract class NetAltingChannelInput : AltingChannelInputWrapper, NetChannelInput
    {
        /**
         * Creates a new NetAltingChannelInput, with the given channel as the guard
         * 
         * @param in
         *            The channel that is used within the alternative
         */
        protected NetAltingChannelInput(AltingChannelInput In) : base(In)
        {
        }

        public abstract NetLocation getLocation();

        public abstract String GetLocationAsString();
        //{

        //    throw new NotImplementedException();
        //}

        public abstract void destroy();
        //{
        //    throw new NotImplementedException();
        //}

        public abstract void setDecoder(NetworkMessageFilter.FilterRx decoder);
        //{
        //    throw new NotImplementedException();
        //}
    }
}