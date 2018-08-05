using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang.Alting
{
    /**
 * This extends {@link Guard} and {@link ChannelInputInt}
 * to enable a process
 * to choose between many integer input (and other) events.
 * <H2>Description</H2>
 * <TT>AltingChannelInputInt</TT> extends {@link Guard} and {@link ChannelInputInt}
 * to enable a process
 * to choose between many integer input (and other) events.  The methods inherited from
 * <TT>Guard</TT> are of no concern to users of this package.
 * </P>
 * <H2>Example</H2>
 * <PRE>
 * import jcsp.lang.*;
 * <I></I>
 * public class AltingIntExample implements CSProcess {
 * <I></I>
 *   private final AltingChannelInputInt in0, in1;
 *   <I></I>
 *   public AltingIntExample (final AltingChannelInputInt in0,
 *                            final AltingChannelInputInt in1) {
 *     this.in0 = in0;
 *     this.in1 = in1;
 *   }
 * <I></I>
 *   public void run () {
 * <I></I>
 *     final Guard[] altChans = {in0, in1};
 *     final Alternative alt = new Alternative (altChans);
 * <I></I>
 *     while (true) {
 *       switch (alt.select ()) {
 *         case 0:
 *           System.out.println ("in0 read " + in0.read ());
 *         break;
 *         case 1:
 *           System.out.println ("in1 read " + in1.read ());
 *         break;
 *       }
 *     }
 * <I></I>
 *   }
 * <I></I>
 * }
 * </PRE>
 *
 * @see jcsp.lang.Guard
 * @see jcsp.lang.Alternative
 * @author P.D.Austin and P.H.Welch
 */

    public abstract class AltingChannelInputInt : Guard, ChannelInputInt
    {
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
