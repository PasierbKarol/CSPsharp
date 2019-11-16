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
using System.IO;
using CSPnet2.Barriers;
using CSPnet2.NetChannels;

namespace CSPnet2.BNS
{
    /**
     * This filter is used by the BNS and BNSService to transmit messages between one another in a manner that is platform
     * independent. This is an internal class to JCSP and is created automatically by the BNS and BNSService. For more
     * information, see the relevant documentation.
     * 
     * @see BNS
     * @see BNSService
     * @see NetworkMessageFilter
     * @author Kevin Chalmers
     */
    sealed class BNSNetworkMessageFilter
    {
        /**
         * The encoding filter used to convert a BNSMessage into bytes
         * 
         * @author Kevin Chalmers
         */

        /*static*/
        internal sealed class FilterTX : NetworkMessageFilter.FilterTx
        {
            /**
             * The byte stream we will use to retrieve the byte message from
             */
            private readonly MemoryStream baos;

            /**
             * the data stream used to write the parts of the BNSMessage to
             */
            private readonly BinaryWriter dos;

            /**
             * Creates a new BNSMessage encoding filter
             */
            internal FilterTX()
            {
                this.baos = new MemoryStream(8192);
                this.dos = new BinaryWriter(this.baos);
            }

            /**
             * Converts a BNSMessage into bytes
             * 
             * @param obj
             *            A BNSMessage to convert
             * @return the byte equivalent of the BNSMessage
             * @//throws IOException
             *             Thrown if something goes wrong during the conversion
             */
            public byte[] filterTX(Object obj)
            ////throws IOException
            {
                // First ensure we have a BNSMessage
                if (!(obj is BNSMessage))
                    throw new IOException("Attempted to send a non BNSMessage on a BNSMessage channel");

                BNSMessage message = (BNSMessage)obj;

                // Now reset the byte stream
                //this.baos.reset();
                this.baos.Dispose(); //TODO check whther this replacement works
                // Write the parts of the BNSMessage to the stream
                this.dos.Write(message.type);
                this.dos.Write(message.wasPreviousMessageSuccessful);
                if (message.serviceLocation != null)
                    this.dos.Write(message.serviceLocation.ToString());
                else
                    this.dos.Write("null");
                if (message.location != null)
                    this.dos.Write(message.location.toString());
                else
                    this.dos.Write("null");
                this.dos.Write(message.name);
                // flush the stream
                this.dos.Flush();
                // Get the bytes
                return this.baos.ToArray();
            }

            public string filterTXtoJSON(object obj)
            {
                throw new NotImplementedException();
            }
        }

        /**
         * The filter used to convert an array of bytes back into a BNSMessage
         * 
         * @author Kevin Chalmers
         */
        /*static*/
        internal sealed class FilterRX : NetworkMessageFilter.FilterRx
        {
            /**
             * The input end of the pipe to read the message back
             */
            private MemoryStream byteIn;

            /**
             * The data input stream used to read in parts of the message
             */
            private BinaryReader dis;

            internal FilterRX()
            {
            }

            /**
             * Decodes a byte array back into a BNSMessage
             * 
             * @param bytes
             *            The bytes to convert back into a BNSMessage
             * @return The recreated BNSMessage
             * @//throws IOException
             *             Thrown if something goes wrong during the recreation
             */
            public Object filterRX(byte[] bytes) //TODO check how to throw some exception here
            ////throws IOException
            {
                this.byteIn = new MemoryStream(bytes);
                this.dis = new BinaryReader(byteIn);

                // Recreate the message
                BNSMessage message = new BNSMessage();
                message.type = this.dis.ReadByte();
                message.wasPreviousMessageSuccessful = this.dis.ReadBoolean();
                message.serviceLocation = NetChannelLocation.parse(this.dis.ReadString());
                message.location = NetBarrierLocation.parse(this.dis.ReadString());
                message.name = this.dis.ReadString();
                return message;
            }

            public T filterRX<T>(byte[] bytes)
            {
                throw new NotImplementedException();
            }

            public object filterRXfromJSON(string json)
            {
                throw new NotImplementedException();
            }
        }
    }
}