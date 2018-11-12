//////////////////////////////////////////////////////////////////////
//                                                                  //
//  JCSP ("CSP for Java") Libraries                                 //
//  Copyright (C) 1996-2018 Peter Welch, Paul Austin and Neil Brown //
//                2001-2004 Quickstone Technologies Limited         //
//                2005-2018 Kevin Chalmers                          //
//                                                                  //
//  You may use this work under the terms of either                 //
//  1. The Apache License, Version 2.0                              //
//  2. or (at your option), the GNU Lesser General Public License,  //
//       version 2.1 or greater.                                    //
//                                                                  //
//  Full licence texts are included in the LICENCE file with        //
//  this library.                                                   //
//                                                                  //
//  Author contacts: P.H.Welch@kent.ac.uk K.Chalmers@napier.ac.uk   //
//                                                                  //
//////////////////////////////////////////////////////////////////////

using System;

namespace CSPlang.Alting
{

    class AltingChannelOutputSymmetricImpl : AltingChannelOutput, MultiwaySynchronisation
    {

        private readonly AltingBarrier ab;

        private readonly ChannelOutput Out;

        private Boolean syncDone = false;

        public AltingChannelOutputSymmetricImpl(
            AltingBarrier ab, ChannelOutput Out)
        {
            this.ab = ab;
            this.Out = Out;
        }

        public override Boolean enable(Alternative alt)
        {
            syncDone = ab.enable(alt);
            return syncDone;
        }

        public override Boolean disable()
        {
            syncDone = ab.disable();
            return syncDone;
        }

        public void write(Object o)
        {
            if (!syncDone) ab.sync();
            syncDone = false;
            Out.write(o);
        }

        public override Boolean pending()
        {
            syncDone = ab.poll(10);
            return syncDone;
        }

        public void poison(int strength)
        {
            Out.poison(strength);
        }



    }
}