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
using CSPnet2.NetChannels;

namespace CSPnet2.Mobile
{
/**
 * @author Kevin
 */

    [Serializable]
    public sealed class MobileChannelOutput : NetChannelOutput
    {
        private NetChannelLocation msgBoxLocation;

        [NonSerialized] private /*transient*/ NetChannelOutput actualOut;

        public MobileChannelOutput(NetChannelLocation loc)
        {
            this.msgBoxLocation = loc;
            this.actualOut = NetChannel.one2net(loc);
        }

        public MobileChannelOutput(NetChannelLocation loc, NetworkMessageFilter.FilterTx encoder)
        {
            this.msgBoxLocation = loc;
            this.actualOut = NetChannel.one2net(loc, encoder);
        }

        public void write(Object _object)
        {
            this.actualOut.write(_object);
        }

        public void destroy()
        {
            this.actualOut.destroy();
        }

        public NetLocation getLocation()
        {
            return this.actualOut.getLocation();
        }

        public void poison(int strength)
        {
            this.actualOut.poison(strength);
        }

        public void asyncWrite(Object obj)
            //throws JCSPNetworkException, NetworkPoisonException
        {
            this.actualOut.asyncWrite(obj);
        }

        public void setEncoder(NetworkMessageFilter.FilterTx encoder)
        {
            this.actualOut.setEncoder(encoder);
        }

        //private void writeObject(ObjectOutputStream output)
        //    //throws IOException
        //{
        //    output.writeObject(this.msgBoxLocation);
        //    this.actualOut.destroy();
        //}

        //private void readObject(ObjectInputStream input)
        //    //throws IOException, ClassNotFoundException
        //{
        //    this.msgBoxLocation = (NetChannelLocation)input.readObject();
        //    this.actualOut = NetChannel.one2net(this.msgBoxLocation);
        //}
    }
}