using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang.Alting
{
    /**
 * This extends {@link Guard} and {@link ChannelAccept}
 * to enable a process to choose between many CALL channel (and other) events.
 * <H2>Description</H2>
 * <TT>AltingChannelAccept</TT> extends {@link Guard} and {@link ChannelAccept}
 * to enable a process
 * to choose between many CALL channel (and other) events.  The methods inherited from
 * <TT>Guard</TT> are of no concern to users of this package.
 *
 * <H2>Example</H2>
 * See the explanations and examples documented in {@link One2OneCallChannel} and
 * {@link Any2OneCallChannel}.
 *
 * @see jcsp.lang.Alternative
 * @see jcsp.lang.One2OneCallChannel
 * @see jcsp.lang.Any2OneCallChannel
 *
 * @author P.H.Welch
 */
    public abstract class AltingChannelAccept : Guard, ChannelAccept
    {

    }
}
