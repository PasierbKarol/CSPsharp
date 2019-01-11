using System;
using System.Collections.Generic;
using System.Text;

namespace ScalingDevice
{
    [Serializable]
    class ScaledData
    {
        public int Original { get; set; }
        public int Scaled { get; set; }


        public String ToString()
        {
            string s = $" {Original}\t\t{Scaled}";

            return s;
        }
    }
}
