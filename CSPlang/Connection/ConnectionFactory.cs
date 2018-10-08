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

using CSPlang.Any2;

namespace CSPlang
{

    /**
     * <p>
     * Defines an interface for a factory than can create connections.
     * </p>
     *
     *
     */
    public interface ConnectionFactory
    {
        /**
         * Constructs and returns an implementation of
         * <code>One2OneConnection</code>.
         *
         * @return	the constructed <code>One2OneConnection</code> object.
         */
         One2OneConnection createOne2One();

        /**
         * Constructs and returns an implementation of
         * <code>Any2OneConnection</code>.
         *
         * @return	the constructed <code>Any2OneConnection</code> object.
         */
         Any2OneConnection createAny2One();

        /**
         * Constructs and returns an implementation of
         * <code>One2AnyConnection</code>.
         *
         * @return	the constructed <code>One2AnyConnection</code> object.
         */
         One2AnyConnection createOne2Any();

        /**
         * Constructs and returns an implementation of
         * <code>Any2AnyConnection</code>.
         *
         * @return	the constructed <code>Any2AnyConnection</code> object.
         */
         Any2AnyConnection createAny2Any();
    }
}