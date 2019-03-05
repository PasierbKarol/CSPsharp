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
        /*
         * Size of the internal buffer of the memory stream
         */
        public static int BUFFER_SIZE = 8192;
        private static MemoryStream memoryStream ; //https://stackoverflow.com/questions/10390356/serializing-deserializing-with-memory-stream;

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
             * These four bytes represent the normal header expected in Java for object streams
             */
            //static readonly byte[] objectStreamHeader = { -84, -19, 0, 5 };

            /**
             * The byte array stream used to connect to the ObjectInputStream
             */
            private readonly ResettableByteArrayInputStream bais;

            /**
             * The ObjectInputStream used to read the objects from.
             */
            // private readonly ObjectInputStream ois;
            //private readonly MemoryStream ois;

            /**
             * Creates a new incoming object filter
             */
            public FilterRX()
            {
                try
                {
                    // We need to put the header down the stream first to create the ObjectInputStream
                    //.bais = new ResettableByteArrayInputStream(FilterRX.objectStreamHeader);

                    // Now hook the ObjectInputStream to the byte array stream. Should work fine.
                    //this.ois = new ObjectInputStream(this.bais);
                    //this.ois = new MemoryStream();
                    memoryStream = getMemoryStreamInstance();

                }
                catch (IOException ioe)
                {
                    // Should never really happen, however...
                    throw new RuntimeException(
                        "Failed to create the required streams for ObjectNetwrokMessageFilter.FilterRX");
                }
            }

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
                    // Reset the byte array stream with the incoming bytes
                    //this.bais.reset(bytes);
                    // Return the object read from the input stream
                    //return this.ois.readObject();

                    /*MemoryStream stream = new MemoryStream();

                    IFormatter formatter = new BinaryFormatter();
                    stream.Seek(0, SeekOrigin.Begin);
                    T objectType = (T) formatter.Deserialize(stream);
                    return objectType;*/

                    // var serializer = new XmlSerializer(typeof(T));


                    // return (T)serializer.Deserialize(bytes);

                    return DeserializeFromStream<T>(bytes);
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
                //TODO
                // Reset the byte array stream with the incoming bytes
                //this.bais.reset(bytes);
                // Return the object read from the input stream
                // return this.ois.;    
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
             * The output stream to get the bytes from
             */
            private readonly ResettableByteArrayOutputStream baos;

            /**
             * The ObjectOutputStream connected to the byte stream to allow the serialization of objects
             */
            //private readonly ObjectOutputStream oos;
            //private readonly MemoryStream oos;

            /**
             * Creates a new encoding object filter
             */
            public FilterTX()
            {
                try
                {
                    // We use an 8Kb buffer to serialize into as default, although this could can adjusted
                    this.baos = new ResettableByteArrayOutputStream(ObjectNetworkMessageFilter.BUFFER_SIZE);
                    //this.oos = new ObjectOutputStream(this.baos);
                    memoryStream = getMemoryStreamInstance();
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
                // First we reset the byte buffer to the buffer size, just in case a previous message caused it to grow
                //this.baos.reset(ObjectNetworkMessageFilter.BUFFER_SIZE);
                // Now reset the object stream. This clears any remembered messages
                //this.oos.reset();
                // Write the object to the stream
                //this.oos.writeObject(obj);
                //byte[] a = (byte[]) new BinaryFormatter().Deserialize(oos);
                //this.oos.Write(obj);
                var a =  SerializeToStream(obj, memoryStream);
                // Get the bytes
                return a.ToArray();
            }
        }

        public static MemoryStream SerializeToStream(object o, MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        private static T DeserializeFromStream<T>(byte[] bytesData)
        {
            object o = null;
            using (MemoryStream stream = new MemoryStream(bytesData))
            {
                //IFormatter br = new BinaryFormatter();
                //o = (br.Deserialize(ms) as T);
                IFormatter formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                o = formatter.Deserialize(stream);
                stream.Flush();
            }
            return (T)o;
        }

        private static MemoryStream getMemoryStreamInstance()
        {
            if (memoryStream == null)
            {
                memoryStream = new MemoryStream();
            }
            return memoryStream;
        }
    }
}