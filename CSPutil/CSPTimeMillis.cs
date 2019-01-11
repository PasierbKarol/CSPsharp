﻿using System;

namespace CSPutil
{
    public class CSPTimeMillis
    {
        private static readonly DateTime Jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public static int CurrentTimeMillisInt()
        {
            int test = (int)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
            return test;
        }
    }
}