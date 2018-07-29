using System;

namespace CSPlang
{
    public interface ChannelInput : Poisonable
    {
        /**
     * Read an Object from the channel.
     *
     * @return the object read from the channel
     */
        /*public*/ Object read();

        /**
         * Begins an extended rendezvous read from the channel.
         * 
         * In an extended rendezvous, the communication is not 
         * completed until the reader has completed its extended 
         * action (in JCSP, this means that the writer has to wait
         * until the reader calls the corresponding endExtRead
         * function).  The writer notices no difference in the
         * communication (except that it usually takes longer).
         * 
         * <b>You must call {@link endRead} at some point 
         * after this function</b>, otherwise the writer will never
         * be freed and deadlock will almost certainly ensue.
         * 
         * You may perform any actions you like between calling 
         * beginExtRead and {@link endRead}, including communications
         * on other channels, but you must not attempt to communicate 
         * again on this channel until you have called {@link endExtRead}.
         * 
         * An extended rendezvous may be used after the channel's Guard
         * has been selected by an Alternative (i.e. it can be done
         * in place of calling {@link read}).
         * 
         * @return The object read from the channel 
         */
        /*public*/ Object startRead();

        /**
         * The call that signifies the end of the extended rendezvous,
         * as begun by {@link endRead}.  This function should only
         * ever be called after {@link endRead}, and must be called
         * once (and only once) after every {@link endRead} call.  
         *
         * @see startRead
         */
        /*public*/ void endRead();

    }
}