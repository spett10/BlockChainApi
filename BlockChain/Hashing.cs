using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainSecurity
{
    public static class Hashing
    {
        public static string Hash(byte[] message)
        {
            using (var hashalg = new SHA256CryptoServiceProvider())
            {
                var res = hashalg.ComputeHash(message, 0, message.Length);
                return BitConverter.ToString(res).Replace("-", "").ToString();
            }                
        }

    }
}
