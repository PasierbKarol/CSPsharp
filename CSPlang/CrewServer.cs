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

using CSPlang.Alting;

namespace CSPlang
{

    /**
     * @author P.H.Welch
     */
    class CrewServer : IamCSProcess
    {

        public /*static*/ const int READER = 0;
        public /*static*/ const int WRITER = 1;

        private readonly AltingChannelInputInt request;
        private readonly AltingChannelInputInt writerControl;
        private readonly AltingChannelInputInt readerRelease;

        ///TODO change this to use poisoning of the above channels, once poison is added
        private readonly AltingChannelInputInt poison;

        public CrewServer(/*final*/ AltingChannelInputInt request,
        /*final*/ AltingChannelInputInt writerControl,
        /*final*/ AltingChannelInputInt readerRelease,
        /*final*/ AltingChannelInputInt poison)
        {
            this.request = request;
            this.writerControl = writerControl;
            this.readerRelease = readerRelease;
            this.poison = poison;
        }

        public void run()
        {
            int nReaders = 0;
            //const 
            Alternative altMain =
      new Alternative(new Guard[] { readerRelease, request, poison });
            const int MAIN_READER_RELEASE = 0;
            const int MAIN_REQUEST = 1;
            const int MAIN_POISON = 2;
            //const 
            Alternative altWriteComplete =
      new Alternative(new Guard[] { writerControl, poison });
            const int WC_WRITER_CONTROL = 0;
            const int WC_POISON = 1;
            //const 
            Alternative altReadComplete =
      new Alternative(new Guard[] { readerRelease, poison });
            const int RC_READER_RELEASE = 0;
            const int RC_POISON = 1;
            while (true)
            {
                // invariant : (nReaders is the number of current readers) and (there are no writers)
                switch (altMain.priSelect())
                {
                    case MAIN_READER_RELEASE:
                        readerRelease.read();
                        nReaders--;
                        break;
                    case MAIN_REQUEST:
                        switch (request.read())
                        {
                            case READER:
                                nReaders++;
                                break;
                            case WRITER:
                                int n = nReaders; // tmp
                                for (int i = 0; i < n; i++)
                                {
                                    switch (altReadComplete.priSelect())
                                    {
                                        case RC_READER_RELEASE:
                                            readerRelease.read();
                                            nReaders--; // tmp
                                            break;
                                        case RC_POISON:
                                            poison.read(); // let the /*final*/izer complete
                                            return;
                                    }
                                }

                                nReaders = 0;
                                writerControl.read(); // let writer start writing
                                switch (altWriteComplete.priSelect())
                                {
                                    case WC_WRITER_CONTROL:
                                        writerControl.read(); // let writer finish writing
                                        break;
                                    case WC_POISON:
                                        poison.read(); // let the /*final*/izer complete
                                        return;
                                }

                                break;
                        }

                        break;
                    case MAIN_POISON:
                        poison.read(); // let the /*final*/izer complete
                        return;
                }
            }
        }
    }
}