using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang
{
    public abstract class AltingChannelInput : Guard, ChannelInput
    {
        // nothing alse to add ... except ...

        /**
         * Returns whether there is data pending on this channel.
         * <P>
         * <I>Note: if there is, it won't go away until you read it.  But if there
         * isn't, there may be some by the time you check the result of this method.</I>
         *
         * @return state of the channel.
         */
        public abstract Boolean pending();
    }
}
