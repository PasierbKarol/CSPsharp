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
using CSPnet2.NetChannels;

namespace CSPnet2.CNS
{

/**
 * This filter is used by the CNS and CNSService to transmit messages between one another in a manner that is platform
 * independent. This is an internal class to JCSP, and is created automatically by the CNS and CNSService. For more
 * information, see the relevant documentation.
 * 
 * @see CNS
 * @see CNSService
 * @see NetworkMessageFilter
 * @author Kevin Chalmers
 */
sealed class CNSNetworkMessageFilter
{

    /**
     * The encoding filter used to convert a CNSMessage into bytes
     * 
     * @author Kevin Chalmers
     */
    /*static*/ internal sealed class FilterTX : NetworkMessageFilter.FilterTx
    {
        /**
         * The byte stream we will use to retrieve the byte message from
         */
        private readonly MemoryStream baos;

        /**
         * The data stream, used to write the parts of the CNSMessage to
         */
        private readonly BinaryWriter dos;

            /**
             * Creates a new CNS encoding filter
             */
        internal FilterTX()
        {
            this.baos = new MemoryStream(8192);
            this.dos = new BinaryWriter(this.baos);
        }

        /**
         * Converts an object (a CNSMessage) into bytes
         * 
         * @param obj
         *            The CNSMessage to convert
         * @return The byte equivalent of the CNSMessage
         * @//throws IOException
         *             Thrown if something goes wrong during the conversion
         */
        public byte[] filterTX(Object obj)
            ////throws IOException
        {
            // First ensure we have a CNSMessage
            if (!(obj is CNSMessage))
                throw new IOException("Attempted to send a non CNSMessage on a CNSMessage channel");
            CNSMessage msg = (CNSMessage)obj;

            // Now reset the byte stream
            //this.baos.reset();
            this.baos.Dispose();
            // Write the parts of the CNSMessage to the stream
            this.dos.Write(msg.type);
            this.dos.Write(msg.success);
            if (msg.location1 != null)
                this.dos.Write(msg.location1.LocationToString());
            else
                this.dos.Write("null");
            if (msg.location2 != null)
                this.dos.Write(msg.location2.LocationToString());
            else
                this.dos.Write("null");
            this.dos.Write(msg.name);
            // Flush the stream
            this.dos.Flush();
            // Get the bytes
            return this.baos.ToArray();
        }

    }

    /**
     * The filter used to convert a CNSMessage from its byte representation back into an object
     * 
     * @author Kevin Chalmers
     */
    /*static*/ internal sealed class FilterRX : NetworkMessageFilter.FilterRx
    {
        /**
         * The input end to read the message back from
         */
        private MemoryStream byteIn;

        /**
         * The data input stream used to read in the parts of the message
         */
        private BinaryReader dis;

        /**
         * Creates a new decoding CNSMessage filter
         */
        internal FilterRX()
        {
            // Nothing to do
        }

        /**
         * Decodes the byte equivalent of a CNSMessage
         * 
         * @param bytes
         *            The byte equivalent of a CNSMessage
         * @return The recreated CNSMessage
         * @//throws IOException
         *             Thrown if something goes wrong during the recreation
         */
        public Object filterRX(byte[] bytes)
            ////throws IOException
        {
            this.byteIn = new MemoryStream(bytes);
            this.dis = new BinaryReader(byteIn);

            // Recreate the message
            CNSMessage msg = new CNSMessage();
            msg.type = this.dis.ReadByte();
            msg.success = this.dis.ReadBoolean();
            msg.location1 = NetChannelLocation.parse(this.dis.ReadString());
            msg.location2 = NetChannelLocation.parse(this.dis.ReadString());
            msg.name = this.dis.ReadString();
            return msg;
        }

        public T filterRX<T>(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
}