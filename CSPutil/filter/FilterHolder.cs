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

/**
 * Storage scheme for a set of filters that is dynamically sized and supports insert and remove
 * operations to keep the filters in a contiguous block.
 *
 *
 */
class FilterHolder
{
    /**
     * The array of filters. The installed filters are in a block at the start of the array.
     */
    private Filter[] filters;

    /**
     * Number of filters currently installed.
     */
    private int count = 0;

    /**
     * Constructs a new <code>FilterHolder</code> with an intial capacity of 2.
     */
    FilterHolder()
    {
        this(2);
    }

    /**
     * Constructs a new <Code>FilterHolder</code> with the given initial capacity.
     *
     * @param initialSize the initial size for the array.
     */
    FilterHolder(int initialSize)
    {
        filters = new Filter[initialSize];
    }

    /**
     * Adds a filter to the end of the array, possibly enlarging it if it is full.
     *
     * @param filter the filter to add.
     */
    public void addFilter(Filter filter)
    {
        makeSpace();
        filters[count] = filter;
        count++;
    }

    /**
     * Adds a filter at the given index. If the index is past the end of the array, the filter is placed
     * at the end of the array. If the index is in use, filter is inserted, shifting the existing ones.
     * If necessary, the array may be enlarged.
     *
     * @param filter the filter to add.
     * @param index the position to add the filter.
     */
    public void addFilter(Filter filter, int index)
    {
        if (index >= count)
            //add filter to end
            addFilter(filter);
        else
        {
            makeSpace();
            //shift all elements from specifed index and above along one
            System.arraycopy(filters, index, filters, index + 1, count - index);
            filters[index] = filter;
            count++;
        }
    }

    /**
     * Removes a filter from the set. The first filter, <code>f</code>, satisfying the condition
     * <code>f.equals (filter)</code> is removed and the remaining filters shifted to close the gap.
     *
     * @param filter the filter to remove.
     */
    public void removeFilter(Filter filter)
    {
        if (filter == null)
            throw new IllegalArgumentException("filter parameter cannot be null");
        for (int i = 0; i < count; i++)
            if (filters[i].equals(filter))
            {
                removeFilter(i);
                return;
            }
        throw new IllegalArgumentException("supplied filter not installed.");
    }

    /**
     * Removes a filter at a given index. The remaining filters are shifted to close the gap.
     *
     * @param index the array index to remove the filter.
     */
    public void removeFilter(int index)
    {
        if (index > (count - 1) || index < 0)
            throw new IndexOutOfBoundsException("Invalid filter index.");
        filters[index] = null;
        //if filter not the last item in the array
        //then need to shift all elements after the
        //specified filter
        if (index < filters.length - 1)
        {
            System.arraycopy(filters, index + 1, filters, index, count - index - 1);
            count--;
        }
        else
        {
            count--;
            compact();
        }
    }

    /**
     * Returns a filter at the given array index.
     */
    public Filter getFilter(int index)
    {
        return filters[index];
    }

    /**
     * Returns the number of filters current installed.
     */
    public int getFilterCount()
    {
        return count;
    }

    /**
     * Enlarges the size of the array to make room for more filters. Currently the array is doubled
     * in size.
     */
    private void makeSpace()
    {
        //if array of filters is full - double size
        if (count == filters.length)
        {
            Filter[] filters = new Filter[count * 2];
            System.arraycopy(this.filters, 0, filters, 0, this.filters.length);
            this.filters = filters;
        }
    }

    /**
     * Shrinks the array to save space if it is 75% empty.
     */
    private void compact()
    {
        int newSize = count + 1;
        if (count < (filters.length / 4) && newSize < filters.length)
        {
            //create a new array which
            Filter[] filters = new Filter[newSize];
            System.arraycopy(this.filters, 0, filters, 0, count);
            this.filters = filters;
        }
    }
}
