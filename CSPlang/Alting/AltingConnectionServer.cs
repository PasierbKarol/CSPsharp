﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang.Alting
{
    public abstract class AltingConnectionServer : Guard, ConnectionServer
    {
        /**
    * The channel used to ALT over.
    */
        private AltingChannelInput altingChannel;

        /**
         * Constructor.
         *
         * Note that this is only intended for use by JCSP, and should
         * not be called by user processes.  Users should use one of the
         * subclasses.
         *
         * @param altingChannel The channel used to implement the Guard
         */
        protected AltingConnectionServer(AltingChannelInput altingChannel)
        {
            this.altingChannel = altingChannel;
        }

        /**
         * Returns the channel used to implement the Guard.
         *
         * Note that this method is only intended for use by
         * JCSP, and should not be called by user processes.
         *
         * Concrete subclasses should override this method to
         * return null, to ensure that the alting channel is
         * kept private.
         *
         * @return The channel passed to the constructor.
         */
        protected AltingChannelInput getAltingChannel()
        {
            return altingChannel;
        }

        /**
         * <code>ConnectionServer</code> implementations are likely to be
         * implemented over channels. Multiple channels from the client
         * to server may be used; one could be used for the initial
         * connection while another one could be used for data requests.
         *
         * This method allows sub-classes to specify which channel should
         * be the next one to be alted over.
         *
         * @param	chan	the channel to be ALTed over.
         */
        protected void setAltingChannel(AltingChannelInput chan)
        {
            altingChannel = chan;
        }

        /**
         * Returns true if the event is ready.  Otherwise, this enables the guard
         * for selection and returns false.
         * <P>
         * <I>Note: this method should only be called by the Alternative class</I>
         *
         * @param alt the Alternative class that is controlling the selection
         * @return true if and only if the event is ready
         */
        Boolean enable(Alternative alt)
        {
            return altingChannel.enable(alt);
        }

        /**
         * Disables the guard for selection. Returns true if the event was ready.
         * <P>
         * <I>Note: this method should only be called by the Alternative class</I>
         *
         * @return true if and only if the event was ready
         */
        Boolean disable()
        {
            return altingChannel.disable();
        }

        /**
         * Returns whether there is an open() pending on this connection. <p>
         *
         * <i>Note: if there is, it won't go away until you accept it.  But if
         * there isn't, there may be one by the time you check the result of
         * this method.</i>
         *
         * @return true only if open() will complete without blocking.
         */
        public Boolean pending()
        {
            return altingChannel.pending();
        }

        public object request()
        {
            throw new NotImplementedException();
        }

        public void reply(object data)
        {
            throw new NotImplementedException();
        }

        public void reply(object data, bool close)
        {
            throw new NotImplementedException();
        }

        public void replyAndClose(object data)
        {
            throw new NotImplementedException();
        }
    }
}
