using System;
using System.Diagnostics;
using CSPlang;

namespace PlugAndPlay
{
    public class ProcessWrite : IamCSProcess
    {
        public Object value;
        private ChannelOutput Out;

        public ProcessWrite(ChannelOutput Out)
        {
            this.Out = Out;
        }

        public void run()
        {
            Out.write(value);
        }
    }
}