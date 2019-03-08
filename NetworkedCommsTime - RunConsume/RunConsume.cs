using System;
using CommsTimeTesting;
using CSPlang;

namespace NetworkedCommsTime___RunConsume
{
    class RunConsume
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consume started!");

            int nLoops = 10000;
            Console.WriteLine(nLoops + " loops ...\n");

            One2OneChannelInt P2D = Channel.one2oneInt();
            One2OneChannelInt D2S = Channel.one2oneInt();
            One2OneChannelInt S2P = Channel.one2oneInt();
            One2OneChannelInt D2C = Channel.one2oneInt();

            new CSPParallel(
                new IamCSProcess[] {
                    new Consume (nLoops, D2C.In())
                }
            ).run();
        }
    }
}
