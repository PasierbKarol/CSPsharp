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

using System;

namespace CSPutil
{

    /**
     * This is wrapper for a <code>SharedChannelOutput</code> that adds
     * write filtering. Instances of this class can be safely used by
     * multiple concurrent processes.
     *
     *
     */
    public class FilteredSharedChannelOutputWrapper : FilteredChannelOutputWrapper, FilteredSharedChannelOutput
    {

    /**
     * The synchronization object to protect the writers from each other when they read data or update
     * the write filters.
     */
    private Object synchObject;

    /**
     * Constructs a new wrapper for the given channel output end.
     *
     * @param out the existing channel end.
     */
    public FilteredSharedChannelOutputWrapper(SharedChannelOutput Out) : base (Out)
    {
        
        synchObject = new Object();
    }

    public void write(Object data)
    {
        synchronized(synchObject)
        {
            super.write(data);
        }
    }

    public void addWriteFilter(Filter filter)
    {
        synchronized(synchObject)
        {
            super.addWriteFilter(filter);
        }
    }

    public void addWriteFilter(Filter filter, int index)
    {
        synchronized(synchObject)
        {
            super.addWriteFilter(filter, index);
        }
    }

    public void removeWriteFilter(Filter filter)
    {
        synchronized(synchObject)
        {
            super.removeWriteFilter(filter);
        }
    }

    public void removeWriteFilter(int index)
    {
        synchronized(synchObject)
        {
            super.removeWriteFilter(index);
        }
    }

    public Filter getWriteFilter(int index)
    {
        synchronized(synchObject)
        {
            return super.getWriteFilter(index);
        }
    }

    public int getWriteFilterCount()
    {
        synchronized(synchObject)
        {
            return super.getWriteFilterCount();
        }
    }
}
}