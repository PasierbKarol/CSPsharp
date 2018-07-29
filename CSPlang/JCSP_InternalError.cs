using System;

namespace CSPlang
{
    public class JCSP_InternalError : Exception
    {

        private static string report = "\n*** Please report the circumstances to jcsp-team@kent.ac.uk - thanks!";
        public JCSP_InternalError(String s) : base(s + report)
        {
            
        }
    }
}