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
     * This filter will throw a <code>PoisonException</code>
     * when <code>filter(Object)</code> is called. This can be
     * used to prevent a channel from being written to or read from.
     *
     *
     */
    public class PoisonFilter : Filter
    {
    /**
     * The message to be placed in the <code>PoisonException</code> raised.
     */
    private String message;

    /**
     * Default message.
     */
    private static String defaultMessage = "Channel end has been poisoned.";

    /**
     * Constructs a new filter with the default message.
     */
    public PoisonFilter() : this (defaultMessage)
        {
        
    }

    /**
     * Constructs a new filter with a specific message.
     */
    public PoisonFilter(String message)
    {
        this.message = message;
    }

    public Object filter(Object obj)
    {
        throw new PoisonFilterException(this.message);
    }
    }
}