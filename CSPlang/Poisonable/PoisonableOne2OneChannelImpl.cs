﻿using System;

namespace CSPlang
{
	[Serializable]
	internal class PoisonableOne2OneChannelImpl : One2OneChannel,  ChannelInternals
	{
		/** The monitor synchronising reader and writer on this channel */
		private Object rwMonitor = new Object();

		/** The (invisible-to-users) buffer used to store the data for the channel */
		private Object hold;

		/** The synchronisation flag */
		private Boolean empty = true;

		/**
		   * This flag indicates that the last transfer went OK. The purpose is to not
		   * throw a PoisonException to the writer side when the last transfer went
		   * OK, but the reader side injected poison before the writer side finished
		   * processing of the last write transfer.
		   */
		private Boolean done = false;

		/**
		 * 0 means unpoisoned
		 */
		private int poisonStrength = 0;

		/**
		 * Immunity is passed to the channel-ends, and is not used directly by the channel algorithms
		 */
		private int immunity;

		/** The Alternative class that controls the selection */
		private Alternative alt;

		/** Flag to deal with a spurious wakeup during a write */
		private Boolean spuriousWakeUp = true;


		private Boolean isPoisoned()
		{
			return poisonStrength > 0;
		}

	   internal	PoisonableOne2OneChannelImpl(int _immunity)
		{
			immunity = _immunity;
		}

		/*************Methods from One2OneChannel******************************/

		/**
		 * Returns the <code>AltingChannelInput</code> to use for this channel.
		 * As <code>One2OneChannelImpl</code> implements
		 * <code>AltingChannelInput</code> itself, this method simply returns
		 * a reference to the object that it is called on.
		 *
		 * @return the <code>AltingChannelInput</code> object to use for this
		 *          channel.
		 */
		public AltingChannelInput In()
	{
		return new AltingChannelInputImpl(this, immunity);
	}

	/**
	 * Returns the <code>ChannelOutput</code> object to use for this channel.
	 * As <code>One2OneChannelImpl</code> implements
	 * <code>ChannelOutput</code> itself, this method simply returns
	 * a reference to the object that it is called on.
	 *
	 * @return the <code>ChannelOutput</code> object to use for this
	 *          channel.
	 */
	public ChannelOutput Out()
	{
		return new ChannelOutputImpl(this, immunity);
}

/*************Methods from ChannelOutput*******************************/

/**
 * Writes an <TT>Object</TT> to the channel.
 *
 * @param value the object to write to the channel.
 */
public void write(Object value)
{
	lock (rwMonitor) {
		if (isPoisoned())
		{
			throw new PoisonException(poisonStrength);
		}

		hold = value;
		if (empty)
		{
			empty = false;
			if (alt != null)
			{
				alt.schedule();
			}
		}
		else
		{
			empty = true;
			rwMonitor.notify();
		}
		try
		{
			rwMonitor.wait();
			while (spuriousWakeUp && !isPoisoned())
			{
				if (Spurious.logging)
				{
					SpuriousLog.record(SpuriousLog.One2OneChannelWrite);
				}
				rwMonitor.wait();
			}
			spuriousWakeUp = true;
		}
		catch (InterruptedException e)
		{
			throw new ProcessInterruptedException(
				"*** Thrown from One2OneChannel.write (Object)\n" + e.toString());
		}

		if (done)
		{
			done = false;
		}
		else if (isPoisoned())
		{
			hold = null;
			throw new PoisonException(poisonStrength);
		}
		else
		{
			done = true;
		}

	}
}

/** ***********Methods from AltingChannelInput************************* */

/**
* Reads an <TT>Object</TT> from the channel.
*
* @return the object read from the channel.
*/
public Object read()
{
	lock (rwMonitor) {
		if (isPoisoned())
		{
			throw new PoisonException(poisonStrength);
		}


		if (empty)
		{
			empty = false;
			try
			{
				rwMonitor.wait();
				while (!empty && !isPoisoned())
				{
					if (Spurious.logging)
					{
						SpuriousLog.record(SpuriousLog.One2OneChannelRead);
					}
					rwMonitor.wait();
				}
			}
			catch (InterruptedException e)
			{
				throw new ProcessInterruptedException("*** Thrown from One2OneChannel.read ()\n"
												   + e.toString());
			}
		}
		else
		{
			empty = true;
		}
		spuriousWakeUp = false;

		if (isPoisoned())
		{
			throw new PoisonException(poisonStrength);
		}
		else
		{
			done = true;
			rwMonitor.notify();
			return hold;
		}
	}
}

public Object startRead()
{
	lock (rwMonitor) {
		if (isPoisoned())
		{
			throw new PoisonException(poisonStrength);
		}

		if (empty)
		{
			empty = false;
			try
			{
				rwMonitor.wait();
				while (!empty && !isPoisoned())
				{
					if (Spurious.logging)
					{
						SpuriousLog.record(SpuriousLog.One2OneChannelRead);
					}
					rwMonitor.wait();
				}
			}
			catch (InterruptedException e)
			{
				throw new ProcessInterruptedException("*** Thrown from One2OneChannel.read ()\n"
												   + e.toString());
			}
		}
		else
		{
			empty = true;
		}

		if (isPoisoned())
		{
			throw new PoisonException(poisonStrength);
		}

		return hold;
	}
}

public void endRead()
{
	lock (rwMonitor) {
		spuriousWakeUp = false;
		rwMonitor.notify();
	}
}


/**
 * turns on Alternative selection for the channel. Returns true if the
 * channel has data that can be read immediately.
 * <P>
 * <I>Note: this method should only be called by the Alternative class</I>
 *
 * @param alt the Alternative class which will control the selection
 * @return true if the channel has data that can be read, else false
 */
public Boolean readerEnable(Alternative alt)
{
	lock (rwMonitor) {
		if (isPoisoned())
		{
			return true;
		}


		if (empty)
		{
			this.alt = alt;
			return false;
		}
		else
		{
			return true;
		}
	}
}

/**
 * turns off Alternative selection for the channel. Returns true if the
 * channel contained data that can be read.
 * <P>
 * <I>Note: this method should only be called by the Alternative class</I>
 *
 * @return true if the channel has data that can be read, else false
 */
public Boolean readerDisable()
{
	lock (rwMonitor) {
		alt = null;
		return !empty || isPoisoned();
	}
}

/**
 * Returns whether there is data pending on this channel.
 * <P>
 * <I>Note: if there is, it won't go away until you read it.  But if there
 * isn't, there may be some by the time you check the result of this method.</I>
 * <P>
 * This method is provided for convenience.  Its functionality can be provided
 * by <I>Pri Alting</I> the channel against a <TT>SKIP</TT> guard, although
 * at greater run-time and syntactic cost.  For example, the following code
 * fragment:
 * <PRE>
 *   if (c.pending ()) {
 *     Object x = c.read ();
 *     ...  do something with x
 *   } else (
 *     ...  do something else
 *   }
 * </PRE>
 * is equivalent to:
 * <PRE>
 *   if (c_pending.priSelect () == 0) {
 *     Object x = c.read ();
 *     ...  do something with x
 *   } else (
 *     ...  do something else
 * }
 * </PRE>
 * where earlier would have had to have been declared:
 * <PRE>
 * final Alternative c_pending =
 *   new Alternative (new Guard[] {c, new Skip ()});
 * </PRE>
 *
 * @return state of the channel.
 */
public Boolean readerPending()
{
	lock (rwMonitor) {
		return !empty || isPoisoned();
	}
}

public void writerPoison(int strength)
{
	if (strength > 0)
	{
		lock (rwMonitor) {
			this.poisonStrength = strength;

			rwMonitor.notifyAll();

			if (null != alt)
			{
				alt.schedule();
			}
		}
	}
}
public void readerPoison(int strength)
{
	if (strength > 0)
	{
		lock (rwMonitor) {
			this.poisonStrength = strength;

			rwMonitor.notifyAll();
		}
	}
}
	}
}