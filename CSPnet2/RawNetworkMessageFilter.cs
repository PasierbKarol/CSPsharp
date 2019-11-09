//////////////////////////////////////////////////////////////////////
//                                                                  //
//  JCSP ("CSP for Java") Libraries                                 //
//  Copyright (C) 1996-2018 Peter Welch, Paul Austin and Neil Brown //
//                2001-2004 Quickstone Technologies Limited         //
//                2005-2018 Kevin Chalmers                          //
//                                                                  //
//  You may use this work under the terms of either                 //
//  1. The Apache License, Version 2.0                              //
//  2. or (at your option), the GNU Lesser General Public License,  //
//       version 2.1 or greater.                                    //
//                                                                  //
//  Full licence texts are included in the LICENCE file with        //
//  this library.                                                   //
//                                                                  //
//  Author contacts: P.H.Welch@kent.ac.uk K.Chalmers@napier.ac.uk   //
//                                                                  //
//////////////////////////////////////////////////////////////////////

using System;

namespace CSPnet2
{
    /**
     * A NetworkMessageFilter used to send and receive raw byte data.
     * 
     * @see NetworkMessageFilter
     * @author Kevin Chalmers
     */
    public sealed class RawNetworkMessageFilter
    {
        /**
         * The receiving filter
         * 
         * @author Kevin Chalmers
         */
        public /*static*/ sealed class FilterRX : NetworkMessageFilter.FilterRx
        {
            /**
             * Creates a new RawNetworkMessageFilter.FilterRX
             */
            public FilterRX()
            {
                // Nothing to do
            }

            /**
             * Decodes an incoming byte array. Does nothing
             * 
             * @param bytes
             *            The bytes received in an incoming message
             * @return The same bytes as is passed in
             */

            public object filterRXfromJSON(string json)
            {
                throw new NotImplementedException();
            }
        }

        /**
         * The sending Filter
         * 
         * @author Kevin Chalmers
         */
        public /*static*/ sealed class FilterTX : NetworkMessageFilter.FilterTx
        {
            /**
             * Creates a new output filter
             */
            public FilterTX()
            {
                // Nothing to do
            }

            /**
             * Will send a byte array as raw bytes
             * 
             * @param obj
             *            The object to send. This must be a byte array
             * @return The same byte array as sent in
             * @//throws IOException
             *             Thrown if the sent object is not a byte array
             */
            public string filterTXtoJSON(object obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}