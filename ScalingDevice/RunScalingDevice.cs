using System;
using CSPlang;
using PlugAndPlay;

namespace ScalingDevice
{
    public class RunScalingDevice
    {
        static void Main(string[] args)
        {
            One2OneChannel data = Channel.one2one();
            One2OneChannel timedData = Channel.one2one();
            One2OneChannel scaledData = Channel.one2one();
            One2OneChannel oldScale = Channel.one2one();
            One2OneChannel newScale = Channel.one2one();
            One2OneChannel pause = Channel.one2one();

            Numbers num = new Numbers(data.Out());

            FixedDelay fixedDelay =  new FixedDelay(1000,
                data.In(),
                timedData.Out());

            Scale scale =  new Scale(inChannel: timedData.In(),
            outChannel: scaledData.Out (),
            factor: oldScale.Out (),
            suspend: pause.In (),
            injector: newScale.In (),
            multiplier: 2,
            scaling: 2);

            Controller controller =  new Controller(testInterval: 11000,
                computeInterval: 3000,
                addition: -1,
                factor: oldScale.In (),
            suspend: pause.Out (),
            injector: newScale.Out ());

            GPrint gPrint =  new GPrint(inChannel: scaledData.In (),
            heading: "Original Scaled",
            delay: 0);

            IamCSProcess[] network = {num, fixedDelay, scale, controller, gPrint};

            new CSPParallel(network).run();

            Console.ReadKey();

        }
    }
}
