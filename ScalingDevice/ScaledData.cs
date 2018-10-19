using System;
using System.Collections.Generic;
using System.Text;

namespace ScalingDevice
{
    [Serializable]
    class ScaledData
    {
        public int original { get; set; }
        public int scaled { get; set; }


        public String toString()
        {
            string s = " " + original + "\t\t" + scaled;

            return s;
        }
    }
}
