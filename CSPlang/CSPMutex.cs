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

        public void Claim()
        {
            lock (this)
            {
                while (claimed)
                {
                    try
                    {
                        Monitor.Wait(this);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        throw new ProcessInterruptedException(
                            "*** Thrown from CSPMutex.Claim()\n" + e.ToString()
                        );
                    }
                }
                claimed = true;
            }
        }

        public void Release()
        {
            lock (this)
            {
                claimed = false;
                Monitor.Pulse(this);
            }
        }
    }
}