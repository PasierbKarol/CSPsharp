using System;
using CSPlang;


namespace CSPutil
{

    /**
     * 
     * @deprecated Use poison directly instead
     */
    public class PoisonFilterException : PoisonException
    {

        //In lieu of knowing a specific poison strength,
        //we supply the maximum:
        public PoisonFilterException(String message) : base (Int32.MaxValue)
        {


        }
    }
}