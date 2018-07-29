using System;

namespace CSPlang
{

	
	public class Alternative
	{

		private static readonly DateTime Jan1st1970 = new DateTime
			(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long CurrentTimeMillis()
		{
			return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
		}


		//======================================================= Karol's Code above ============================


		/** The monitor synchronising the writers and alting reader */
		protected Object altMonitor = new Object();

		private static readonly  int enabling = 0;
		private static readonly  int waiting = 1;
		private static readonly  int ready = 2;
		private static readonly  int inactive = 3;

		/** The state of the ALTing process. */
		private int state = inactive;

		/** The array of guard events from which we are selecting. */
		private readonly Guard[] guard;

		/** The index of the guard with highest priority for the next select. */
		private int favourite = 0;  // invariant: 0 <= favourite < guard.length

		/** The index of the selected guard. */
		private int selected;       // after the enable/disable sequence :
									//    0 <= selected < guard.length

		private readonly  int NONE_SELECTED = -1;

		/** This indicates whether an AltingBarrier is one of the Guards. */
		private Boolean barrierPresent;

		/** This flag is set by a successful AltingBarrier enable/disable. */
		private Boolean barrierTrigger = false;

		/** The index of a selected AltingBarrier. */
		private int barrierSelected;

		/**
		 * This is the index variable used during the enable/disable sequences.
		 * This has been made global to simplify the call-back (setTimeout) from
		 * a CSTimer that is being enabled.  That call-back sets the timeout, msecs
		 * and timeIndex variables below.  The latter variable is needed only to
		 * work around the bug that Java wait-with-timeouts sometimes return early.
		 */
		private int enableIndex;

		/** This flag is set if one of the enabled guards was a CSTimer guard. */
		private Boolean timeout = false;

		/** If one or more guards were CSTimers, this holds the earliest timeout. */
		private long msecs;

		/**
		 * If one or more guards were CSTimers, this holds the index of the one
		 * with the earliest timeout.
		 */
		private int timeIndex;

		/**
		 * Construct an <code>Alternative</code> object operating on the {@link Guard}
		 * array of events.  Supported guard events are channel inputs
		 * ({@link AltingChannelInput} and {@link AltingChannelInputInt}),
		 * CALL channel accepts ({@link AltingChannelAccept}),
		 * barriers ({@link AltingBarrier}),
		 * timeouts ({@link CSTimer}) and skips ({@link Skip}).
		 * <P>
		 *
		 * @param guard the event guards over which the select operations will be made.
		 */
		public Alternative( Guard[] guard) //Guard here was readonly - Karol
		{
			this.guard = guard;
			for (int i = 0; i < guard.Length; i++)
			{
				if (guard[i] is AltingBarrier) 
			{
				barrierPresent = true;
				return;
			}
		}
		barrierPresent = false;
	}

	/**
	 * Returns the index of one of the ready guards. The method will block
	 * until one of the guards becomes ready.  If more than one is ready,
	 * an <I>arbitrary</I> choice is made.
	 */
	public /*sealed*/ int select()
	{
		return fairSelect();  // a legal implementation of arbitrary choice!
	}

	/**
	 * Returns the index of one of the ready guards. The method will block
	 * until one of the guards becomes ready.  If more than one is ready,
	 * the one with the lowest index is selected.
	 */
	public /*readonly*/  int priSelect()
	{
		// if (barrierPresent) {
		//   throw new AlternativeError (
		//     "*** Cannot 'priSelect' with an AltingBarrier in the Guard array"
		//   );
		// }
		state = enabling;
		favourite = 0;
		enableGuards();
		/*synchronized*/ lock   (altMonitor) {
			if (state == enabling)
			{
				state = waiting;
				try
				{
					if (timeout)
					{
						long delay = msecs - /*System.currentTimeMillis()*/ CurrentTimeMillis();
						if (delay > Spurious.earlyTimeout)
						{
							altMonitor.wait(delay);
							/*
									  while ((state == waiting) &&
											 ((delay = (msecs - System.currentTimeMillis ()))
										  > Spurious.earlyTimeout)) {
											if (Spurious.logging) {
										  SpuriousLog.record (SpuriousLog.AlternativeSelectWithTimeout);
										}
										altMonitor.wait (delay);
									  }
									  if ((state == waiting) && (delay > 0)) {
										if (Spurious.logging) {
										  SpuriousLog.incEarlyTimeouts ();
										}
										  }
							*/
							while (state == waiting)
							{
								delay = msecs - /*System.currentTimeMillis()*/ CurrentTimeMillis();
								if (delay > Spurious.earlyTimeout)
								{
									if (Spurious.logging)
									{
										SpuriousLog.record(SpuriousLog.AlternativeSelectWithTimeout);
									}
									altMonitor.wait(delay);
								}
								else
								{
									if ((delay > 0) && (Spurious.logging))
									{
										SpuriousLog.incEarlyTimeouts();
									}
									break;
								}
							}
							// System.out.println (state + " [" + delay + "]");
						}
					}
					else
					{
						altMonitor.wait();
						while (state == waiting)
						{
							if (Spurious.logging)
							{
								SpuriousLog.record(SpuriousLog.AlternativeSelect);
							}
							altMonitor.wait();
						}
					}
				}
				catch (InterruptedException e)
				{
					throw new ProcessInterruptedException(
				  "*** Thrown from Alternative.priSelect ()\n" + e.toString()
				);
				}
				state = ready;
			}
		}
		disableGuards();
		state = inactive;
		timeout = false;
		return selected;
	}

	/**
	 * Returns the index of one of the ready guards. The method will block
	 * until one of the guards becomes ready.  Consequetive invocations will
	 * service the guards `fairly' in the case when many guards are always
	 * ready.  <I>Implementation note: the last guard serviced has the lowest
	 * priority next time around.</I>
	 */
	public /*readonly*/  int fairSelect()
	{
		state = enabling;
		enableGuards();
		/*synchronized*/ lock  (altMonitor) {
			if (state == enabling)
			{
				state = waiting;
				try
				{
					if (timeout)
					{
						long delay = msecs - /*System.currentTimeMillis()*/ CurrentTimeMillis();
						// NOTE: below is code that demonstrates whether wait (delay)
						// sometimes returns early!  Because this happens in some JVMs,
						// we are forced into a workaround - see disableGuards ().
						//   long now = System.currentTimeMillis ();
						//   long delay = msecs - now;
						if (delay > Spurious.earlyTimeout)
						{
							altMonitor.wait(delay);
							/*
									  while ((state == waiting) &&
											 ((delay = msecs - System.currentTimeMillis ())
										  > Spurious.earlyTimeout)) {
											if (Spurious.logging) {
										  SpuriousLog.record (SpuriousLog.AlternativeSelectWithTimeout);
										}
										altMonitor.wait (delay);
									  }
									  if ((state == waiting) && (delay > 0)) {
										if (Spurious.logging) {
										  SpuriousLog.incEarlyTimeouts ();
										}
										  }
							*/
							while (state == waiting)
							{
								delay = msecs - /*System.currentTimeMillis()*/ CurrentTimeMillis();
								if (delay > Spurious.earlyTimeout)
								{
									if (Spurious.logging)
									{
										SpuriousLog.record(SpuriousLog.AlternativeSelectWithTimeout);
									}
									altMonitor.wait(delay);
								}
								else
								{
									if ((delay > 0) && (Spurious.logging))
									{
										SpuriousLog.incEarlyTimeouts();
									}
									break;
								}
							}
						}
						//   long then = System.currentTimeMillis ();
						//   System.out.println ("*** fairSelect: " + msecs +
						//                       ", " + now + ", " + delay +
						//                       ", " + then);
					}
					else
					{
						altMonitor.wait();
						while (state == waiting)
						{
							if (Spurious.logging)
							{
								SpuriousLog.record(SpuriousLog.AlternativeSelect);
							}
							altMonitor.wait();
						}
					}
				}
				catch (InterruptedException e)
				{
					throw new ProcessInterruptedException(
				  "*** Thrown from Alternative.fairSelect/select ()\n" + e.toString()
				);
				}
				state = ready;
			}
		}
		disableGuards();
		state = inactive;
		favourite = selected + 1;
		if (favourite == guard.length)
			favourite = 0;
		timeout = false;
		return selected;
	}

	/**
	 * Enables the guards for selection.  If any of the guards are ready,
	 * it sets selected to the ready guard's index, state to ready and returns.
	 * Otherwise, it sets selected to NONE_SELECTED and returns.
	 */
	private /*readonly*/  void enableGuards()
	{
		if (barrierPresent)
		{
			// System.out.println ("ENABLE barrier(s) present ...");
			AltingBarrierCoordinate.startEnable();
		}
		barrierSelected = NONE_SELECTED;
		for (enableIndex = favourite; enableIndex < guard.Length; enableIndex++)
		{
			if (guard[enableIndex].enable(this))
			{
				// if (guard[enableIndex] instanceof AltingChannelInput) {
				//   System.out.println ("CHANNEL ENABLE " + enableIndex + " SUCCEED");
				// } else {
				//   System.out.println ("ENABLE " + enableIndex + " SUCCEED");
				// }
				selected = enableIndex;
				state = ready;
				if (barrierTrigger)
				{
					barrierSelected = selected;
					barrierTrigger = false;
				}
				else if (barrierPresent)
				{
					// System.out.println ("ENABLE " + enableIndex + " NON-BARRIER SUCCEED");
					AltingBarrierCoordinate.finishEnable();
				}
				return;
			} // else {
			  // if (guard[enableIndex] instanceof AltingChannelInput) {
			  //   System.out.println ("CHANNEL ENABLE " + enableIndex + " FAIL");
			  // } else {
			  //   System.out.println ("ENABLE " + enableIndex + " FAIL");
			  // }
			  // }
		}
		for (enableIndex = 0; enableIndex < favourite; enableIndex++)
		{
			if (guard[enableIndex].enable(this))
			{
				// if (guard[enableIndex] instanceof AltingChannelInput) {
				//   System.out.println ("CHANNEL ENABLE " + enableIndex + " SUCCEED");
				// } else {
				//   System.out.println ("ENABLE " + enableIndex + " SUCCEED");
				// }
				selected = enableIndex;
				state = ready;
				if (barrierTrigger)
				{
					barrierSelected = selected;
					barrierTrigger = false;
				}
				else if (barrierPresent)
				{
					// System.out.println ("ENABLE " + enableIndex + " NON-BARRIER SUCCEED");
					AltingBarrierCoordinate.finishEnable();
				}
				return;
			} // else {
			  // if (guard[enableIndex] instanceof AltingChannelInput) {
			  //   System.out.println ("CHANNEL ENABLE " + enableIndex + " FAIL");
			  // } else {
			  //   System.out.println ("ENABLE " + enableIndex + " FAIL");
			  // }
			  // }
		}
		// System.out.println ("ENABLE ALL FAIL");
		selected = NONE_SELECTED;
		if (barrierPresent)
		{
			AltingBarrierCoordinate.finishEnable();
		}
	}

	/**
	 * Disables the guards for selection.  Sets selected to the index of
	 * the ready guard, taking care of priority/fair choice.
	 */
	private void disableGuards()
	{
		if (selected != favourite)
		{    // else there is nothing to disable
			int startIndex = (selected == NONE_SELECTED) ? favourite - 1 : selected - 1;
			if (startIndex < favourite)
			{
				for (int i = startIndex; i >= 0; i--)
				{
					if (guard[i].disable())
					{
						selected = i;
						if (barrierTrigger)
						{
							if (barrierSelected != NONE_SELECTED)
							{
								throw new JCSP_InternalError(
							  "*** Second AltingBarrier completed in ALT sequence: " +
							  barrierSelected + " and " + i
							);
							}
							barrierSelected = selected;
							barrierTrigger = false;
						}
					}
				}
				startIndex = guard.Length - 1;
			}
			for (int i = startIndex; i >= favourite; i--)
			{
				if (guard[i].disable())
				{
					selected = i;
					if (barrierTrigger)
					{
						if (barrierSelected != NONE_SELECTED)
						{
							throw new JCSP_InternalError(
							  "\n*** Second AltingBarrier completed in ALT sequence: " +
						  barrierSelected + " and " + i
							);
						}
						barrierSelected = selected;
						barrierTrigger = false;
					}
				}
			}
			if (selected == NONE_SELECTED)
			{
				// System.out.println ("disableGuards: NONE_SELECTED ==> " + timeIndex);
				// NOTE: this is a work-around for Java wait-with-timeouts sometimes
				// returning early.  If this did not happen, we would not get here!
				selected = timeIndex;
			}
		}
		if (barrierSelected != NONE_SELECTED)
		{        // We must choose a barrier sync
			selected = barrierSelected;                  // if one is ready - so that all
			AltingBarrierCoordinate.finishDisable();    // parties make the same choice.
		}
	}

	/**
	 * This is the call-back from enabling a CSTimer guard.  It is part of the
	 * work-around for Java wait-with-timeouts sometimes returning early.
	 * It is still in the flow of control of the ALTing process.
	 */
	void setTimeout(long msecs)
	{
		if (timeout)
		{
			if (msecs < this.msecs)
			{
				this.msecs = msecs;
				timeIndex = enableIndex;
			}
		}
		else
		{
			timeout = true;
			this.msecs = msecs;
			timeIndex = enableIndex;
		}
	}

	/**
	 * This is a call-back from an AltingBarrier.
	 * It is still in the flow of control of the ALTing process.
	 */
	void setBarrierTrigger()
	{
		barrierTrigger = true;
	}

	/**
	 * This is the wake-up call to the process ALTing on guards controlled
	 * by this object.  It is in the flow of control of a process writing
	 * to an enabled channel guard.
	 */
	public void schedule()
	{
		/*synchronized*/ lock  (altMonitor) {
			switch (state)
			{
				case enabling:
				state = ready;
				break;
				case waiting:
				state = ready;
				altMonitor.notify();
				break;
				// case ready: case inactive:
				// break
			}
		}
	}


	/////////////////// The pre-conditioned versions of select ///////////////////


	/**
	 * Returns the index of one of the ready guards whose <code>preCondition</code>
	 * index is true. The method will block until one of these guards becomes
	 * ready.  If more than one is ready, an <I>arbitrary</I> choice is made.
	 * <P>
	 * <I>Note: the length of the </I><code>preCondition</code><I> array must be the
	 * same as that of the array of guards with which this object was constructed.</I>
	 * <P>
	 *
	 * @param preCondition the guards from which to select
	 */
	public /*readonly*/  int select(Boolean[] preCondition)
	{
		return fairSelect(preCondition);  // a legal implementation of arbitrary choice!
	}

	/**
	 * Returns the index of one of the ready guards whose <code>preCondition</code>
	 * index is true. The method will block until one of these guards becomes
	 * ready.  If more than one is ready, the one with the lowest index is selected.
	 * <P>
	 * <I>Note: the length of the </I><code>preCondition</code><I> array must be the
	 * same as that of the array of guards with which this object was constructed.</I>
	 * <P>
	 *
	 * @param preCondition the guards from which to select
	 */
	public /*readonly*/  int priSelect(Boolean[] preCondition)
	{
		// if (barrierPresent) {
		//   throw new AlternativeError (
		//     "*** Cannot 'priSelect' with an AltingBarrier in the Guard array"
		//   );
		// }
		if (preCondition.Length != guard.Length)
		{
			throw new IllegalArgumentException(
			  "*** jcsp.lang.Alternative.select called with a preCondition array\n" +
			  "*** whose length does not match its guard array"
			);
		}
		state = enabling;
		favourite = 0;
		enableGuards(preCondition);
		/*synchronized*/ lock  (altMonitor) {
			if (state == enabling)
			{
				state = waiting;
				try
				{
					if (timeout)
					{
						long delay = msecs - /*System.currentTimeMillis()*/ CurrentTimeMillis();
						if (delay > Spurious.earlyTimeout)
						{
							altMonitor.wait(delay);
							while (state == waiting)
							{
								delay = msecs - /*System.currentTimeMillis()*/ CurrentTimeMillis();
								if (delay > Spurious.earlyTimeout)
								{
									if (Spurious.logging)
									{
										SpuriousLog.record(SpuriousLog.AlternativeSelectWithTimeout);
									}
									altMonitor.wait(delay);
								}
								else
								{
									if ((delay > 0) && (Spurious.logging))
									{
										SpuriousLog.incEarlyTimeouts();
									}
									break;
								}
							}
						}
					}
					else
					{
						altMonitor.wait();
						while (state == waiting)
						{
							if (Spurious.logging)
							{
								SpuriousLog.record(SpuriousLog.AlternativeSelect);
							}
							altMonitor.wait();
						}
					}
				}
				catch (InterruptedException e)
				{
					throw new ProcessInterruptedException(
				  "*** Thrown from Alternative.priSelect (Boolean[])\n" + e.toString()
				);
				}
				state = ready;
			}
		}
		disableGuards(preCondition);
		state = inactive;
		timeout = false;
		return selected;
	}

	/**
	 * Returns the index of one of the ready guards whose <code>preCondition</code> index
	 * is true. The method will block until one of these guards becomes ready.
	 * Consequetive invocations will service the guards `fairly' in the case
	 * when many guards are always ready.  <I>Implementation note: the last
	 * guard serviced has the lowest priority next time around.</I>
	 * <P>
	 * <I>Note: the length of the </I><code>preCondition</code><I> array must be the
	 * same as that of the array of guards with which this object was constructed.</I>
	 * <P>
	 *
	 * @param preCondition the guards from which to select
	 */
	public /*readonly*/  int fairSelect(Boolean[] preCondition)
	{
		if (preCondition.Length != guard.Length)
		{
			throw new IllegalArgumentException(
			  "*** jcsp.lang.Alternative.select called with a preCondition array\n" +
			  "*** whose length does not match its guard array"
			);
		}
		state = enabling;
		enableGuards(preCondition);
		/*/*synchronized*/ lock */ lock (altMonitor) {
			if (state == enabling)
			{
				state = waiting;
				try
				{
					if (timeout)
					{
						long delay = msecs - /*System.currentTimeMillis()*/ CurrentTimeMillis();
						if (delay > Spurious.earlyTimeout)
						{
							altMonitor.wait(delay);
							while (state == waiting)
							{
								delay = msecs - /*System.currentTimeMillis()*/ CurrentTimeMillis();
								if (delay > Spurious.earlyTimeout)
								{
									if (Spurious.logging)
									{
										SpuriousLog.record(SpuriousLog.AlternativeSelectWithTimeout);
									}
									altMonitor.wait(delay);
								}
								else
								{
									if ((delay > 0) && (Spurious.logging))
									{
										SpuriousLog.incEarlyTimeouts();
									}
									break;
								}
							}
						}
					}
					else
					{
						altMonitor.wait();
						while (state == waiting)
						{
							if (Spurious.logging)
							{
								SpuriousLog.record(SpuriousLog.AlternativeSelect);
							}
							altMonitor.wait();
						}
					}
				}
				catch (InterruptedException e)
				{
					throw new ProcessInterruptedException(
				  "*** Thrown from Alternative.fairSelect/select (Boolean[])\n" + e.toString()
					);
				}
				state = ready;
			}
		}
		disableGuards(preCondition);
		state = inactive;
		favourite = selected + 1;
		if (favourite == guard.Length)
			favourite = 0;
		timeout = false;
		return selected;
	}

	/**
	 * Enables the guards for selection.  The preCondition must be true for
	 * an guard to be selectable.  If any of the guards are ready, it sets
	 * selected to the ready guard's index, state to ready and returns.
	 * Otherwise, it sets selected to NONE_SELECTED and returns.
	 * <P>
	 *
	 * @return true if and only if one of the guards is ready
	 */
	private /*readonly*/  void enableGuards(Boolean[] preCondition)
	{
		if (barrierPresent)
		{
			AltingBarrierCoordinate.startEnable();
		}
		barrierSelected = NONE_SELECTED;
		for (enableIndex = favourite; enableIndex < guard.Length; enableIndex++)
		{
			if (preCondition[enableIndex] && guard[enableIndex].enable(this))
			{
				selected = enableIndex;
				state = ready;
				if (barrierTrigger)
				{
					barrierSelected = selected;
					barrierTrigger = false;
				}
				else if (barrierPresent)
				{
					AltingBarrierCoordinate.finishEnable();
				}
				return;
			}
		}
		for (enableIndex = 0; enableIndex < favourite; enableIndex++)
		{
			if (preCondition[enableIndex] && guard[enableIndex].enable(this))
			{
				selected = enableIndex;
				state = ready;
				if (barrierTrigger)
				{
					barrierSelected = selected;
					barrierTrigger = false;
				}
				else if (barrierPresent)
				{
					AltingBarrierCoordinate.finishEnable();
				}
				return;
			}
		}
		selected = NONE_SELECTED;
		if (barrierPresent)
		{
			AltingBarrierCoordinate.finishEnable();
		}
	}

	/**
	 * Disables the guards for selection.  The preCondition must be true for
	 * an guard to be selectable.  Sets selected to the index of the ready guard,
	 * taking care of priority/fair choice.
	 */
	private void disableGuards(Boolean[] preCondition)
	{
		if (selected != favourite)
		{    // else there is nothing to disable
			int startIndex = (selected == NONE_SELECTED) ? favourite - 1 : selected - 1;
			if (startIndex < favourite)
			{
				for (int i = startIndex; i >= 0; i--)
				{
					if (preCondition[i] && guard[i].disable())
					{
						selected = i;
						if (barrierTrigger)
						{
							if (barrierSelected != NONE_SELECTED)
							{
								throw new JCSP_InternalError(
							  "*** Second AltingBarrier completed in ALT sequence: " +
							  barrierSelected + " and " + i
							);
							}
							barrierSelected = selected;
							barrierTrigger = false;
						}
					}
				}
				startIndex = guard.Length - 1;
			}
			for (int i = startIndex; i >= favourite; i--)
			{
				if (preCondition[i] && guard[i].disable())
				{
					selected = i;
					if (barrierTrigger)
					{
						if (barrierSelected != NONE_SELECTED)
						{
							throw new JCSP_InternalError(
							  "*** Second AltingBarrier completed in ALT sequence: " +
						  barrierSelected + " and " + i
							);
						}
						barrierSelected = selected;
						barrierTrigger = false;
					}
				}
			}
			if (selected == NONE_SELECTED)
			{
				// System.out.println ("disableGuards: NONE_SELECTED ==> " + timeIndex);
				// NOTE: this is a work-around for Java wait-with-timeouts sometimes
				// returning early.  If this did not happen, we would not get here!
				selected = timeIndex;
			}
		}
		if (barrierSelected != NONE_SELECTED)
		{        // We must choose a barrier sync
			selected = barrierSelected;                  // if one is ready - so that all
			AltingBarrierCoordinate.finishDisable();    // parties make the same choice.
		}
	}

}
}
