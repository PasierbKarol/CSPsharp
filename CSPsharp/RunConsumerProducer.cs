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
            //Console.WriteLine("Hello World!");

            var connect = Channel.one2one();
            //List<Object> processList = new List<Object> {new Producer(connect.Out()), new Consumer(connect.In())};
            IamCSProcess[] processList = new IamCSProcess[] {new Producer(connect.Out()), new Consumer(connect.In())};

            CSPParallel PAR = new CSPParallel(processList);
            PAR.run();

            Console.ReadKey();

        }
    }
}
