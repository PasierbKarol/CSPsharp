//////////////////////////////////////////////////////////////////////
//                                                                  //
//  JCSP ("CSP for Java") Libraries                                 //
// Copyright 1996-2017 Peter Welch, Paul Austin and Neil Brown      //
//           2005-2017 Kevin Chalmers and Jon Kerridge              //
//                                                                  //
// Licensed under the Apache License, Version 2.0 (the "License");  //
// you may not use this file except in compliance with the License. //
// You may obtain a copy of the License at                          //
//                                                                  //
//      http://www.apache.org/licenses/LICENSE-2.0                  //
//                                                                  //
// Unless required by applicable law or agreed to in writing,       //
// software distributed under the License is distributed on         //
// an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,  //
// either express or implied. See the License for the specific      //
// language governing permissions and limitations under the License.//
//                                                                  //
//                                                                  //
//                                                                  //
//                                                                  //
//  Author Contact: P.H.Welch@ukc.ac.uk                             //
//                                                                  //
//  Author contact: K.Chalmers@napier.ac.uk                         //
//                                                                  //
//                                                                  //
//////////////////////////////////////////////////////////////////////

using CSPlang.Alting;
using CSPlang.Any2;

namespace CSPlang
{

    /**
     * This class provides static factory methods for constructing
     * different types of connection. The methods are equivalent to
     * the non-static methods of the <code>StandardConnectionFactory</code>
     * class.
     *
     *
     */
    public class Connection
    {
        private static StandardConnectionFactory factory = new StandardConnectionFactory();

        /**
         * Constructor for Connection.
         */
        private Connection() : base()
        {
            
        }


        /**
         * @see jcsp.lang.ConnectionFactory#createOne2One()
         */
        public static One2OneConnection createOne2One()
        {
            return factory.createOne2One();
        }

        /**
         * @see jcsp.lang.ConnectionFactory#createAny2One()
         */
        public static Any2OneConnection createAny2One()
        {
            return factory.createAny2One();
        }

        /**
         * @see jcsp.lang.ConnectionFactory#createOne2Any()
         */
        public static One2AnyConnection createOne2Any()
        {
            return factory.createOne2Any();
        }

        /**
         * @see jcsp.lang.ConnectionFactory#createAny2Any()
         */
        public static Any2AnyConnection createAny2Any()
        {
            return factory.createAny2Any();
        }

        /**
         * @see jcsp.lang.ConnectionArrayFactory#createOne2One(int)
         */
        public static One2OneConnection[] createOne2One(int n)
        {
            return factory.createOne2One(n);
        }

        /**
         * @see jcsp.lang.ConnectionArrayFactory#createAny2One(int)
         */
        public static Any2OneConnection[] createAny2One(int n)
        {
            return factory.createAny2One(n);
        }

        /**
         * @see jcsp.lang.ConnectionArrayFactory#createOne2Any(int)
         */
        public static One2AnyConnection[] createOne2Any(int n)
        {
            return factory.createOne2Any(n);
        }

        /**
         * @see jcsp.lang.ConnectionArrayFactory#createAny2Any(int)
         */
        public static Any2AnyConnection[] createAny2Any(int n)
        {
            return factory.createAny2Any(n);
        }

        /**
         * Returns an array of client connection ends suitable for use as guards in an <code>Alternative</code>
         * construct.
         *
         * @param c the connection array to get the client ends from.
         * @return the array of client ends.
         */
        public static AltingConnectionClient[] getClientArray(One2AnyConnection[] c)
        {
            AltingConnectionClient[] r = new AltingConnectionClient[c.Length];
            for (int i = 0; i < c.Length; i++)
                r[i] = c[i].client();
            return r;
        }

        /**
         * Returns an array of client connection ends suitable for use as guards in an <code>Alternative</code>
         * construct.
         *
         * @param c the connection array to get the client ends from.
         * @return the array of client ends.
         */
        public static AltingConnectionClient[] getClientArray(One2OneConnection[] c)
        {
            AltingConnectionClient[] r = new AltingConnectionClient[c.Length];
            for (int i = 0; i < c.Length; i++)
                r[i] = c[i].client();
            return r;
        }

        /**
         * Returns an array of client connection ends suitable for use by multiple concurrent
         * processes.
         *
         * @param c the connection array to get the client ends from.
         * @return the array of client ends.
         */
        public static SharedConnectionClient[] getClientArray(Any2AnyConnection[] c)
        {
            SharedConnectionClient[] r = new SharedConnectionClient[c.Length];
            for (int i = 0; i < c.Length; i++)
                r[i] = c[i].client();
            return r;
        }

        /**
         * Returns an array of client connection ends suitable for use by multiple concurrent
         * processes.
         *
         * @param c the connection array to get the client ends from.
         * @return the array of client ends.
         */
        public static SharedConnectionClient[] getClientArray(Any2OneConnection[] c)
        {
            SharedConnectionClient[] r = new SharedConnectionClient[c.Length];
            for (int i = 0; i < c.Length; i++)
                r[i] = c[i].client();
            return r;
        }

        /**
         * Returns an array of server connection ends suitable for use as guards in an <code>Alternative</code>
         * construct.
         *
         * @param c the connection array to get the server ends from.
         * @return the array of server ends.
         */
        public static AltingConnectionServer[] getServerArray(Any2OneConnection[] c)
        {
            AltingConnectionServer[] r = new AltingConnectionServer[c.Length];
            for (int i = 0; i < c.Length; i++)
                r[i] = c[i].server();
            return r;
        }

        /**
         * Returns an array of server connection ends suitable for use as guards in an <code>Alternative</code>
         * construct.
         *
         * @param c the connection array to get the server ends from.
         * @return the array of server ends.
         */
        public static AltingConnectionServer[] getServerArray(One2OneConnection[] c)
        {
            AltingConnectionServer[] r = new AltingConnectionServer[c.Length];
            for (int i = 0; i < c.Length; i++)
                r[i] = c[i].server();
            return r;
        }

        /**
         * Returns an array of server connection ends suitable for use by multiple concurrent
         * processes.
         *
         * @param c the connection array to get the server ends from.
         * @return the array of server ends.
         */
        public static SharedConnectionServer[] getServerArray(Any2AnyConnection[] c)
        {
            SharedConnectionServer[] r = new SharedConnectionServer[c.Length];
            for (int i = 0; i < c.Length; i++)
                r[i] = c[i].server();
            return r;
        }

        /**
         * Returns an array of server connection ends suitable for use by multiple concurrent
         * processes.
         *
         * @param c the connection array to get the server ends from.
         * @return the array of server ends.
         */
        public static SharedConnectionServer[] getServerArray(One2AnyConnection[] c)
        {
            SharedConnectionServer[] r = new SharedConnectionServer[c.Length];
            for (int i = 0; i < c.Length; i++)
                r[i] = c[i].server();
            return r;
        }
    }
}