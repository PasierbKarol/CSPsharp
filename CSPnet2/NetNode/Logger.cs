using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CSPutil;

namespace CSPnet2.NetNode
{
    /**
     * @author Kevin Chalmers
     */
    public /*static*/ class Logger
    {
        /**
         * 
         */
        private readonly StreamWriter logger;

        /**
         * 
         */
        internal Logger()
        {
            this.logger = null;
        }

        /**
         * @param stream
         */
        internal Logger(StreamWriter stream)
        {
            this.logger = stream;
        }

        /**
         * @param clazz
         * @param message
         */
        public /*synchronized*/ void log(Type clazz, String message) //changed Java Class to Object
        {
            if (this.logger == null)
                return;
            DateTime date = new DateTime(CSPTimeMillis.CurrentTimeMillis());
            try
            {
                this.logger.WriteLine("(" + date.ToString() + ")-" + clazz.GetType().Name + ":");
                this.logger.WriteLine("\t\"" + message + "\"");
                this.logger.Flush();
            }
            catch (Exception e)
            {
                // Do nothing
            }
        }
    }
}
