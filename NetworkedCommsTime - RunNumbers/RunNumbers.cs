using System;
using CSPlang;
using PlugAndPlay.Ints;

namespace NetworkedCommsTime___RunNumbers
{
    class RunNumbers
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Numbers started!");

            int nLoops = 10000;
            Console.WriteLine(nLoops + " loops ...\n");

            One2OneChannelInt P2D = Channel.one2oneInt();
            One2OneChannelInt D2S = Channel.one2oneInt();
            One2OneChannelInt S2P = Channel.one2oneInt();
            One2OneChannelInt D2C = Channel.one2oneInt();

            new CSPParallel(
                new IamCSProcess[] {
                    new PrefixInt (0, S2P.In(), P2D.Out()),
                    new Delta2Int (P2D.In(), D2C.Out(), D2S.Out()),
                    new SuccessorInt (D2S.In(), S2P.Out()),
                }
            ).run();
        }
    }
}
