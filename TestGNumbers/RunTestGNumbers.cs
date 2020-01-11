using System;
using CSPlang;
using PlugAndPlay;

namespace TestGNumbers
{
    class RunTestGNumbers
    {
        static void Main(string[] args)
        {
            One2OneChannel N2P = Channel.one2one();

            IamCSProcess[] testList =
            {
                new Numbers(outChannel: N2P.Out()),
                new GPrint(inChannel: N2P.In(), heading: "Numbers", delay: 1000)
            };

            new CSPParallel(testList).run();
        }
    }
}