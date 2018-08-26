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
     * This is wrapper for a <code>SharedChannelInput</code> that adds
     * read filtering. Instances of this class can be safely used by
     * multiple concurrent processes.
     *
     *
     */
    public class FilteredSharedChannelInputWrapper : FilteredChannelInputWrapper, FilteredSharedChannelInput
    {
    /**
     * The object used for synchronization by the methods here to protect the readers from each other
     * when manipulating the filters and reading data.
     */
    private Object synchObject;

    /**
     * Constructs a new wrapper for the given channel input end.
     *
     * @param in the existing channel end.
     */
    public FilteredSharedChannelInputWrapper(SharedChannelInput In) : base(In)
    {
        
        synchObject = new Object();
    }

    public Object read()
    {
        synchronized(synchObject)
        {
            return super.read();
        }
    }

    public void addReadFilter(Filter filter)
    {
        synchronized(synchObject)
        {
            super.addReadFilter(filter);
        }
    }

    public void addReadFilter(Filter filter, int index)
    {
        synchronized(synchObject)
        {
            super.addReadFilter(filter, index);
        }
    }

    public void removeReadFilter(Filter filter)
    {
        synchronized(synchObject)
        {
            super.removeReadFilter(filter);
        }
    }

    public void removeReadFilter(int index)
    {
        synchronized(synchObject)
        {
            super.removeReadFilter(index);
        }
    }

    public Filter getReadFilter(int index)
    {
        synchronized(synchObject)
        {
            return super.getReadFilter(index);
        }
    }

    public int getReadFilterCount()
    {
        synchronized(synchObject)
        {
            return super.getReadFilterCount();
        }
    }
    }
}