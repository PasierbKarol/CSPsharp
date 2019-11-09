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
//  Author Contact: P.H.Welch@ukc.ac.uk                             //
//                                                                  //
//  Author contact: K.Chalmers@napier.ac.uk                         //
//                                                                  //
//                                                                  //
//////////////////////////////////////////////////////////////////////

namespace CSPutil
{
    /**
     * <p>Interface for a channel end supporting write filtering operations. A channel end that implements this
     * interface can have instances of the <code>Filter</code> interface installed to apply transformations
     * on data as it is written to the channel.</p>
     *
     * <p>Multiple filters can be installed and referenced by a zero-based index to specify a specific
     * ordering.</p>
     *
     * <p>If multiple filters are installed, they are applied in order of increasing index. For example:</p>
     *
     * <pre>
     *   FilteredChannelOutput out = ...;
     *
     *   Filter f1 = ...;
     *   Filter f2 = ...;
     *
     *   out.addWriteFilter (f1, 0);
     *   out.addWriteFilter (f2, 1);
     * </pre>
     *
     * <p>The <code>out.write()</code> method will deliver <code>f2.filter (f1.filter (obj))</code> to
     * the reader of the channel where <code>obj</code> is the data value that would have been delivered in
     * the absence of filters.</p>
     *
     * @see jcsp.util.filter.Filter
     * @see jcsp.util.filter.FilteredChannelOutput
     *
     *
     */
    public interface WriteFiltered
    {
        /**
         * Installs a write filter defining a transformation to be applied by the <code>write</code> method of
         * the channel end. The filter will be appended to the end of the current list, making it the last to
         * be applied.
         *
         * @param filter the filter to be installed; may not be null.
         */
        void addWriteFilter(Filter filter);

        /**
         * Installs a write filter defining a transformation to be applied by the <code>write</code> method of the
         * channel end at a specific index. If there is already a filter at that index position the existing
         * filters are shifted to make room. If the index is greater than the number of filters already
         * installed the filter is placed at the end.
         *
         * @param filter the filter to be installed; may not be null.
         * @param index the zero based index; may not be negative.
         */
        void addWriteFilter(Filter filter, int index);

        /**
         * Removes the first write filter (lowest index) matching the filter given as a parameter. The filter
         * removed, <code>r</code>, will satisfy the condition <code>r.equals (filter)</code>. The remaining
         * filters are shifted to close the gap in the index allocation.
         *
         * @param filter the filter to be removed; may not be null.
         */
        void removeWriteFilter(Filter filter);

        /**
         * Removes the write filter installed at the given index. The remaining filters are shifted to close the
         * gap in the index allocation.
         *
         * @param index zero-based index of the filter to be removed.
         */
        void removeWriteFilter(int index);

        /**
         * Returns the write filter installed at the given index.
         *
         * @param index zero-based index of the filter to return.
         * @return the filter at that position.
         */
        Filter getWriteFilter(int index);

        /**
         * Returns the number of write filters currently installed.
         */
        int getWriteFilterCount();
    }
}