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
     * @author Jo Aldous
     */
    abstract class AbstractConnectionImpl
    {
        protected static readonly int CLIENT_STATE_CLOSED = 1;
        protected static readonly int CLIENT_STATE_OPEN = 2;
        protected static readonly int CLIENT_STATE_MADE_REQ = 3;
        protected static readonly int SERVER_STATE_CLOSED = 1;
        protected static readonly int SERVER_STATE_OPEN = 2;
        protected static readonly int SERVER_STATE_RECEIVED = 3;

        protected static readonly NonSingleRequestOpenMsg nonSingleRequestMsg = new NonSingleRequestOpenMsg();

        protected class NonSingleRequestOpenMsg
        {
        }
    }
}