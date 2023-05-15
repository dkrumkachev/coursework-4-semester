using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using Common.Encryption;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using ECPoint = Org.BouncyCastle.Math.EC.ECPoint;

namespace Server
{
    public static class Program
    {
        static void Main(string[] args)
        {
            //Server server = new Server();
            //server.Run();
            X9ECParameters curveParams = NistNamedCurves.GetByName("P-256");
            ECDomainParameters domainParams = new ECDomainParameters(curveParams.Curve, curveParams.G, curveParams.N, curveParams.H);

            byte[] alicePrivateKeyBytes = new byte[32];
            byte[] bobPrivateKeyBytes = new byte[32];
            byte[] carolPrivateKeyBytes = new byte[32];

            var random = new Random();
            random.NextBytes(alicePrivateKeyBytes);
            random.NextBytes(bobPrivateKeyBytes);
            random.NextBytes(carolPrivateKeyBytes);

            BigInteger alicePrivateKey = new BigInteger(1, alicePrivateKeyBytes);
            BigInteger bobPrivateKey = new BigInteger(1, bobPrivateKeyBytes);
            BigInteger carolPrivateKey = new BigInteger(1, carolPrivateKeyBytes);

            ECPoint alicePublicKey = domainParams.G.Multiply(alicePrivateKey); //a
            ECPoint ab = alicePublicKey.Multiply(bobPrivateKey); //b
            ECPoint bobPublicKey = domainParams.G.Multiply(bobPrivateKey); //b
            ECPoint abc = ab.Multiply(carolPrivateKey); //c
            ECPoint bc = bobPublicKey.Multiply(carolPrivateKey); //c
            ECPoint carolPublicKey = domainParams.G.Multiply(carolPrivateKey); //c
            ECPoint bca = bc.Multiply(alicePrivateKey); //a
            ECPoint ca = carolPublicKey.Multiply(alicePrivateKey); //a
            ECPoint cab = ca.Multiply(bobPrivateKey); //b


            byte[] shared1 = abc.Normalize().XCoord.GetEncoded();
            byte[] shared2 = bca.Normalize().XCoord.GetEncoded();
            byte[] shared3 = cab.Normalize().XCoord.GetEncoded();
            

            if (shared1.SequenceEqual(shared2) && shared2.SequenceEqual(shared3))
            {
                Console.WriteLine(shared1.Length);
            }
            else
            {
                Console.WriteLine("Error: shared secrets do not match.");
            }

            TripleDES des = new TripleDES();
            des.Key = new byte[TripleDES.KeySize / 8];
            random = new Random();
            random.NextBytes(des.Key);
            BitsArray qa = new BitsArray(5);
            byte[] q = JsonSerializer.SerializeToUtf8Bytes(qa);
            Console.WriteLine(Encoding.UTF8.GetString(q));
            foreach (var b in q)
            {
                Console.Write(b + " ");
            }
            Console.WriteLine();
            byte[] a = des.Encrypt(q);
            a = des.Encrypt(a, true);
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == 0) 
                {
                    a = a[..i];
                    break;
                }
            }
            Console.WriteLine(Encoding.UTF8.GetString(a));

            BitsArray qw = JsonSerializer.Deserialize<BitsArray>(a) ?? throw new Exception();
            Console.WriteLine(qw.Equals(qa));
            foreach (var b in a)
            {
                Console.Write(b + " ");
            }
        }
    }
}
