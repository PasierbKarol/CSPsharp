//////////////////////////////////////////////////////////////////////
//                                                                  //
//  JCSP ("CSP for Java") Libraries                                 //
// Copyright 1996-2017 Peter Welch, Paul Austin and Neil Brown      //
//           2005-2017 Kevin Chalmers and Jon Kerridge              //
//                                                                  //
// Licensed under the Apache License, Version 2.0 (the "License");  //
// you may not use this file except in compliance with the License. //
// You may obtain a copy of the License at                          //
//                                                                  //
//      http://www.apache.org/licenses/LICENSE-2.0                  //
//                                                                  //
// Unless required by applicable law or agreed to in writing,       //
// software distributed under the License is distributed on         //
// an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,  //
// either express or implied. See the License for the specific      //
// language governing permissions and limitations under the License.//
//                                                                  //
//                                                                  //
//                                                                  //
//                                                                  //
//  Author Contact: P.H.Welch@ukc.ac.uk                             //
//                                                                  //
//  Author contact: K.Chalmers@napier.ac.uk                         //
//                                                                  //
//                                                                  //
//////////////////////////////////////////////////////////////////////

using System;
using System.Threading;
using CSPlang.Alting;
using CSPutil;

namespace CSPlang
{

	/**
	 * This implements a one-to-one integer channel with user-definable buffering.
	 * <H2>Description</H2>
	 * <TT>BufferedOne2OneChannelIntImpl</TT> implements a one-to-one integer channel with
	 * user-definable buffering.  Multiple readers or multiple writers are
	 * not allowed -- these are catered for by {@link BufferedAny2OneChannel},
	 * {@link BufferedOne2AnyChannel} or {@link BufferedAny2AnyChannel}.
	 * <P>
	 * The reading process may {@link Alternative <TT>ALT</TT>} on this channel.
	 * The writing process is committed (i.e. it may not back off).
	 * <P>
	 * The constructor requires the user to provide
	 * the channel with a <I>plug-in</I> driver conforming to the
	 * {@link jcsp.util.ints.ChannelDataStoreInt <TT>ChannelDataStoreInt</TT>}
	 * interface.  This allows a variety of different channel semantics to be
	 * introduced -- including buffered channels of user-defined capacity
	 * (including infinite), overwriting channels (with various overwriting
	 * policies) etc..
	 * Standard examples are given in the <TT>jcsp.util</TT> package, but
	 * <I>careful users</I> may write their own.
	 *
	 * @see jcsp.lang.Alternative
	 * @see jcsp.lang.BufferedAny2OneChannelIntImpl
	 * @see jcsp.lang.BufferedOne2AnyChannelIntImpl
	 * @see jcsp.lang.BufferedAny2AnyChannelIntImpl
	 * @see jcsp.util.ints.ChannelDataStoreInt
	 *
	 * @author P.D.Austin
	 * @author P.H.Welch
	 */

	class BufferedOne2OneChannelIntImpl : One2OneChannelInt, ChannelInternalsInt
	{
	/** The monitor synchronising reader and writer on this channel */
	private Object rwMonitor = new Object();

	/** The Alternative class that controls the selection */
	private Alternative alt;

	/** The ChannelDataStoreInt used to store the data for the channel */

	private readonly ChannelDataStoreInt data;

	/*************Methods from One2OneChannelInt******************************/

	/**
	 * Returns the <code>AltingChannelInputInt</code> object to use for this
	 * channel. As <code>One2OneChannelIntImpl</code> implements
	 * <code>AltingChannelInputInt</code> itself, this method simply returns
	 * a reference to the object that it is called on.
	 *
	 * @return the <code>AltingChannelInputInt</code> object to use for this
	 *          channel.
	 */
	public AltingChannelInputInt In()
	{
		return new AltingChannelInputIntImpl(this, 0);
}

/**
 * Returns the <code>ChannelOutputInt</code> object to use for this
 * channel. As <code>One2OneChannelIntImpl</code> implements
 * <code>ChannelOutputInt</code> itself, this method simply returns
 * a reference to the object that it is called on.
 *
 * @return the <code>ChannelOutputInt</code> object to use for this
 *          channel.
 */
public ChannelOutputInt Out()
	{
		return new ChannelOutputIntImpl(this, 0);
	}

	/**
	 * Constructs a new BufferedOne2OneChannelIntImpl with the specified ChannelDataStoreInt.
	 *
	 * @param data the ChannelDataStoreInt used to store the data for the channel
	 */
	public BufferedOne2OneChannelIntImpl(ChannelDataStoreInt data)
{
	if (data == null)
		throw new ArgumentException 
			("Null ChannelDataStoreInt given to channel constructor ...\n");
	this.data = (ChannelDataStoreInt)data.Clone();
}

/**
 * Reads an <TT>int</TT> from the channel.
 *
 * @return the integer read from the channel.
 */
public int read()
{
	lock (rwMonitor) {
		if (data.getState() == ChannelDataStoreState.EMPTY)
		{
			try
			{
				Monitor.Wait(rwMonitor);
				while (data.getState() == ChannelDataStoreState.EMPTY)
				{
					if (Spurious.logging)
					{
						SpuriousLog.record(SpuriousLog.One2OneChannelIntXRead);
					}

					Monitor.Wait(rwMonitor);
				}
			}
			catch (ThreadInterruptedException e)
			{
				throw new ProcessInterruptedException(
					"*** Thrown from One2OneChannelInt.read (int)\n" + e.ToString()
				);
			}
		}

		Monitor.Pulse(rwMonitor);
		return data.get();
	}
}

public int startRead()
{
	lock(rwMonitor) {
		if (data.getState() == ChannelDataStoreState.EMPTY)
		{
			try
			{
				Monitor.Wait(rwMonitor);
				while (data.getState() == ChannelDataStoreState.EMPTY)
				{
					if (Spurious.logging)
					{
						SpuriousLog.record(SpuriousLog.One2OneChannelXRead);
					}

					Monitor.Wait(rwMonitor);
				}
			}
			catch (ThreadInterruptedException e)
			{
				throw new ProcessInterruptedException(
					"*** Thrown from One2OneChannel.read (int)\n" + e.ToString()
				);
			}
		}

		return data.startGet();
	}
}

public void endRead()
{
	lock(rwMonitor) {
		data.endGet();
		Monitor.Pulse(rwMonitor);
	}
}

/**
 * Writes an <TT>int</TT> to the channel.
 *
 * @param value the integer to write to the channel.
 */
public void write(int value)
{
	lock(rwMonitor) {
		data.put(value);
		if (alt != null)
		{
			alt.schedule();
		}
		else
		{
			Monitor.Pulse(rwMonitor);
		}

		if (data.getState() == ChannelDataStoreState.FULL)
		{
			try
			{
				Monitor.Wait(rwMonitor);
				while (data.getState() == ChannelDataStoreState.FULL)
				{
					if (Spurious.logging)
					{
						SpuriousLog.record(SpuriousLog.One2OneChannelIntXWrite);
					}

					Monitor.Wait(rwMonitor);
				}
			}
			catch (ThreadInterruptedException e)
			{
				throw new ProcessInterruptedException(
					"*** Thrown from One2OneChannelInt.write (int)\n" + e.ToString()
				);
			}
		}
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
	lock(rwMonitor) {
		if (data.getState() == ChannelDataStoreState.EMPTY)
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
	lock(rwMonitor) {
		alt = null;
		return data.getState() != ChannelDataStoreState.EMPTY;
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
 *     int x = c.read ();
 *     ...  do something with x
 *   } else (
 *     ...  do something else
 *   }
 * </PRE>
 * is equivalent to:
 * <PRE>
 *   if (c_pending.priSelect () == 0) {
 *     int x = c.read ();
 *     ...  do something with x
 *   } else (
 *     ...  do something else
 * }
 * </PRE>
 * where earlier would have had to have been declared:
 * <PRE>
 * readonly Alternative c_pending =
 *   new Alternative (new Guard[] {c, new Skip ()});
 * </PRE>
 *
 * @return state of the channel.
 */
public Boolean readerPending()
{
	lock(rwMonitor) {
		return (data.getState() != ChannelDataStoreState.EMPTY);
	}
}

//  No poison in these channels:
public void writerPoison(int strength)
{
}

public void readerPoison(int strength)
{
}
	}
}