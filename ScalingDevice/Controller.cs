using System;
using System.Collections.Generic;
using System.Text;
using CSPlang;

namespace ScalingDevice
{
    class Controller : IamCSProcess
    {
        long testInterval = 11000;
        long computeInterval = 2000;
        int addition = 1;
        ChannelInput factor;
        ChannelOutput suspend;
        ChannelOutput injector;


        public Controller(long testInterval, long computeInterval, int addition, ChannelInput factor, ChannelOutput suspend, ChannelOutput injector)
        {
            this.testInterval = testInterval;
            this.computeInterval = computeInterval;
            this.addition = addition;
            this.factor = factor;
            this.suspend = suspend;
            this.injector = injector;
        }

        public void run()
        {
            int currentFactor = 0;
            var timer = new CSTimer();
            var timeout = timer.read();

            while (true)
            {
                timeout = timeout + testInterval;
                timer.after(timeout);
                suspend.write(0);
                currentFactor = (int)factor.read();
                currentFactor = currentFactor + addition;
                timer.sleep(computeInterval);
                injector.write(currentFactor);
            }
        }
    }
}
