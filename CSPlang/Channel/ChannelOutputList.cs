using System;
using System.Collections.Generic;
using System.Text;
using CSPlang.Any2;
using CSPlang.Shared;

namespace CSPlang
{
    public class ChannelOutputList
    {
        public List<ChannelOutput>channelOutputs{ get; set; }
        ChannelOutput[] beforeList;
        Object[] channelListArray;
        string[] channelType = {"Any2AnyChannel", "Any2OneChannel", "One2AnyChannel", "One2OneChannel" };

        public ChannelOutputList(Object[] channelListArray)
        {
            channelOutputs = new List<ChannelOutput>();
            ChannelOutput[] outputEnds = new ChannelOutput[channelListArray.Length];
            var name = channelListArray.GetType().Name;
            name = name.Substring(0, name.Length-2);
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

            //return channelOutputs;
        }

        public ChannelOutput this[int index]
        {
            get
            {
                // get the item for that index.
                return getChannelOuptutItem(index);
            }
            set
            {
                // set the item for this index. value will be of type Thing.
                //YourAddItemMethod(index, value);
            }
        }

        private ChannelOutput getChannelOuptutItem(int index)
        {
            return channelOutputs[index];
        }

        //private ChannelOutput setChannelOuptutItem(int index, value)
        //{
        //    return channelOutputs[index];
        //}
    }
}
