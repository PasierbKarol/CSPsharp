using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang
{
    public abstract class Guard
    {
        /**
     * Returns true if the event is ready.  Otherwise, this enables the guard
     * for selection and returns false.
     * <P>
     * <I>Note: this method should only be called by the Alternative class</I>
     *
     * @param alt the Alternative class that is controlling the selection
     * @return true if and only if the event is ready
     */
        protected abstract Boolean enable(Alternative alt);

        /**
         * Disables the guard for selection. Returns true if the event was ready.
         * <P>
         * <I>Note: this method should only be called by the Alternative class</I>
         *
         * @return true if and only if the event was ready
         */
        protected abstract Boolean disable();

        /**
         * Schedules the process performing the given Alternative to run again.
         * This is intended for use by advanced users of the library who want to
         * create their own Guards that are not in the jcsp.lang package.
         * 
         * @param alt The Alternative to schedule
         */
        protected void schedule(Alternative alt)
        {
            alt.schedule();
        }

    }
}
