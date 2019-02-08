using System;
using System.Collections.Generic;
using System.Text;
using CSPlang;
using TestingUtilities;

namespace StressedAlt_PerformanceTesting
{
    public class StressedWriterPerformance : IamCSProcess
    {
        private readonly ChannelOutput Out;
        private readonly int channel;
        private readonly int idWriter;

        public StressedWriterPerformance(ChannelOutput Out, int channel, int idWriter)
        {
            this.Out = Out;
            this.channel = channel;
            this.idWriter = idWriter;
        }

        public void run()
        {
            int n = 0;
            StressedPacket stressedPacketA = new StressedPacket(idWriter);
            StressedPacket stressedPacketB = new StressedPacket(idWriter);
            while (true)
            {
                // for (int i = 0; i < idWriter; i++) System.out.print ("  ");
                // System.out.println (id + " " + n);
                stressedPacketA.n = idWriter;
                Out.write(stressedPacketA);
                //n++;
                // for (int i = 0; i < idWriter; i++) System.out.print ("  ");
                // System.out.println (id + " " + n);
                stressedPacketB.n = idWriter;
                Out.write(stressedPacketB);
                //n++;
            }
        }
    }
}
