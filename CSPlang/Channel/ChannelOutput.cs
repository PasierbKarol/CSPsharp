using System;

namespace CSPlang
{
    public interface ChannelOutput : Poisonable
    {
        /**
 * Write an Object to the channel.
 *
 * @param object the object to write to the channel
 */
        /*public*/ void write(Object object_name);
    }
}