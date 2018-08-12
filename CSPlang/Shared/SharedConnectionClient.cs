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

namespace CSPlang.Shared
{

    /**
     * <p>
     * Defines an interface for a client end of a connection that
     * can be shared by multiple clients.
     * </p>
     * <p>
     * This object cannot itself be shared between concurrent processes
     * but duplicate objects can be generated that can be used by
     * multiple concurrent processes. This can be achieved using
     * the <code>{@link #duplicate()}</code> method.
     * </p>
     * <p>
     * See <code>{@link ConnectionClient}</code> for a fuller explanation
     * of how to use connection client objects.
     * </p>
     *
     *
     */
    public interface SharedConnectionClient : ConnectionClient
    {
        /**
         * Returns a duplicates <code>SharedConnectionClient</code> object
         * which may be used by another process to this instance.
         *
         * @return a duplicate <code>SharedConnectionClient</code> object.
         */
        SharedConnectionClient duplicate();
    }
}