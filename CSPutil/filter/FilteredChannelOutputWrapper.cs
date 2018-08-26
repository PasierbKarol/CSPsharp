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

package jcsp.util.filter;

import jcsp.lang.*;

/**
 * Wrapper for an output channel end to include write filtering ability.
 *
 *
 */
class FilteredChannelOutputWrapper
        extends ChannelOutputWrapper
        implements FilteredChannelOutput
{
    /**
     * Set of write filters installed.
     */
    private FilterHolder filters = null;

    /**
     * Constructs a new <code>FilteredChannelOutputWrapper</code> around the given output channel end.
     *
     * @param out the existing output channel.
     */
    public FilteredChannelOutputWrapper(ChannelOutput out)
    {
        super(out);
    }

    public void write(Object data)
    {
        for (int i = 0; filters != null && i < filters.getFilterCount(); i++)
            data = filters.getFilter(i).filter(data);
        super.write(data);
    }

    public void addWriteFilter(Filter filter)
    {
        if (filters == null)
            filters = new FilterHolder();
        filters.addFilter(filter);
    }

    public void addWriteFilter(Filter filter, int index)
    {
        if (filters == null)
            filters = new FilterHolder();
        filters.addFilter(filter, index);
    }

    public void removeWriteFilter(Filter filter)
    {
        if (filters == null)
            filters = new FilterHolder();
        filters.removeFilter(filter);
    }

    public void removeWriteFilter(int index)
    {
        if (filters == null)
            filters = new FilterHolder();
        filters.removeFilter(index);
    }

    public Filter getWriteFilter(int index)
    {
        if (filters == null)
            filters = new FilterHolder();
        return filters.getFilter(index);
    }

    public int getWriteFilterCount()
    {
        if (filters == null)
            return 0;
        return filters.getFilterCount();
    }
}
