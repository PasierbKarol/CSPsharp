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

namespace CSPlang
{

    /**
     *
     * 
     * TODO (NCCB) what is the reason for this class, over and above using a simple flat Alternative ?
     */
    public class InlineAlternative : Guard
    {
        /** Flags to control behaviour of this ALT if used as a guard. */
        public static readonly int MODE_ARBITRARY = 0, MODE_FAIR = 1, MODE_PRI = 2;

        /** If used as a top level ALT, the work will be delegated to this */
        private Alternative alt;

        /** Mode of the select */
        private int selectMode;

        /** Index last selected */
        private int selected;

        /** Index to favour (fair / pri) */
        private int favourite;

        /** The preconditions set for the ALT when it is being used as a guard. */
        private Boolean[] preconditions;

        /** The guards */
        private readonly Guard[] guard;

        /** The timer guards */
        private readonly CSTimer[] timers;

        /** Nested ALTs */
        private readonly InlineAlternative[] ialts;

        /** Timeout index */
        private int timeoutIndex;

        /** Shortest alarm set by a timer */
        private long minAlarm;

        /** Creates a new one */
        public InlineAlternative(Guard[] guards) : this(guards, MODE_ARBITRARY)
        {

        }

        /** Creates a new one */
        public InlineAlternative(Guard[] guards, int mode) : base()
        {

            guard = guards;
            selectMode = mode;
            timers = new CSTimer[guards.Length];
            ialts = new InlineAlternative[guards.Length];
            for (int i = 0; i < guards.Length; i++)
                if (guards[i] is CSTimer)
                    timers[i] = (CSTimer)guards[i];
                else if (guards[i] is InlineAlternative)
                    ialts[i] = (InlineAlternative)guards[i];
        }

        /**
         * Returns the index of the guard obtained by a call to select() or if this guard became ready within its parent ALT.
         */
        public int getSelected()
        {
            if (selectMode == MODE_FAIR)
            {
                favourite = selected + 1;
                if (favourite == guard.Length)
                    favourite = 0;
            }

            return selected;
        }

        /**
         * Establishes a precondition array that will be used by default in calls to select(). This is useful when the ALT
         * is used as a guard within another ALT.
         */
        public void setPreconditions(Boolean[] precons)
        {
            preconditions = precons;
        }

        /**
         * Alters the precondition on a guard.
         */
        public void setPreconditionByIndex(int index, Boolean on)
        {
            if (preconditions == null)
            {
                preconditions = new Boolean[guard.Length];
                for (int i = 0; i < preconditions.Length; i++)
                    preconditions[i] = true;
            }

            preconditions[index] = on;
        }

        /**
         * Returns the actual guard object corresponding to the selected guard. For example it can return the channel
         * or the ALT object.
         */
        public Guard getSelectedGuard()
        {
            return guard[selected];
        }

        /**
         * Returns the guard object at a given index. For example to obtain a channel or ALT object.
         */
        public Guard getGuardByIndex(int index)
        {
            return guard[index];
        }

        /**
         * Creates an Alternative (if needed) and delegates the call to it.
         */
        public int select()
        {
            if (alt == null)
                alt = new Alternative(guard);
            if (preconditions != null)
                return alt.select(preconditions);
            else
                return alt.select();
        }

        /**
         * Creates an Alternative (if needed) and delegates the call to it.
         */
        public int priSelect()
        {
            if (alt == null)
                alt = new Alternative(guard);
            if (preconditions != null)
                return alt.priSelect(preconditions);
            else
                return alt.priSelect();
        }

        /**
         * Creates an Alternative (if needed) and delegates the call to it.
         */
        public int fairSelect()
        {
            if (alt == null)
                alt = new Alternative(guard);
            if (preconditions != null)
                return alt.fairSelect(preconditions);
            else
                return alt.fairSelect();
        }

        /**
         * Enable this ALT as a guard within its parent ALT. This will enable all of its guards.
         */
        public override Boolean enable(Alternative alt)
        {
            timeoutIndex = -1;
            for (int i = favourite; i < guard.Length; i++)
            {
                if ((preconditions == null) || (preconditions[i]))
                {
                    if (guard[i].enable(alt))
                    {
                        selected = i;
                        return true;
                    }

                    if (timers[i] != null)
                    {
                        if (timeoutIndex < 0)
                        {
                            timeoutIndex = i;
                            minAlarm = timers[i].getAlarm();
                        }
                        else
                        {
                            long a = timers[i].getAlarm();
                            if (a < minAlarm)
                            {
                                timeoutIndex = i;
                                minAlarm = a;
                            }
                        }
                    }
                    else if (ialts[i] != null)
                    {
                        if (timeoutIndex < 0)
                        {
                            timeoutIndex = i;
                            minAlarm = ialts[i].minAlarm;
                        }
                        else
                        {
                            long a = ialts[i].minAlarm;
                            if (a < minAlarm)
                            {
                                timeoutIndex = i;
                                minAlarm = a;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < favourite; i++)
            {
                if ((preconditions == null) || (preconditions[i]))
                {
                    if (guard[i].enable(alt))
                    {
                        selected = i;
                        return true;
                    }

                    if (timers[i] != null)
                    {
                        if (timeoutIndex < 0)
                        {
                            timeoutIndex = i;
                            minAlarm = timers[i].getAlarm();
                        }
                        else
                        {
                            long a = timers[i].getAlarm();
                            if (a < minAlarm)
                            {
                                timeoutIndex = i;
                                minAlarm = a;
                            }
                        }
                    }
                    else if (ialts[i] != null)
                    {
                        if (timeoutIndex < 0)
                        {
                            timeoutIndex = i;
                            minAlarm = ialts[i].minAlarm;
                        }
                        else
                        {
                            long a = ialts[i].minAlarm;
                            if (a < minAlarm)
                            {
                                timeoutIndex = i;
                                minAlarm = a;
                            }
                        }
                    }
                }
            }

            selected = -1;
            return false;
        }

        /**
         * Disable this ALT as a guard within its parent ALT. This will disable all of its guards.
         */
        public override Boolean disable()
        {
            Boolean result = false;
            int startIndex;
            if (selected == -1)
                startIndex = favourite - 1;
            else
                startIndex = selected - 1;
            if (startIndex < favourite)
            {
                for (int i = startIndex; i >= 0; i--)
                {
                    if (((preconditions == null) || (preconditions[i])) && guard[i].disable())
                    {
                        result = true;
                        selected = i;
                    }
                }

                startIndex = guard.Length - 1;
            }

            for (int i = startIndex; i >= favourite; i--)
            {
                if (((preconditions == null) || (preconditions[i])) && guard[i].disable())
                {
                    result = true;
                    selected = i;
                }
            }

            if (selected == -1)
            {
                // We might be here because no guards were ready or because a timer
                // returned early. The workaround in the Alternative class will make
                // it OK to return FALSE. However it will probe down into the nested
                // ALTs so we must set 'selected' to refer to the timer, or point to
                // the nested ALT which contains the timer.
                selected = timeoutIndex;
            }

            return result;
        }
    }
}