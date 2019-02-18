using System;
using System.Collections.Generic;
using System.Text;

namespace CSPlang
{
    public class ALT
    {
        public ALT(Object[] objectGuards)
        {
            List<Guard> guards = new List<Guard>();
            foreach (var objectGuard in objectGuards)
            {
                guards.Add(objectGuard as Guard);
            }
            Alternative alt = new Alternative(guards.ToArray());
        }
    }
}
