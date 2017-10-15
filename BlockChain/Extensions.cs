using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainSecurity
{
    public static class Extensions
    {
        public static uint ToUnixTimeStamp(this DateTime timestamp)
        {
            return (uint)(timestamp.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
