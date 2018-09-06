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

using System.Threading;

namespace CSPlang
{

    /**
     * This is an extension of the {@link Parallel} class that prioritises
     * the processes given to its control.
     * <H2>Description</H2>
     * <TT>PriParallel</TT> is an extension of the {@link Parallel} class that prioritises
     * the processes given to its control.
     * The ordering of the processes in
     * the array passed to the constructor (or added/inserted later) is significant,
     * with earlier processes having higher priority.  The last process in the
     * array inherits the priority of the constructing process.  That priority may
     * be set explicitly by {@link #setPriority <TT>setPriority</TT>}.
     * <P>
     * <I>Implementation Note</I>: these priorities are currently implemented
     * using the underlying threads priority mechanism.  If there are more
     * priorities required than the maximum allowed for the threadgroup of
     * the spawning process, the higher requested priorities will be truncated
     * to that maximum.  Also, the semantics of priority will be that implemented
     * by the JVM being used.
     *
     * @author P.D.Austin
     */

    public class PriParallel : CSPParallel
    {
    /**
     * Construct a new PriParallel object initially without any processes.
     * Processes may be added later using the inherited addProcess methods.
     * The order of their adding is significant, with ealier processes
     * having higher priority.
     */
    public PriParallel() : base (null, true)
    {
        
    }

/**
 * Construct a new PriParallel object with the processes specified.
 * The ordering of the processes in the array is significant, with
 * ealier processes having higher priority.  The last process in the
 * array inherits the priority of the constructing process.
 *
 * @param processes The processes to be executed in parallel
 */
    public PriParallel(IamCSProcess[] processes) : base(processes, true)
    {
        
    }

/**
 * Insert another process to the pri-parallel object at the specifed
 * index.  The point of insertion is significant because the ordering of
 * process components determines the priorities.  The extended network
 * will be executed the next time run() is invoked.
 * <P>
 * @param process the process to be inserted
 * @param index the index at which to insert the process
 */
    public void insertProcessAt(IamCSProcess process, int index)
    {
        base .insertProcessAt(process, index);
    }

/**
 * This returns the current priority of this process.
 * <P>
 * @return the current priority of this process.
 */
    public static int getPriority()
    {
        return Thread.currentThread().getPriority();
    }

/**
 * This changes the priority of this process.  Note that JCSP only provides
 * this method for changing the priority of the <I>invoking</I> process.
 * Changing the process of <I>another</I> process is not considered wise.
 * <P>
 * <I>Implementation Note</I>: these priorities are currently implemented
 * using the underlying threads priority mechanism - hence run time
 * exceptions corresponding to the {@link java.lang.Thread}.<TT>getPriority()</TT>
 * may be thrown.
 * <P>
 * @throws <TT>java.lang.IllegalArgumentException</TT> if the priority is not
 *   in the range supported by the underlying threads implementation.
 * @throws <TT>java.lang.SecurityException</TT> if the security manager of
 *   the underlying threads implementation will not allow this modification.
 */
    public static void setPriority(int newPriority)
    {
        Thread.currentThread().setPriority(newPriority);
    }
    }
}