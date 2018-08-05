using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang.Alting
{
	/**
 * This class wraps an ALTable channel so that only the reading part is
 * available to the caller.  Writes are impossible unless you subclass
 * this (and use getChannel()) or keep a reference to the original
 * channel.  <p>
 *
 * Note that usually you do not need the absolute guarantee that this class
 * provides - you can usually just cast the channel to an AltingChannelInput,
 * which prevents you from <I>accidentally</I> writing to the channel.  This
 * class mainly exists for use by some of the jcsp.net classes, where the
 * absolute guarantee that you cannot write to it is important.
 *
 * @see jcsp.lang.AltingChannelInput
 *
 *
 */

	public class AltingChannelInputWrapper : AltingChannelInput
	{
		/**
	 * Creates a new AltingChannelInputWrapper which wraps the specified
	 * channel.
	 */
	public AltingChannelInputWrapper(AltingChannelInput channel)
	{
		this.channel = channel;
	}

	/**
	 * This constructor does not wrap a channel.
	 * The underlying channel can be set by calling
	 * <code>setChannel(AltingChannelInput)</code>.
	 *
	 */
	protected AltingChannelInputWrapper()
	{
		this.channel = null;
	}

	/**
	 * The real channel which this object wraps.
	 *
	 * This used to be a final field but this caused problems
	 * when sub-classes wanted to be serializable. Added a
	 * protected mutator.
	 */
	private AltingChannelInput channel;

	/**
	 * Get the real channel.
	 *
	 * @return The real channel.
	 */
	protected AltingChannelInput getChannel()
	{
		return channel;
	}

	/**
	 * Sets the real channel to be used.
	 *
	 * @param chan the real channel to be used.
	 */
	protected void setChannel(AltingChannelInput chan)
	{
		this.channel = chan;
	}

	/**
	 * Read an Object from the channel.
	 *
	 * @return the object read from the channel
	 */
	public Object read()
	{
		return channel.read();
	}
	
	/**
	 * Begins an extended rendezvous
	 * 
	 * @see ChannelInput.beginExtRead
	 * @return The object read from the channel
	 */
	public Object startRead()
	{
		return channel.startRead();
	}
	
	/**
	 * Ends an extended rendezvous
	 * 
	 * @see ChannelInput.endExtRead
	 */
	public void endRead()
	{
		channel.endRead();
	}
	
	

	/**
	 * Returns whether there is data pending on this channel.
	 * <P>
	 * <I>Note: if there is, it won't go away until you read it.  But if there
	 * isn't, there may be some by the time you check the result of this method.</I>
	 *
	 * @return state of the channel.
	 */
	public Boolean pending()
	{
		return channel.pending();
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
		return channel.enable(alt);
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
		return channel.disable();
	}

	public void poison(int strength) 
	{
		channel.poison(strength);	
	}
	
	
	}
}
