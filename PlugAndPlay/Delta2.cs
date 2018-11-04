using System;
using System.Diagnostics;
using CSPlang;

namespace PlugAndPlay
{
    public sealed class Delta2 : IamCSProcess
    {
        private ChannelInput In;
        private ChannelOutput Out0;
        private ChannelOutput Out1;

        public Delta2(ChannelInput In, ChannelOutput out0, ChannelOutput out1)
        {
            this.In = In;
            this.Out0 = out0;
            this.Out1 = out1;
        }




        public void run()
        {
            ProcessWrite[] parWrite = { new ProcessWrite(Out0), new ProcessWrite(Out1) };
            CSPParallel par = new CSPParallel(parWrite);
            while (true)
            {
                Object value = In.read();
                //Debug.WriteLine("Delta2 read " + value.ToString());

                parWrite[0].value = value;
                parWrite[1].value = value;
                par.run();
            }
        }
    }
}