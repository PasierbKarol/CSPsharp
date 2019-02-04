using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSPlang;

namespace AltingBarrierTesting
{
    public class ProcessesArray : IamCSProcess
    {
        IamCSProcess[] processes;
        public ProcessesArray(int n, ChannelInput[] inputs, ChannelOutput[] outputs)
        {
            processes = new SingleProcess[n];
            for (int i = 0; i < n; i++)
            {
                processes[i] = new SingleProcess(inputs[i], outputs[i], i+1);
            }
        }


        public void run()
        {
            new CSPParallel(processes).run();
        }
    }
}
