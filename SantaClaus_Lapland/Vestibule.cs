using System;
using CSPlang;

namespace SantaClaus_Lapland
{
    class Vestibule : IamCSProcess
    {
        Bucket[] groups;

        // elf connection
        ChannelInput needToConsult;
        ChannelOutput joinGroup;

        // Santa connections
        ChannelInput openForBusiness;
        ChannelInput consultationOver;

        public Vestibule(Bucket[] groups, ChannelInput needToConsult, ChannelOutput joinGroup,
            ChannelInput openForBusiness, ChannelInput consultationOver)
        {
            this.groups = groups;
            this.needToConsult = needToConsult;
            this.joinGroup = joinGroup;
            this.openForBusiness = openForBusiness;
            this.consultationOver = consultationOver;
        }

        public void run()
        {
            Skip flush = new Skip();
            Alternative vAlt = new Alternative(new Guard[] { needToConsult as Guard, consultationOver as Guard, flush });
            int index = -1;
            int filling = 0;
            int[] counter = { 0, 0, 0, 0 };
            const int NEED = 0;
            const int OVER = 1;
            const int FLUSH = 2;
            Boolean[] preCon = new Boolean[3];
            preCon[NEED] = true;
            preCon[OVER] = true;
            preCon[FLUSH] = false;
            openForBusiness.read();
            int removing = 0;


            while (true)
            {
                index = vAlt.select(preCon);
                switch (index)
                {
                    case NEED:
                        needToConsult.read();
                        joinGroup.write(filling);
                        counter[filling] = counter[filling] + 1;
                        if (counter[filling] == 3) filling = (filling + 1) % 4;
                        break;
                    case OVER:
                        consultationOver.read();
                        removing = (removing + 1) % 4;
                        break;
                    case FLUSH:
                        groups[removing].flush();
                        counter[removing] = 0;
                        break;
                }

                preCon[FLUSH] = (groups[removing].holding() == 3);
                // have to ensure that the the last elf to fallInto a bucket
                // has actually done so
                // we do not rely on the fact that the coounter for the bucket has reached 3
                // we test to make sure there are three processes in the bucket  
                // which is next to be removed
                // changed as a result of the discussion at CPA2008 Fringe session from
                // preCon[FLUSH] = (counter[removing] == 3)
            }
        }
    }
}