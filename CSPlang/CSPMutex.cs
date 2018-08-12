using System;
using System.Threading;

namespace CSPlang
{

    /**
     * A package-visible class that implements a straightforward mutex, for use by 
     * One2AnyChannel and Any2AnyChannel
     * 
     * @author nccb
     *
     */
    class CSPMutex
    {
        private Boolean claimed = false;
        private CSPMutex mutexToLock; //Added this variable to be used for the Monitor - KP

        public void claim()
        {
            lock (mutexToLock)
            { //changed this to mutexToLock - KP
                while (claimed)
                {
                    try
                    {
                        Monitor.Wait(mutexToLock);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        throw new ProcessInterruptedException(
                            "*** Thrown from CSPMutex.claim()\n" + e.ToString()
                        );
                    }
                }

                claimed = true;
            }
        }

        public void release()
        {
            lock (mutexToLock)
            { //changed this to mutexToLock - KP
                claimed = false;
                Monitor.Pulse(mutexToLock);
            }
        }
    }
}