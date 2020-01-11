using System;
using CSPlang;

namespace ConsumerProducer
{
    public class RunConsumerProducer
    {
        static void Main(string[] args)
        {
            var connect = Channel.one2one();
            IamCSProcess[] processList = { new Producer(connect.Out()), new Consumer(connect.In()) };

            new CSPParallel(processList).run();

            Console.ReadKey();
        }
    }
}