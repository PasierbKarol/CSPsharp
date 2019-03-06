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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using CSPlang;
using CSPnet2;

namespace CSPnet2
{
//    import java.io.IOException;
//import java.io.ObjectInputStream;
//import java.io.ObjectOutputStream;

/**
 * This class is the standard encoding and decoding filter for networked JCSP channels. It uses standard Java
 * serialization to operate.
 * 
 * @author Kevin Chalmers
 */
    public sealed class ObjectNetworkMessageFilter
    {
       //https://stackoverflow.com/questions/10390356/serializing-deserializing-with-memory-stream;

        /**
         * The receiving (decoding) filter for Objects
         * 
         * @author Kevin Chalmers
         */
        /**
 * Static classes are sealed by default in C# - Karol Pasierb
 */
        public /*static*/ sealed class FilterRX : NetworkMessageFilter.FilterRx
        {

            /**
             * Creates a new incoming object filter
             */
            public FilterRX()
            {
                try
                {

                }
                catch (IOException ioe)
                {
                    // Should never really happen, however...
                    throw new RuntimeException(
                        "Failed to create the required streams for ObjectNetwrokMessageFilter.FilterRX");
                }
            }

            //https://stackoverflow.com/questions/33616621/how-to-deserialize-byte-into-generic-object-to-be-cast-at-method-call/33616721
            /**
             * Decodes an incoming byte array, converting it back into an Object
             * 
             * @param bytes
             *            The byte representation of the object
             * @return The recreated Object
             * @//throws IOException
             *             Thrown of something goes wrong during the decoding
             */
            public T filterRX<T>(byte[] bytes)
                ////throws IOException
            {
                try
                {
                    object o = null;
                    using (MemoryStream stream = new MemoryStream(bytes))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        stream.Seek(0, SeekOrigin.Begin);
                        o = formatter.Deserialize(stream);
                        stream.Flush();
                    }
                    return (T)o;
                }
                //catch (ClassNotFoundException cnfe)
                catch (Exception cnfe)
                {
                    // Not an exception thrown by other filters, so we convert into an IOException
                    throw new IOException("Class not found");
                }
            }

            public object filterRX(byte[] bytes)
            {
   
                return new object();
            }
        }

        /**
         * The sending (encoding) filter for Object channels
         * 
         * @author Kevin Chalmers
         */
        public /*static*/ sealed class FilterTX : NetworkMessageFilter.FilterTx
        {

            /**
             * Creates a new encoding object filter
             */
            public FilterTX()
            {
                try
                {
                }
                catch (IOException ioe)
                {
                    throw new RuntimeException(
                        "Failed to create the required streams for ObjectNetworkMessageFilter.FilterTX");
                }
            }

            /**
             * Encodes an object into bytes by using Object serialization
             * 
             * @param obj
             *            The Object to serialize
             * @return The byte array equivalent of the object
             * @//throws IOException
             *             Thrown if something goes wrong during the serialization
             */
            public byte[] filterTX(Object obj)
                // //throws IOException
            {
                IFormatter formatter = new BinaryFormatter();
                MemoryStream memoryStream = new MemoryStream();
                formatter.Serialize(memoryStream, obj);

                // Get the bytes
                return memoryStream.ToArray();
            }
        }

    }
}