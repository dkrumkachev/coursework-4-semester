using Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Common.Encryption
{
    public static class Encryption
    {
        public readonly struct DHParameters 
        {
            public readonly BigInteger P { get; init; }

            public readonly int G { get; init; }
        }
        
        public static BigInteger RandomBigInteger()
        {
            return 0;
        }

        public static byte[] KeyFromBigInteger(BigInteger number, int size)
        {
            return new byte[size];
        }

        public static string SHA256Hash(byte[] bytes)
        {
            byte[] hash = System.Security.Cryptography.SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }

    }
}
