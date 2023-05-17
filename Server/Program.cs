using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using Common.Encryption;
using Common.Messages;
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
            Server server = new Server();
            server.Run();
            /*string SqlConnectionString =
                "Server=localhost\\SQLEXPRESS;Database=Coursework;Trusted_Connection=True;";
            using var connection = new SqlConnection(SqlConnectionString);
            connection.Open();
            SqlCommand command = new SqlCommand("EXEC sp_rename 'Users.email', 'username', 'COLUMN'", connection);
            command.ExecuteNonQuery();
            connection.Close();*/

            /*X9ECParameters curveParams = NistNamedCurves.GetByName("P-256");
            ECDomainParameters domainParams = new ECDomainParameters(curveParams.Curve, curveParams.G, curveParams.N, curveParams.H);

            X9ECParameters curveParams2 = NistNamedCurves.GetByName("P-256");
            ECDomainParameters domainParams2 = new ECDomainParameters(curveParams.Curve, curveParams.G, curveParams.N, curveParams.H);

            byte[] alicePrivateKeyBytes = new byte[32];
            byte[] bobPrivateKeyBytes = new byte[32];

            var random = new Random();
            random.NextBytes(alicePrivateKeyBytes);
            random.NextBytes(bobPrivateKeyBytes);

            BigInteger alicePrivateKey = new BigInteger(1, alicePrivateKeyBytes);
            BigInteger bobPrivateKey = new BigInteger(1, bobPrivateKeyBytes);

            ECPoint alicePublicKey = domainParams.G.Multiply(alicePrivateKey); //a
            var encoded1 = alicePublicKey.GetEncoded();
            ECPoint bobPublicKey = domainParams2.G.Multiply(bobPrivateKey); //b
            var encoded2 = bobPublicKey.GetEncoded();
            var decoded1 = curveParams.Curve.DecodePoint(encoded1);
            var decoded2 = curveParams.Curve.DecodePoint(encoded2);
            ECPoint sharedKey1 = decoded1.Multiply(bobPrivateKey); //b
            encoded1 = sharedKey1.GetEncoded();
            ECPoint sharedKey2 = decoded2.Multiply(alicePrivateKey); //b
            encoded2 = sharedKey2.GetEncoded();
            decoded1 = curveParams.Curve.DecodePoint(encoded1);
            decoded2 = curveParams.Curve.DecodePoint(encoded2);

            foreach (var b in decoded1.GetEncoded())
            {
                Console.Write(b + " ");
            }
            Console.WriteLine();
            foreach (var b in decoded2.GetEncoded())
            {
                Console.Write(b + " ");
            }
*/
        }
    }
}
