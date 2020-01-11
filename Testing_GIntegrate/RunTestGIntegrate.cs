using System;
using CSPlang;
using PlugAndPlay;

namespace Testing_GIntegrate
{
    class RunTestGIntegrate
    {
        static void Main(string[] args)
        {
            One2OneChannel N2I = Channel.one2one();
            One2OneChannel I2P = Channel.one2one();

            IamCSProcess[] testList =
            {
                new Numbers(outChannel: N2I.Out()),
                new Integrate(In: N2I.In(), Out: I2P.Out()),
                new GPrint(inChannel: I2P.In(), heading: "Integrate", delay: 1000)
            };

            new CSPParallel(testList).run();
        }
    }
}