using System;
using CSPlang;

namespace PlugAndPlay
{
    public class Numbers : IamCSProcess
    {
    private ChannelOutput outChannel;
  
    public Numbers(ChannelOutput outChannel)
    {
        this.outChannel = outChannel;
    }




    public void run()
    {
        One2OneChannel a = Channel.createOne2One();
        One2OneChannel b = Channel.createOne2One();
        One2OneChannel c = Channel.createOne2One();

        new CSPParallel(
                new IamCSProcess[] {
                    new Delta2(a.In(), b.Out(), outChannel),
                    new Successor(b.In(), c.Out()),
                    new Prefix(0, c.In(), a.Out()) })
            .run();
    }
    }


}