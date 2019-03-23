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
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CSPnet2
{
    /**
     * This class is the standard encoding and decoding filter for networked JCSP channels. It uses standard Java
     * serialization to operate.
     *
     * https://stackoverflow.com/questions/29688498/how-to-deserialize-json-to-objects-of-the-correct-type-without-having-to-define
     *
     * @author Kevin Chalmers, altered by Karol Pasierb
     */
    public sealed class ObjectNetworkMessageFilter
    {
        private static JsonSerializerSettings settings = 
            new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, TypeNameAssemblyFormat = FormatterAssemblyStyle.Full };

        /**
         * The receiving (decoding) filter for Objects
         * 
         * @author Kevin Chalmers
         */
        /**
         * Static classes are sealed by default in C# - Karol Pasierb
         */
        public sealed class FilterRX : NetworkMessageFilter.FilterRx
        {
            /**
             * Decodes an incoming byte array, converting it back into an Object
             *
             * https://stackoverflow.com/questions/33616621/how-to-deserialize-byte-into-generic-object-to-be-cast-at-method-call/33616721
             *
             * @param bytes
             *            The byte representation of the object
             * @return The recreated Object
             * @//throws IOException
             *             Thrown of something goes wrong during the decoding
             */
            public object filterRXfromJSON(string json)
            {
                return JsonConvert.DeserializeObject(json, settings);
            }
        }

        /**
         * The sending (encoding) filter for Object channels
         * 
         * @author Kevin Chalmers
         */
        public sealed class FilterTX : NetworkMessageFilter.FilterTx
        {
            /**
             * Encodes an object into bytes by using Object serialization
             * 
             * @param obj
             *            The Object to serialize
             * @return The byte array equivalent of the object
             * @//throws IOException
             *             Thrown if something goes wrong during the serialization
             */
            public string filterTXtoJSON(Object obj)
            {
                return JsonConvert.SerializeObject(obj, obj.GetType(), settings);
            }
        }
    }
}