using CSPlang.Any2;
using CSPutil;

namespace CSPlang
{
    /**
 * Defines an interface for a factory that can create channels with user-definable buffering semantics.
 *
 *
 */
    public interface BufferedChannelFactory
    {
        /**
     * Creates a new <code>One2One</code> channel with the given buffering behaviour.
     *
     * @param buffer the buffer implementation to use.
     * @return the created channel.
     */
        One2OneChannel createOne2One(ChannelDataStore buffer);

        /**
         * Creates a new <code>Any2One</code> channel with the given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        Any2OneChannel createAny2One(ChannelDataStore buffer);

        /**
         * Creates a new <code>One2Any</code> channel with the given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        One2AnyChannel createOne2Any(ChannelDataStore buffer);

        /**
         * Creates a new <code>Any2Any</code> channel with the given buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @return the created channel.
         */
        Any2AnyChannel createAny2Any(ChannelDataStore buffer);

    }
}