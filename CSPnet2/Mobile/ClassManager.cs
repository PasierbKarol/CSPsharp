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
using System.Collections;
using System.IO;
using CSPlang;
using CSPnet2.Mobile;
using CSPnet2.NetChannel;
using CSPnet2.NetNode;

namespace CSPnet2.Mobile
{

//    import java.io.IOException;
//import java.io.InputStream;
//import java.util.Hashtable;
//
//import jcsp.net2.JCSPNetworkException;
//import jcsp.net2.NetChannel;
//import jcsp.net2.NetChannelInput;
//import jcsp.net2.NetChannelOutput;

/**
 * @author Kevin
 */
sealed class ClassManager : IamCSProcess
{
    internal static Hashtable classLoaders = new Hashtable();

    static NetChannelInput In = NetChannel.numberedNet2One(10);

    public void run()
    {
        while (true)
        {
            try
            {
                ClassRequest req = (ClassRequest) In.read();
                if (req.originatingNode.equals(Node.getInstance().getNodeID()))
                {
                    String className = req.className.Replace('.', '/') + ".class";
                    InputStream inputStream = ClassLoader.getSystemResourceAsStream(className);
                    try
                    {
                        if (inputStream != null)
                        {
                            int read = 0;
                            byte[] bytes = new byte[inputStream.available()];
                            while (read < bytes.Length)
                                read += inputStream.read(bytes, read, bytes.Length - read);
                            ClassData resp = new ClassData(req.className, bytes);
                            NetChannelOutput Out = NetChannel.one2net(req.returnLocation);
                            Out.asyncWrite(resp);
                            Out.destroy();
                            Out = null;
                        }
                        else
                        {
                            ClassData resp = new ClassData(req.className, null);
                            NetChannelOutput Out = NetChannel.one2net(req.returnLocation);
                            Out.asyncWrite(resp);
                            Out.destroy();
                            Out = null;
                        }
                    }
                    catch (IOException ioe)
                    {
                        ClassData resp = new ClassData(req.className, null);
                        NetChannelOutput Out = NetChannel.one2net(req.returnLocation);
                        Out.asyncWrite(resp);
                        Out.destroy();
                    }
                }
                else
                {
                    DynamicClassLoader loader = (DynamicClassLoader)ClassManager.classLoaders[req.originatingNode];
                    if (loader == null)
                    {
                        ClassData resp = new ClassData(req.className, null);
                        NetChannelOutput Out = NetChannel.one2net(req.returnLocation);
                        Out.asyncWrite(resp);
                        Out.destroy();
                    }
                    else
                    {
                        try
                        {
                            byte[] bytes = loader.requestClass(req.className);
                            ClassData resp = new ClassData(req.className, bytes);
                            NetChannelOutput Out = NetChannel.one2net(req.returnLocation);
                            Out.asyncWrite(resp);
                            Out.destroy();
                        }
                        catch (ClassNotFoundException cnf)
                        {
                            ClassData resp = new ClassData(req.className, null);
                            NetChannelOutput Out = NetChannel.one2net(req.returnLocation);
                            Out.asyncWrite(resp);
                            Out.destroy();
                        }
                    }
                }
            }
            catch (JCSPNetworkException jne)
            {
                // Do nothing
            }
        }
    }
}
}