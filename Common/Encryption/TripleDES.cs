using Common.Encryption;
using Org.BouncyCastle.Crypto.Paddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Encryption
{
    public class TripleDES : DES
    {
        private const int KeysNumber = 3;
        public new const int KeySize = DES.KeySize * KeysNumber;
        private byte[] key = new byte[KeySize / 8];
        private BitsArray keyBits = new();
        private readonly BitsArray[][] subkeys = new BitsArray[KeysNumber][];
        private readonly BitsArray[][] reversedSubkeys = new BitsArray[KeysNumber][];

        public new byte[] Key
        {
            get { return key; }
            set
            {
                if (value.Length > KeySize / 8)
                {
                    value = value[..(KeySize / 8)];
                }
                key = value;
                keyBits = new BitsArray(key);
                GenerateSubkeysArray(keyBits);
            }
        }

        public override byte[] Encrypt(byte[] bytes, bool decrypt = false)
        {
            List<BitsArray> blocks = SplitIntoBlocks(bytes);
            if (!decrypt)
            {
                for (int i = 0; i < blocks.Count; i++)
                {
                    blocks[i] = CipherBlock(blocks[i], subkeys[0]);
                    blocks[i] = CipherBlock(blocks[i], reversedSubkeys[1]);
                    blocks[i] = CipherBlock(blocks[i], subkeys[2]);
                }
            }
            else
            {
                for (int i = 0; i < blocks.Count; i++)
                {
                    blocks[i] = CipherBlock(blocks[i], reversedSubkeys[2]);
                    blocks[i] = CipherBlock(blocks[i], subkeys[1]);
                    blocks[i] = CipherBlock(blocks[i], reversedSubkeys[0]);
                }
            }
            return new BitsArray(blocks).ToByteArray();
        }

        public override byte[] GenerateKey()
        {
            Array.Copy(base.GenerateKey(), 0, key, 0, DES.KeySize);
            Array.Copy(base.GenerateKey(), 0, key, DES.KeySize, DES.KeySize);
            Array.Copy(base.GenerateKey(), 0, key, 2 * DES.KeySize, DES.KeySize);
            keyBits = new BitsArray(key);
            GenerateSubkeysArray(keyBits);
            return key;
        }

        private BitsArray[][] GenerateSubkeysArray(BitsArray key)
        {
            var keys = new BitsArray[KeysNumber]
            {
                key.GetRange(0, DES.KeySize),
                key.GetRange(DES.KeySize, DES.KeySize),
                key.GetRange(DES.KeySize * 2, DES.KeySize)
            };
            for (int i = 0; i < keys.Length; i++)
            {
                subkeys[i] = base.GenerateSubkeys(keys[i]);
                reversedSubkeys[i] = (BitsArray[])subkeys[i].Clone();
                Array.Reverse(reversedSubkeys[i]);
            }
            return subkeys;
        }
    }
}
