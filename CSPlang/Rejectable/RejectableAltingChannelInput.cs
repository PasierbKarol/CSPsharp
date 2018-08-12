namespace CSPlang
{

    /**
     * Defines an interface for an input channel end that gives the reader the ability to reject instead
     * of accepting pending data.
     *
     *
     * 
     * @deprecated This channel is superceded by the poison mechanisms, please see {@link PoisonException}
     */
    public abstract class RejectableAltingChannelInput : AltingChannelInput, RejectableChannelInput
    {
        /**
         * Reject any data pending instead of reading it. The currently blocked writer will receive a
         * <Code>ChannelDataRejectedException</code>.
         */
        public abstract void reject();
    }
}