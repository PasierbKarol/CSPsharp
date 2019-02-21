using System;
using System.Collections.Generic;
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
        private readonly PrintWriter logger;

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
        Logger(OutputStream stream)
        {
            this.logger = new PrintWriter(stream);
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
                this.logger.println("(" + date.ToString() + ")-" + clazz.GetType().Name + ":");
                this.logger.println("\t\"" + message + "\"");
                this.logger.flush();
            }
            catch (Exception e)
            {
                // Do nothing
            }
        }
    }
}
