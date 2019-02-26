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
using CSPnet2;
using CSPnet2.Mobile;
using CSPnet2.NetChannels;
using CSPnet2.NetNode;

namespace CSPnet2.Mobile
{

/**
 * @author Kevin
 */
public sealed class CodeLoadingChannelFilter
{
    public /*static*/ sealed class FilterRX : NetworkMessageFilter.FilterRx
    {
        private readonly ObjectNetworkMessageFilter.FilterRX objectFilter = new ObjectNetworkMessageFilter.FilterRX();

        public FilterRX()
        {
            // Do nothing
        }

        public Object filterRX(byte[] bytes)
            ////throws IOException
        {
            try
            {
                Object message = this.objectFilter.filterRX(bytes);
                if (!(message is DynamicClassLoaderMessage))
                {
                    return message;
                }

                DynamicClassLoaderMessage loaderMessage = (DynamicClassLoaderMessage)message;
                byte[] bytesWithHeader = new byte[4 + loaderMessage.bytes.Length];
                byte[] header = { -84, -19, 0, 5 };
                System.arraycopy(header, 0, bytesWithHeader, 0, 4);
                System.arraycopy(loaderMessage.bytes, 0, bytesWithHeader, 4, loaderMessage.bytes.Length);
                ByteArrayInputStream bais = new ByteArrayInputStream(bytesWithHeader);
                DynamicClassLoader loader = (DynamicClassLoader) ClassManager.classLoaders[loaderMessage.originatingNode];

                if (loader == null)
                {
                    loader = new DynamicClassLoader(loaderMessage.originatingNode, loaderMessage.requestLocation);
                    ClassManager.classLoaders.Add(loaderMessage.originatingNode, loader);
                }

                DynamicObjectInputStream dois = new DynamicObjectInputStream(bais, loader);

                Object toReturn = dois.readObject();
                return toReturn;
            }
            //catch (ClassNotFoundException cnfe)
            catch (Exception cnfe)
            {
                throw new IOException("Failed to load class");
            }
        }
    }

    public /*static*/ sealed class FilterTX : NetworkMessageFilter.FilterTx
    {
        private readonly ObjectNetworkMessageFilter.FilterTX internalFilter = new ObjectNetworkMessageFilter.FilterTX();

        public FilterTX()
        {

        }

        public byte[] filterTX(Object obj)
            ////throws IOException
        {
            DynamicClassLoaderMessage message;
            byte[] wrappedData;
            ClassLoader loader = obj.GetType().getClassLoader();
            byte[] bytes = this.internalFilter.filterTX(obj);
            if (loader == ClassLoader.getSystemClassLoader() || loader == null)
            {
                message = new DynamicClassLoaderMessage(Node.getInstance().getNodeID(),
                        (NetChannelLocation) ClassManager.In.getLocation(), bytes);
                wrappedData = this.internalFilter.filterTX(message);
                return wrappedData;
            }
            DynamicClassLoader dcl = (DynamicClassLoader)loader;
            message = new DynamicClassLoaderMessage(dcl.originatingNode,
                    (NetChannelLocation) ClassManager.In.getLocation(), bytes);
            wrappedData = this.internalFilter.filterTX(message);
            return wrappedData;
        }

    }
}
}