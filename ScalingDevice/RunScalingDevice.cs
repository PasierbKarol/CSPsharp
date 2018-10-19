using System;
using CSPlang;

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




            Console.ReadKey();

        }
    }
}
