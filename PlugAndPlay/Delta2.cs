using System;
using System.Diagnostics;
using CSPlang;

namespace PlugAndPlay
{
    public sealed class Delta2 : IamCSProcess
    {
        private ChannelInput In;
        private ChannelOutput out1;
        private ChannelOutput out2;

        public Delta2(ChannelInput In, ChannelOutput Out1, ChannelOutput Out2)
        {
            this.In = In;
            this.out1 = Out1;
            this.out2 = Out2;
        }

        public void run()
        {
            ProcessWrite[] parWrite = { new ProcessWrite(out1), new ProcessWrite(out2) };
            CSPParallel par = new CSPParallel(parWrite);
            while (true)
            {
                Object value = In.read();

                parWrite[0].value = value;
                parWrite[1].value = value;
                par.run();
            }
        }
    }
}