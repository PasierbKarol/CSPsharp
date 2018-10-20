using System;
using System.Collections.Generic;
using System.Text;
using CSPlang;

namespace ScalingDevice
{
    class Scale : IamCSProcess
    {
        int scaling = 2;
        int multiplier = 2;

        ChannelOutput outChannel;
        ChannelOutput factor;
        ChannelInput inChannel;
        ChannelInput suspend;
        ChannelInput injector;

        public Scale( ChannelInput inChannel,ChannelOutput outChannel, ChannelOutput factor, ChannelInput suspend, ChannelInput injector,int multiplier, int scaling)
        {
            this.outChannel = outChannel;
            this.factor = factor;
            this.inChannel = inChannel;
            this.suspend = suspend;
            this.injector = injector;
            this.multiplier = multiplier;
            this.scaling = scaling;
        }

        public void run()
        {
            int SECOND = 1000;
            int DOUBLE_INTERVAL = 5 * SECOND;
            const int NORMAL_SUSPEND = 0;
            const int NORMAL_TIMER = 1;
            const int NORMAL_IN = 2;
            const int SUSPENDED_INJECT = 0;
            const int SUSPENDED_IN = 1;
            var timer = new CSTimer();
            Guard[] guardsNormal = {suspend as Guard, timer, inChannel as Guard};
            Guard[] guardsSuspended = {injector as Guard, inChannel as Guard};
            List<IamCSProcess> processes = new List<IamCSProcess>{};
            var normalAlt = new Alternative(guardsNormal);
            var suspendedAlt = new Alternative(guardsSuspended);
            var timeout = timer.read() + DOUBLE_INTERVAL;
            timer.setAlarm(timeout);
            int inValue;
            ScaledData result;

            while (true)
            {
                switch (normalAlt.priSelect())
                {

                    case NORMAL_SUSPEND:
                        suspend.read();         // its a signal, no data content;
                        factor.write(scaling);   //reply with current value of scaling;
                        bool suspended = true;
                        Console.WriteLine("Suspended");
                        while (suspended)
                        {

                            switch (suspendedAlt.priSelect())
                            {

                                case SUSPENDED_INJECT:
                                    scaling = (int)injector.read();   //this is the resume signal as well;
                                    Console.WriteLine("Injected scaling is " + scaling);
                                    suspended = false;
                                    timeout = timer.read() + DOUBLE_INTERVAL;
                                    timer.setAlarm(timeout);
                                    break;
                

                                case SUSPENDED_IN:
                                    inValue = (int)inChannel.read();
                                    result = new ScaledData();
                                    result.original = inValue;
                                    result.scaled = inValue;
                                    outChannel.write(result);
                                    break;
                            }  // end-switch
                        } //end-while
                        break;


                    case NORMAL_TIMER:
                        timeout = timer.read() + DOUBLE_INTERVAL;
                        timer.setAlarm(timeout);
                        scaling = scaling * multiplier;
                        Console.WriteLine("Normal Timer: new scaling is " + scaling);
                        break;


                    case NORMAL_IN:
                        inValue = (int)inChannel.read();
                        result = new ScaledData();
                        result.original = inValue;
                        result.scaled = inValue * scaling;
                        outChannel.write(result);
                        break;


                } //end-switch
            }
        }
    }
}
