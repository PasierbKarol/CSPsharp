using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang.One2
{
    class One2AnyChannelImpl : One2AnyImpl
    {
        public One2AnyChannelImpl() : base(new One2OneChannelImpl())
        {
        }

    }
}
