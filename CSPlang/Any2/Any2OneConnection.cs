﻿//////////////////////////////////////////////////////////////////////
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

using CSPlang.Alting;
using CSPlang.Shared;

namespace CSPlang.Any2
{
    /**
 * <p>
 * Defines an interface for a connection that can be shared
 * by multiple concurrent clients but used by
 * a single server. The server end of the connection can be
 * used as a guard in an <code>Alternative</code>.
 * </p>
 *
 *
 */


    public interface Any2OneConnection : ConnectionWithSharedAltingClient
    {
        /**
 * Returns a client end of the connection. This may only be
 * safely used by a single process but further calls will
 * return new clients which may be used by other processes.
 *
 * @return a new <code>SharedAltingConnectionClient</code> object.
 */
        SharedAltingConnectionClient client();

        /**
         * Returns the server end of the connection.
         *
         * @return the instance of the <code>AltingConnectionServer</code>.
         */
        AltingConnectionServer server();
    }
}