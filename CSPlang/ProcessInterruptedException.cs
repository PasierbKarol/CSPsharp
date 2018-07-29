using System;

namespace CSPlang
{
    public class ProcessInterruptedException : /*Error*/ Exception
    {
        private static String message = "\n*** Interrupting a running process is not compatible with JCSP\n" +
                                        "*** Please don't do this!\n";

        public ProcessInterruptedException(String s) : base(message + s)
        {
            //throw new NotImplementedException();
        }


    }
}