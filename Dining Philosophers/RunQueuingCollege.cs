// copyright 2012-13 Jon Kerridge
// Let's Do It In Parallel

using System;
using System.Collections.Generic;
using CSPlang;
using CSPlang.Any2;

namespace Dining_Philosophers
{
    public class RunQueuingCollege
    {
        static void Main(string[] args)
        {
            Console.WriteLine("How many Philosophers?\t");
            int philosophers = Int32.Parse(Console.ReadLine());
            Any2OneChannel service = Channel.any2one();
            One2AnyChannel deliver = Channel.one2any();
            One2OneChannel supply = Channel.one2one();

            List<IamCSProcess> network = new List<IamCSProcess>();

            Philosopher[] philosopherList = new Philosopher[philosophers];
            for (int i = 0; i < philosophers; i++)
            {
                philosopherList.SetValue(new Philosopher(philosopherId: i,
                    service: service.Out(),
                    deliver:
                    deliver.In()), i);
                network.Add(philosopherList[i]);
            }



            network.Add(new QueuingServery(service: service.In(),
                deliver: deliver.Out(),
                supply: supply.In()));

            network.Add(new Kitchen(supply: supply.Out()));

            new CSPParallel(network.ToArray()).run();
        }
    }
}