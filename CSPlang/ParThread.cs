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

namespace CSPlang
{

    /**
     * This is the <TT>Thread</TT> class used by {@link CSPParallel} to run all but
     * one of its given processes.
     *
     * <H2>Description</H2>
     * A <TT>ParThread</TT> is a <TT>Thread</TT> used by {@link CSPParallel} to run
     * all but one of its given processes.
     * <P>
     * The <TT>IamCSProcess</TT> to be executed can be changed using the
     * <TT>setProcess</TT> method providing the <TT>ParThread</TT> is not active.
     *
     * @see jcsp.lang.IamCSProcess
     * @see jcsp.lang.ProcessManager
     * @see jcsp.lang.CSPParallel
     *
     * @author P.D.Austin
     * @author P.H.Welch
     */
    //}}}

    class ParThread : Thread
    {
    /** the process to be executed */
    private IamCSProcess process;

/** the barrier at the end of a PAR */
    private Barrier barrier;

    private Boolean running = true;

/** parking barrier for this thread */
    private Barrier park = new Barrier(2);

/**
 * Construct a new ParThread.
 *
 * @param process the process to be executed
 * @param barrier the barrier for then end of the PAR
 */
    public ParThread(IamCSProcess process, Barrier barrier)
    {
        setDaemon(true);
        this.process = process;
        this.barrier = barrier;
        setName(process.ToString());
    }

/**
 * reset the ParThread.
 *
 * @param process the process to be executed
 * @param barrier the barrier for then end of the PAR
 */
    public void reset(IamCSProcess process, Barrier barrier)
    {
        this.process = process;
        this.barrier = barrier;
        setName(process.ToString());
    }

/**
 * Sets the ParThread to terminate next time it's unparked.
 *
 */
    public void terminate()
    {
        running = false;
        park.sync();
    }

/**
 * Releases the ParThread to do some more work.
 */
    public void release()
    {
        park.sync();
    }

/**
 * The main body of this process.
 * above.
 */
    public void run()
    {
        try
        {
            CSPParallel.addToAllParThreads(this);
            while (running)
            {
                try
                {
                    process.run();
                }
                catch (Throwable e)
                {
                    CSPParallel.uncaughtException("jcsp.lang.CSPParallel", e);
                }

                barrier.resign();
                park.sync();
            }
        }
        catch (Throwable t)
        {
            CSPParallel.uncaughtException("jcsp.lang.CSPParallel", t);
        }
        finally
        {
            CSPParallel.removeFromAllParThreads(this);
        }
    }
    }
}