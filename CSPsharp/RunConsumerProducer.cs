using System;
using System.Collections.Generic;
using System.Text;
using CSPlang;
using CSPutil;

namespace ConsumerProducer
{
    public class RunConsumerProducer
    {
        static void Main(string[] args)
        {
            var connect = Channel.one2oneInt();
            IamCSProcess[] processList = {new Producer(connect.Out()), new Consumer(connect.In())};

            new CSPParallel(processList).run();
            

            Console.ReadKey();

        }
    }
}
