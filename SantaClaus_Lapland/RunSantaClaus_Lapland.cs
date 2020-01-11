using System.Collections.Generic;
using CSPlang;
using CSPlang.Any2;

namespace SantaClaus_Lapland
{
    class RunSantaClaus_Lapland
    {
        static void Main(string[] args)
        {
            // synchronisation components
            AltingBarrier[] stable = AltingBarrier.create(10);
            var elfGroups = Bucket.create(4);

            // Santa to Vestibule Channels
            One2OneChannel openForBusiness = Channel.createOne2One();
            One2OneChannel consultationOver = Channel.createOne2One();

            //reindeer channels
            Any2OneChannel harness = Channel.createAny2One();
            One2AnyChannel harnessed = Channel.createOne2Any();
            One2AnyChannel returned = Channel.createOne2Any();
            One2OneChannel[] unharness = Channel.createOne2One(9);
            ChannelOutputList unharnessList = new ChannelOutputList(unharness);

            // elf channels, including Vestibule channels
            Any2OneChannel needToConsult = Channel.createAny2One();
            One2AnyChannel joinGroup = Channel.createOne2Any();
            Any2OneChannel consult = Channel.createAny2One();
            Any2OneChannel negotiating = Channel.createAny2One();
            One2OneChannel[] consulting = Channel.createOne2One(10);
            One2OneChannel[] consulted = Channel.createOne2One(10);
            ChannelOutputList consultingList = new ChannelOutputList(consulting);
            ChannelOutputList consultedList = new ChannelOutputList(consulted);

            List<IamCSProcess> grottoList = new List<IamCSProcess>();
            Reindeer[] herd = new Reindeer[9];
            for (int i = 0; i < 9; i++)
            {
                herd[i] = new Reindeer(number: i,
                    stable: stable[i],
                    harness: harness.Out(),
                    harnessed:
                    harnessed.In(),
                    returned:
                    returned.In(),
                    unharness:
                    unharness[i].In(),
                    holidayTime:
                    15000
                );
                grottoList.Add(herd[i]);
            }

            Elf[] elves = new Elf[9];
            IamCSProcess[] elvesNetwork = new IamCSProcess[8];
            for (int i = 0; i < 9; i++)
            {
                elves[i] = new Elf(number: i,
                    groups: elfGroups,
                    needToConsult: needToConsult.Out(),
                    joinGroup:
                    joinGroup.In(),
                    consult:
                    consult.Out(),
                    consulting:
                    consulting[i].In(),
                    negotiating:
                    negotiating.Out(),
                    consulted:
                    consulted[i].In(),
                    workingTime:
                    1000
                );
                grottoList.Add(elves[i]);
            }

            Santa santa = new Santa(openForBusiness: openForBusiness.Out(),
                consultationOver: consultationOver.Out(),
                harness:
                harness.In(),
                harnessed:
                harnessed.Out(),
                returned:
                returned.Out(),
                unharnessList:
                unharnessList,
                stable:
                stable[9],
                consult:
                consult.In(),
                consulting:
                consultingList,
                negotiating:
                negotiating.In(),
                consulted:
                consultedList,
                deliveryTime:
                1000,
                consultationTime:
                1000
            );

            Vestibule vestibule = new Vestibule(groups: elfGroups,
                needToConsult: needToConsult.In(),
                joinGroup: joinGroup.Out(),
                openForBusiness:
                openForBusiness.In(),
                consultationOver:
                consultationOver.In()
            );
            grottoList.Add(santa);
            grottoList.Add(vestibule);
            IamCSProcess[] grotto = grottoList.ToArray();

            new CSPParallel(grotto).run();
        }
    }
}