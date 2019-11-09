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

namespace CSPlang
{
    /**
     * This implements {@link ChannelOutputInt} with <I>black hole</I> semantics.
     * <H2>Description</H2>
     * <TT>BlackHoleChannelInt</TT> is an implementation of {@link ChannelOutputInt} that yields
     * <I>black hole</I> semantics for a channel.  Writers may always write but there can be
     * no readers.  Any number of writers may share the same <I>black hole</I>.
     * <P>
     * <I>Note:</I> <TT>BlackHoleChannelInt</TT>s are used for masking off unwanted outputs
     * from processes.  They are useful when we want to reuse an existing process component
     * intact, but don't need some of its output channels (i.e. we don't want to redesign
     * and reimplement the component to remove the redundant channels).  Normal channels cannot
     * be plugged in and left dangling as this may deadlock (parts of) the component being
     * reused.
     * <P>
     *
     * @see jcsp.lang.ChannelOutputInt
     * @see jcsp.lang.One2OneChannelInt
     * @see jcsp.lang.Any2OneChannelInt
     * @see jcsp.lang.One2AnyChannelInt
     * @see jcsp.lang.Any2AnyChannelInt
     *
     * @author P.H.Welch
     */
    public class BlackHoleChannelInt : ChannelOutputInt //TODO is it actually needed?
    {
        /**
        * Write an integer to the channel and loose it.
        *
        * @param value the object to write to the channel.
        */
        public void write(int i)
        {
        }

        public void poison(int strength)
        {
        }
    }
}