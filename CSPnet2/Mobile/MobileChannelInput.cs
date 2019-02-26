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
using CSPnet2.NetChannels;

namespace CSPnet2.Mobile
{
    [Serializable]
    public sealed class MobileChannelInput : NetChannelInput
    {
        private NetChannelLocation messageBoxLoc;

        private NetChannelLocation msgBoxReqLoc;

        [NonSerialized] private /*transient*/ NetChannelInput actualIn;

        [NonSerialized] private /*transient*/ NetChannelOutput toMessageBox;

        public MobileChannelInput()
        {
            NetAltingChannelInput toMsgBox = NetChannel.net2one();
            NetAltingChannelInput msgBoxReq = NetChannel.net2one();
            MessageBox msgBox = new MessageBox(toMsgBox, msgBoxReq, new ObjectNetworkMessageFilter.FilterTX());
            new ProcessManager(msgBox).start();
            this.messageBoxLoc = (NetChannelLocation) toMsgBox.getLocation();
            this.msgBoxReqLoc = (NetChannelLocation) msgBoxReq.getLocation();
            this.actualIn = NetChannel.net2one();
            this.toMessageBox = NetChannel.one2net(this.msgBoxReqLoc);
        }

        public MobileChannelInput(NetworkMessageFilter.FilterTx encoder, NetworkMessageFilter.FilterRx decoder)
        {
            NetAltingChannelInput toMsgBox = NetChannel.net2one(decoder);
            NetAltingChannelInput msgBoxReq = NetChannel.net2one();
            MessageBox msgBox = new MessageBox(toMsgBox, msgBoxReq, encoder);
            new ProcessManager(msgBox).start();
            this.messageBoxLoc = (NetChannelLocation) toMsgBox.getLocation();
            this.msgBoxReqLoc = (NetChannelLocation) msgBoxReq.getLocation();
            this.actualIn = NetChannel.net2one(decoder);
            this.toMessageBox = NetChannel.one2net(this.msgBoxReqLoc);
        }

        public void endRead()
        {
            this.actualIn.endRead();
        }

        public Object read()
        {
            MobileChannelMessage msg = new MobileChannelMessage();
            msg.type = MobileChannelMessage.REQUEST;
            msg.inputLocation = (NetChannelLocation) this.actualIn.getLocation();
            this.toMessageBox.write(msg);
            Object toReturn = this.actualIn.read();
            return toReturn;
        }

        public Object startRead()
        {
            MobileChannelMessage msg = new MobileChannelMessage();
            msg.type = MobileChannelMessage.REQUEST;
            msg.inputLocation = (NetChannelLocation) this.actualIn.getLocation();
            this.toMessageBox.write(msg);
            Object toReturn = this.actualIn.startRead();
            return toReturn;
        }

        public void poison(int strength)
        {
            this.actualIn.poison(strength);
        }

        public void destroy()
        {
            this.actualIn.destroy();
            this.toMessageBox.destroy();
        }

        public NetLocation getLocation()
        {
            return this.messageBoxLoc;
        }

        public void setDecoder(NetworkMessageFilter.FilterRx decoder)
        {
            this.actualIn.setDecoder(decoder);
        }

        //private void writeObject(ObjectOutputStream output)
        //    //throws IOException
        //{
        //    output.writeObject(this.messageBoxLoc);
        //    output.writeObject(this.msgBoxReqLoc);
        //    this.actualIn.destroy();
        //    this.toMessageBox.destroy();
        //}

        //private void readObject(ObjectInputStream input)
        //    //throws IOException, ClassNotFoundException
        //{
        //    this.messageBoxLoc = (NetChannelLocation)input.readObject();
        //    this.msgBoxReqLoc = (NetChannelLocation)input.readObject();
        //    this.actualIn = NetChannel.net2one();
        //    this.toMessageBox = NetChannel.one2net(this.messageBoxLoc);
        //}
    }
}