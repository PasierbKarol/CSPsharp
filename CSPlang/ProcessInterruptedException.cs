﻿/*************************************************************************
*                                                                        *
*  JCSP ("CSP for Java") libraries                                       *
*  Copyright (C) 1996-2006 Peter Welch and Paul Austin.                  *
*                                                                        *
*  This library is free software; you can redistribute it and/or         *
*  modify it under the terms of the GNU Lesser General Public            *
*  License as published by the Free Software Foundation; either          *
*  version 2.1 of the License, or (at your option) any later version.    *
*                                                                        *
*  This library is distributed in the hope that it will be useful,       *
*  but WITHOUT ANY WARRANTY; without even the implied warranty of        *
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU     *
*  Lesser General Public License for more details.                       *
*                                                                        *
*  You should have received a copy of the GNU Lesser General Public      *
*  License along with this library; if not, write to the Free Software   *
*  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  *
*  USA.                                                                  *
*                                                                        *
*  Author contact: P.H.Welch@ukc.ac.uk                                   *
*                                                                        *
*************************************************************************/

using System;

namespace CSPlang
{
    /**
     * This is thrown if a process is interrupted whilst blocked during synchronisation
     * - processes should never be interrupted.
     *
     * <H2>Description</H2>
     * This is caused by accessing the Java thread executing a JCSP process and invoking its
     * <TT>java.lang.Thread.interrupt</TT>() method.
     * If this is done to a process blocked on a JCSP synchronisation primitive (such as
     * a channel communication or timeout), the process will wake up prematurely
     * -- invalidating the semantics of that primitive.
     * The wake up is intercepted and this {@link java.lang.Error} is thrown.
     * <P>
     * Some browsers, when shutting down an <I>applet</I>, may do this to processes
     * spawned by an {@link jcsp.awt.ActiveApplet} that have not died naturally.
     *
     * Alternatively, this may be raised by processes stopped prematurely as a result of
     * a call to <TT>Parallel.destroy</TT>, or by calling <TT>stop</TT> on the
     * <TT>ProcessManager</TT> responsible for the process (or network).
     *
     * @author P.H.Welch
     */

    //TODO Is it needed? How does it work and could it be changed to be useful?
    public class ProcessInterruptedException : /*Error*/ Exception
    {
        private static String message = "\n*** Interrupting a running process is not compatible with JCSP\n" +
                                        "*** Please don't do this!\n";

        public ProcessInterruptedException(String s) : base(message + s)
        {
            //TODO throw new NotImplementedException();
        }
    }
}