/*************************************************************************
*                                                                        *
*  JCSP ("CSP for Java") libraries                                       *
*  Copyright (C) 1996-2006 Peter Welch and Paul Austin.                  *
*                                                                        *
*  This library is free software; you can redistribute it and/or         *
*  modify it under the terms of the GNU Lesser General Public            *
*  License as published by the Free Software Foundation; either          *
*  version 2.1 of the License, or (at your option) any later version.    *
*                                                                        *
*  This library is distributed in the hope that it will be useful,       *
*  but WITHOUT ANY WARRANTY; without even the implied warranty of        *
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU     *
*  Lesser General Public License for more details.                       *
*                                                                        *
*  You should have received a copy of the GNU Lesser General Public      *
*  License along with this library; if not, write to the Free Software   *
*  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  *
*  USA.                                                                  *
*                                                                        *
*  Author contact: P.H.Welch@ukc.ac.uk                                   *
*                                                                        *
*************************************************************************/

using System;

namespace CSPlang
{

    //{{{  javadoc
    /**
     * This is thrown for an illegal operation on an Alternative.
     *
     * <H2>Description</H2>
     * Currently, there is only one cause: we cannot invoke 'priSelect' when there is
     * an AltingBarrier in the Guard array.
     *
     * @author P.H.Welch
     */
    //}}}

    public class AlternativeError : Exception
    {

        public AlternativeError(String errorMessage) : base(errorMessage)
        {

        }

    }

}