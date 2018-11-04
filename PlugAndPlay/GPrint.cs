using System;
using System.Collections.Generic;
using System.Text;
using CSPlang;

namespace PlugAndPlay
{
    public sealed class GPrint : IamCSProcess
    {
        public ChannelInput inChannel { get; set; }
        public String heading { get; set; }
        public long delay { get; set; }

        public GPrint(ChannelInput inChannel, string heading, long delay)
        {
            this.inChannel = inChannel;
            this.heading = heading;
            this.delay = delay;
        }

        public GPrint(ChannelInput inChannel, string heading)
        {
            this.inChannel = inChannel;
            this.heading = heading;
        }

        public void run()
        {
            CSTimer timer = new CSTimer();
            while (true)
            {
                //Console.WriteLine("Inside GPrint timer");
                if (this.delay !=0)
                {
                    timer.after(delay);
                    Console.WriteLine(heading.Split(' ')[0] + " \t" + inChannel.read());
                }
            }
        
        }

}
}
