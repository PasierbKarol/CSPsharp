using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CSPlang;
using TestingUtilities;

namespace StressedAlt_PerformanceTesting
{
    public class StressedWriterPerformance : IamCSProcess
    {
        private readonly ChannelOutput Out;
        private readonly int channel;
        private readonly int writer;
        private readonly int writerID;

        public StressedWriterPerformance(ChannelOutput Out, int channel, int writer, int writerID)
        {
            this.Out = Out;
            this.channel = channel;
            this.writer = writer;
            this.writerID = writerID;
        }

        public void run()
        {
            StressedPacket stressedPacketA = new StressedPacket(writer);
            StressedPacket stressedPacketB = new StressedPacket(writer);
                stressedPacketA.n = writerID;
                Out.write(stressedPacketA);
                stressedPacketB.n = writerID;
                Out.write(stressedPacketB);
        }
    }
}
