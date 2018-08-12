using CSPlang.Any2;

namespace CSPlang
{
    /**
 * Defines an interface for a factory that can create arrays of integer carrying channels with
 * user-definable buffering semantics.
 *
 *
 */

    public interface BufferedChannelIntArrayFactory
    {
        /**
    * Creates a populated array of <code>n</code> <code>One2One</code> channels with the
    * specified buffering behaviour.
    *
    * @param buffer the buffer implementation to use.
    * @param n the size of the array.
    * @return the created array of channels.
    */
        One2OneChannelInt[] createOne2One(ChannelDataStoreInt buffer, int n);

        /**
         * Creates a populated array of <code>n</code> <code>Any2One</code> channels with the
         * specified buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @param n the size of the array.
         * @return the created array of channels.
         */
        Any2OneChannelInt[] createAny2One(ChannelDataStoreInt buffer, int n);

        /**
         * Creates a populated array of <code>n</code> <code>One2Any</code> channels with the
         * specified buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @param n the size of the array.
         * @return the created array of channels.
         */
        One2AnyChannelInt[] createOne2Any(ChannelDataStoreInt buffer, int n);

        /**
         * Creates a populated array of <code>n</code> <code>Any2Any</code> channels with the
         * specified buffering behaviour.
         *
         * @param buffer the buffer implementation to use.
         * @param n the size of the array.
         * @return the created array of channels.
         */
        Any2AnyChannelInt[] createAny2Any(ChannelDataStoreInt buffer, int n);
    }
}