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
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

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

    public class ParThread //: Thread
    {


        /**
         * In C# Thread is a sealed class and cannot be inherit from.
         * However there is a thing called Composition.
         * Using this means this class (ParThread) is used as a new base class of Thread
         * Therefore this class requires additional fields and methods to accommodate such change
         * https://stackoverflow.com/questions/8123461/unable-to-inherit-from-a-thread-class-in-c-sharp/8123600#8123600
         */

        //============================= COMPOSITION METHODS AND FIELDS ====================
        private Thread _thread;

        public ParThread()
        {
            _thread = new Thread(run);

        }

        // Thread methods / properties
        public void Start() => _thread.Start();
        public void Join() => _thread.Join();
        public bool IsAlive => _thread.IsAlive;
        public void Interrupt() => _thread.Interrupt();
        public bool IsBackground
        {
            get => _thread.IsBackground;
            set => _thread.IsBackground = value;
        }
        public int Priority
        {
            get => (int)_thread.Priority;
            set => _thread.Priority = (ThreadPriority)value;
        }

        public void setPriority(int priority)
        {
            _thread.Priority = (ThreadPriority)priority;
        }



        //================================ END OF COMPOSITION ======================
        //================ THREAD GROUP =============
        //Using thread pool as a group

        //=============
        /** the process to be executed */
        private IamCSProcess process;

        private string name;

        /** the cspBarrier at the end of a PAR */
        private CSPBarrier _cspBarrier;

        private Boolean running = true;

        /** parking cspBarrier for this thread */
        private CSPBarrier park = new CSPBarrier(2);

        /**
         * Construct a new ParThread.
         *
         * @param process the process to be executed
         * @param cspBarrier the cspBarrier for then end of the PAR
         */
        public ParThread(IamCSProcess process, CSPBarrier cspBarrier) : this() //Added call to main constructor to run a thread - KP
        {
            //setDaemon(true);
            this.IsBackground = true;
            //Call to this reset method to avoid code duplication
            reset(process, cspBarrier);
            //this.process = process;
            this._cspBarrier = cspBarrier;
            //this.name = process.ToString();
            //setName(process.ToString());
        }

        /**
         * reset the ParThread.
         *
         * @param process the process to be executed
         * @param cspBarrier the cspBarrier for then end of the PAR
         */

        public void reset(IamCSProcess process, CSPBarrier cspBarrier)
        {
            this.process = process;
            this._cspBarrier = cspBarrier;
            //setName(process.ToString());
            this.name = process.ToString();
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
                        //_cspBarrier.enroll();
                    }
                    catch (Exception e)
                    {
                        CSPParallel.uncaughtException("jcsp.lang.CSPParallel", e);
                    }

                    //_cspBarrier.resign();
                    park.sync();
                }
            }
            catch (Exception t)
            {
                CSPParallel.uncaughtException("jcsp.lang.CSPParallel", t);
            }
            finally
            {
                CSPParallel.removeFromAllParThreads(this);
            }
        }


        //=========== Copied from Process manager
        //public void run()
        //{
        //    try
        //    {
        //        CSPParallel.addToAllParThreads(this);
        //        process.run();
        //    }
        //    catch (Exception e)
        //    {
        //        CSPParallel.uncaughtException("jcsp.lang.ProcessManager", e);
        //    }
        //    finally
        //    {
        //        CSPParallel.removeFromAllParThreads(this);
        //    }
        //}

    }
}