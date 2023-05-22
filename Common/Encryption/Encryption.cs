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
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Security;

namespace Common.Encryption
{
    public static class Encryption
    {
        public const int ECDHKeySize = 32;
        public const string ECName = "P-256";
        private static readonly X9ECParameters curveParams = NistNamedCurves.GetByName(ECName);
        private static readonly ECDomainParameters domainParams = 
            new (curveParams.Curve, curveParams.G, curveParams.N, curveParams.H);

        public static BigInteger GeneratePrivateKey()
        {
            byte[] privateKeyBytes = new byte[ECDHKeySize];
            var random = new Random();
            random.NextBytes(privateKeyBytes);
            return new BigInteger(1, privateKeyBytes);
        }

        public static byte[] GetPublicKey(BigInteger privateKey)
        {
            ECPoint publicKey = domainParams.G.Multiply(privateKey);
            return publicKey.GetEncoded();
        }

        public static byte[] MultiplyByPrivateKey(byte[] publicKey, BigInteger privateKey)
        {
            ECPoint publicKeyPoint = curveParams.Curve.DecodePoint(publicKey);
            return publicKeyPoint.Multiply(privateKey).GetEncoded();
        }

        public static byte[] CropTheKey(byte[] key)
        {
            return key[..(TripleDES.KeySize / 8)];
        }

        public static string SHA256Hash(byte[] bytes)
        {
            byte[] hash = System.Security.Cryptography.SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }

    }
}
