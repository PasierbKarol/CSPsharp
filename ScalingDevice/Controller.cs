﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Console.WriteLine("Controller: current factor " + currentFactor);
            Console.WriteLine("Controller: test interval" + testInterval);
            Console.WriteLine("Controller: compute intervcal " + computeInterval);
            Console.WriteLine("Controller: timeout " + timeout);

            while (true)
            {
                timeout = timeout + testInterval;
                timer.after(timeout);
                Debug.WriteLine("Controller splept " + timeout);
                suspend.write(0);
                currentFactor = (int)factor.read();
                Debug.WriteLine("Controller currentFactor " + currentFactor);
                currentFactor = currentFactor + addition;
                Debug.WriteLine("Controller currentFactor + addition " + currentFactor);
                timer.sleep(computeInterval);
                Debug.WriteLine("Controller splept " + computeInterval);

                injector.write(currentFactor);
                Debug.WriteLine("Controller wrote to Injector ");

            }
        }
    }
}
