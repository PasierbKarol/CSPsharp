using System;
using CSPlang;

namespace PlugAndPlay
{
    public class Numbers : IamCSProcess
    {
    private ChannelOutput Out;
  
    public Numbers(ChannelOutput Out)
    {
        this.Out = Out;
    }




    public void run()
    {
        One2OneChannel a = Channel.createOne2One();
        One2OneChannel b = Channel.createOne2One();
        One2OneChannel c = Channel.createOne2One();

        new CSPParallel(
                new IamCSProcess[] {
                    new Delta2(a.In(), b.Out(), Out),
                    new Successor(b.In(), c.Out()),
                    new Prefix(0, c.In(), a.Out()) })
            .run();
    }
    }


}