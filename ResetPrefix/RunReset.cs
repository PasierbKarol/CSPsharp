using System;
using CSPlang;
using PlugAndPlay;

namespace ResetPrefix
{
    class RunReset
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            One2OneChannel RU2RN = Channel.one2one();

            One2OneChannel RN2Conv = Channel.one2one();
            One2OneChannel Conv2FD = Channel.one2one();
            One2OneChannel FD2GC = Channel.one2one();

            IamCSProcess[] RNprocList =
            {
                new ResetNumbers(resetChannel: RU2RN.In(),
                    initialValue: 1000,
                    outChannel: RN2Conv.Out()),
                new GObjectToConsoleString(inChannel: RN2Conv.In(),
                    outChannel: Conv2FD.Out()),
                new FixedDelay(delayTime: 200,
                    In: Conv2FD.In(),
                    Out: FD2GC.Out()),
                //new GConsole(toConsole: FD2GC.In(),
                //    frameLabel: "Reset Numbers Console")
            };

            One2OneChannel RU2GC = Channel.one2one();
            One2OneChannel GC2Conv = Channel.one2one();
            One2OneChannel Conv2RU = Channel.one2one();
            One2OneChannel RU2GCClear = Channel.one2one();

            IamCSProcess[] RUprocList =
            {
                new ResetUser(resetValue: RU2RN.Out(),
                    toConsole: RU2GC.Out(),
                    fromConverter: Conv2RU.In(),
                    toClearOutput: RU2GCClear.Out()),
                new GConsoleStringToInteger(inChannel: GC2Conv.In(),
                    outChannel: Conv2RU.Out()),
                //new GConsole(toConsole: RU2GC.In(),
                //    fromConsole: GC2Conv.Out(),
                //    clearInputArea: RU2GCClear.In(),
                //    frameLabel: "Reset Value Generator")
            };

            //new CSPParallel(RNprocList + RUprocList).run();
            Console.ReadKey();
        }
    }
}