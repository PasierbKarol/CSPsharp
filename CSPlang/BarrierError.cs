using System;

namespace CSPlang
{
    public class BarrierError : Exception
    {
        public BarrierError(String errorMessage) : base (errorMessage)
        {
            
        }

    }
}