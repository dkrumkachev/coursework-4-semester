using Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Org.BouncyCastle.Math;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.X9;

namespace Common.Encryption
{
    public static class Encryption
    {
        public const int ECDHKeySize = 32;
        public const string ECName = "P-256";
        
        public static ECDomainParameters GetDomainParams()
        {
            X9ECParameters curveParams = NistNamedCurves.GetByName(ECName);
            return new ECDomainParameters(curveParams.Curve, curveParams.G, curveParams.N, curveParams.H);

        }

        public static BigInteger GeneratePrivateKey()
        {
            byte[] privateKeyBytes = new byte[ECDHKeySize];
            var random = new Random();
            random.NextBytes(privateKeyBytes);
            return new BigInteger(1, privateKeyBytes);
        }

        public static byte[] GetSharedSecretBytes(ECPoint sharedSecret, int keySize)
        {
            byte[] bytes = sharedSecret.Normalize().XCoord.GetEncoded();
            return bytes[..keySize];
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
