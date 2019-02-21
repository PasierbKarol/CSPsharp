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

namespace CSPnet2.NetConnection
{

sealed class ConnectionManager
{
    private static int index = 50;

    private readonly Hashtable connections = new Hashtable();

    private static ConnectionManager instance = new ConnectionManager();

    private ConnectionManager()
    {

    }

    internal static ConnectionManager getInstance()
    {
        return instance;
    }

    /*synchronized*/ internal void create(ConnectionData data)
    {
        int objIndex = index;
        while (this.connections[objIndex] != null)
        {
            //objIndex = new Integer(++index);
            ++index;
        }

        data.vconnn = index;

        this.connections.Add(objIndex, data);

        index++;
    }

    /*synchronized*/ internal void create(int idx, ConnectionData data)
        //throws IllegalArgumentException
    {
        int objIndex = idx;
        if (this.connections[objIndex] != null)
        {
            throw new ArgumentException("Connection of given number already exists");
        }

        data.vconnn = idx;

        this.connections.Add(objIndex, data);

        if (idx == ConnectionManager.index)
        {
            index++;
        }
    }

    internal ConnectionData getConnection(int idx)
    {
        int objIndex = idx;
        return (ConnectionData)this.connections[objIndex];
    }

    internal void removeConnection(ConnectionData data)
    {
        int objIndex =data.vconnn;
        this.connections.Remove(objIndex);
    }
}
}