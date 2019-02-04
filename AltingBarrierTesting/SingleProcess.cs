using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CSPlang;

namespace AltingBarrierTesting
{
    public class SingleProcess : IamCSProcess
    {
        ChannelInput input;
        ChannelOutput output;
        int n;
        long timeout = 0;


        public SingleProcess(ChannelInput input, ChannelOutput output, int n)
        {
            this.input = input;
            this.output = output;
            this.n = n;
            //this.timeout = timeout;
            
        }

        public void run()
        {
            CSTimer timer = new CSTimer();
            Random rnd = new Random();
            int randomTimeout = rnd.Next(1, 10) * n * 500;
            timeout = timer.read() + randomTimeout;
            Debug.WriteLine(n + "Process timeout " + randomTimeout);

            Object x = input.read();
            Console.WriteLine(n + "Read value" + x);
            while (true)
            {     
                 x = input.read();
                    Console.WriteLine(n +"Read value" + x);

                //output.write(x);
                long currentTime = timer.read();
                if (timeout <= currentTime) //if timeout should occur now
                {
                    
                    timeout = timer.read() + rnd.Next(1, 10)  * 500;
                    Debug.WriteLine(n+"Process timout ppassed " + randomTimeout);
                    output.write(n+"For Input guard" + x + "0000000");
                    timer.after(timeout);
                    output.write(n+"Finished");
                    timeout = timer.read() + rnd.Next(1, 10) * n * 500;

                }

            }
        }
    }
}
