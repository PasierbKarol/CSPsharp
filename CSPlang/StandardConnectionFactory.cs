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
     * Implements a factory for creating connections.
     * </p>
     *
     *
     */
    public class StandardConnectionFactory : ConnectionFactory, ConnectionArrayFactory
    {
    /**
     * @see ConnectionFactory#createOne2One
     */
    public One2OneConnection createOne2One()
    {
        return new One2OneConnectionImpl();
    }

    /**
     * @see ConnectionFactory#createAny2One
     */
    public Any2OneConnection createAny2One()
    {
        return new Any2OneConnectionImpl();
    }

    /**
     * @see ConnectionFactory#createOne2Any
     */
    public One2AnyConnection createOne2Any()
    {
        return new One2AnyConnectionImpl();
    }

    /**
     * @see ConnectionFactory#createAny2Any
     */
    public Any2AnyConnection createAny2Any()
    {
        return new Any2AnyConnectionImpl();
    }

    /**
     * @see ConnectionArrayFactory#createOne2One
     */
    public One2OneConnection[] createOne2One(int n)
    {
        One2OneConnection[] toReturn = new One2OneConnection[n];
        for (int i = 0; i < n; i++)
            toReturn[i] = createOne2One();
        return toReturn;
    }

    /**
     * @see ConnectionArrayFactory#createAny2One
     */
    public Any2OneConnection[] createAny2One(int n)
    {
        Any2OneConnection[] toReturn = new Any2OneConnection[n];
        for (int i = 0; i < n; i++)
            toReturn[i] = createAny2One();
        return toReturn;
    }

    /**
     * @see ConnectionArrayFactory#createOne2Any
     */
    public One2AnyConnection[] createOne2Any(int n)
    {
        One2AnyConnection[] toReturn = new One2AnyConnection[n];
        for (int i = 0; i < n; i++)
            toReturn[i] = createOne2Any();
        return toReturn;
    }

    /**
     * @see ConnectionArrayFactory#createAny2Any
     */
    public Any2AnyConnection[] createAny2Any(int n)
    {
        Any2AnyConnection[] toReturn = new Any2AnyConnection[n];
        for (int i = 0; i < n; i++)
            toReturn[i] = createAny2Any();
        return toReturn;
    }
}
}