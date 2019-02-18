using System;
using System.Collections.Generic;
using System.Text;
using CSPlang.Any2;
using CSPlang.Shared;

namespace CSPlang
{
    public class ChannelOutputList
    {
        private List<ChannelOutput> channelOutputs;
        public ChannelOutputList(Object[] channelListArray)
        {
            channelOutputs = new List<ChannelOutput>();
            ChannelOutput[] outputEnds = new ChannelOutput[channelListArray.Length];

            var name = channelListArray.GetType().Name;
            name = name.Substring(0, name.Length-2);  //removed array symbols [] to use the channel type name in Switch - Karo Pasierb

            switch (name)
            {
                case nameof(Any2AnyChannel):
                    outputEnds = Channel.getOutputArray((Any2AnyChannel[])channelListArray);
                    break;
                case nameof(Any2OneChannel):
                    outputEnds = Channel.getOutputArray((Any2OneChannel[])channelListArray);
                    break;
                case nameof(One2AnyChannel):
                    outputEnds = Channel.getOutputArray((One2AnyChannel[])channelListArray);
                    break;
                case nameof(One2OneChannel):
                    outputEnds = Channel.getOutputArray((One2OneChannel[])channelListArray);
                    break;
            }

            for (int i = 0; i < channelListArray.Length; i++)
            {
                channelOutputs.Add(outputEnds[i]);
            }
        }

        public ChannelOutput this[int index]
        {
            get { return getChannelOuptutItem(index); }
        }

        private ChannelOutput getChannelOuptutItem(int index)
        {
            return channelOutputs[index];
        }
    }
}
