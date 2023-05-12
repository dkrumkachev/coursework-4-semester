using Common.Encryption;
using System.Collections;
using System.Security.Cryptography;
using static System.Reflection.Metadata.BlobBuilder;

namespace Common.Encryption
{
    public class DES
    {
        public const int BlockSize = 64;
        public const int KeySize = 64;
        public const int EffectiveKeySize = 56;
        protected const int SubkeySize = 48;
        protected const int RoundsNumber = 16;

        #region Permutation tables and S-boxes (WITH VULNERABILITIES LEFT BY THE AMERICANS)

        private readonly int[] InitialPermutation = new int[BlockSize] {
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7,
            56, 48, 40, 32, 24, 16, 8, 0,
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6
        };

        private readonly int[] FinalPermutation = new int[BlockSize] {
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25,
            32, 0, 40, 8, 48, 16, 56, 24
        };

        private readonly int[] ExpansionPermutation = new int[SubkeySize] {
            31, 0, 1, 2, 3, 4,
            3, 4, 5, 6, 7, 8,
            7, 8, 9, 10, 11, 12,
            11, 12, 13, 14, 15, 16,
            15, 16, 17, 18, 19, 20,
            19, 20, 21, 22, 23, 24,
            23, 24, 25, 26, 27, 28,
            27, 28, 29, 30, 31, 0,
        };

        private readonly int[] PermutationBox = new int[BlockSize / 2]
        {
            15, 6, 19, 20,
            28, 11, 27, 16,
            0, 14, 22, 25,
            4, 17, 30, 9,
            1, 7, 23, 13,
            31, 26, 2, 8,
            18, 12, 29, 5,
            21, 10, 3, 24,
        };

        private readonly int[] PermutedChoice1 = new int[EffectiveKeySize]
        {
            56, 48, 40, 32, 24, 16, 8,
            0, 57, 49, 41, 33, 25, 17,
            9, 1, 58, 50, 42, 34, 26,
            18, 10, 2, 59, 51, 43, 35,
            62, 54, 46, 38, 30, 22, 14,
            6, 61, 53, 45, 37, 29, 21,
            13, 5, 60, 52, 44, 36, 28,
            20, 12, 4, 27, 19, 11, 3
        };

        private readonly int[] PermutedChoice2 = new int[SubkeySize]
        {
            13, 16, 10, 23, 0, 4,
            2, 27, 14, 5, 20, 9,
            22, 18, 11, 3, 25, 7,
            15, 6, 26, 19, 12, 1,
            40, 51, 30, 36, 46, 54,
            29, 39, 50, 44, 32, 47,
            43, 48, 38, 55, 33, 52,
            45, 41, 49, 35, 28, 31
        };

        private readonly int[] BitsRotation = new int[RoundsNumber]
        {
            1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
        };

        private readonly int[,,] SubstitutionBoxes = new int[8, 4, 16]
        {

            {
                { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
            },
            {
                {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
                {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 },
            },
            {
                { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
                { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
            },
            {
                { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
            },
            {
                { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
                { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
                { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
            },
            {
                { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
                { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
            },
            {
                { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
                { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
            },
            {
                { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
            }
        };
        #endregion

        private BitsArray[] subkeys = new BitsArray[RoundsNumber];
        private BitsArray[] reversedSubkeys = new BitsArray[RoundsNumber];
        private byte[] key = new byte[KeySize / sizeof(byte)];
        private BitsArray keyBits = new();

        protected BitsArray Permutation(BitsArray block, int[] permutationArray)
        {
            var permutatedBlock = new BitsArray(permutationArray.Length);
            for (int i = 0; i < permutationArray.Length; i++)
            {
                permutatedBlock[i] = block[permutationArray[i]];
            }
            return permutatedBlock;
        }

        protected void AddUpTo64BitsBoundary(ref byte[] bytes)
        {
            int length = bytes.Length;
            if (length % sizeof(byte) != 0)
            {
                Array.Resize(ref bytes, length + sizeof(byte) - length % sizeof(byte));
            }
        }

        protected List<BitsArray> SplitIntoBlocks(byte[] bytes)
        {
            AddUpTo64BitsBoundary(ref bytes);
            List<BitsArray> blocks = new List<BitsArray>(bytes.Length / sizeof(byte));
            for (int i = 0; i < bytes.Length / sizeof(byte); i++)
            {
                int blockStart = i * sizeof(byte);
                int blockEnd = blockStart + sizeof(byte);
                byte[] bytesBlock = bytes[blockStart..blockEnd];
                blocks.Add(new BitsArray(bytesBlock));
            }
            return blocks;
        }

        private static bool IsKeyWeakOrSemiweak(byte[] key)
        {
            ulong half1 = BitConverter.ToUInt64(key.AsSpan()[..(key.Length / 2)]);
            ulong half2 = BitConverter.ToUInt64(key.AsSpan()[(key.Length / 2)..]);
            return (half1 == 0x01010101u && half2 == 0x01010101u ||
                    half1 == 0xFEFEFEFEu && half2 == 0xFEFEFEFEu ||
                    half1 == 0x1F1F1F1Fu && half2 == 0x0E0E0E0Eu ||
                    half1 == 0xE0E0E0E0u && half2 == 0xF1F1F1F1u ||
                    half1 == 0x01FE01FEu && half2 == 0x01FE01FEu ||
                    half1 == 0xFE01FE01u && half2 == 0xFE01FE01u ||
                    half1 == 0x1FE01FE0u && half2 == 0x1FE01FE0u ||
                    half1 == 0xE0F1E0F1u && half2 == 0xE0F1E0F1u ||
                    half1 == 0x01E001E0u && half2 == 0x01F101F1u ||
                    half1 == 0xE001E001u && half2 == 0xF101F101u ||
                    half1 == 0x1FFE1FFEu && half2 == 0x0EFE0EFEu ||
                    half1 == 0xFE1FFE1Fu && half2 == 0xFE0EFE0Eu ||
                    half1 == 0x011F011Fu && half2 == 0x010E010Eu ||
                    half1 == 0x1F011F01u && half2 == 0x0E010E01u ||
                    half1 == 0xE0FEE0FEu && half2 == 0xF1FEF1FEu ||
                    half1 == 0xFEE0FEE0u && half2 == 0xFEF1FEF1u);
        }

        public virtual byte[] Encrypt(byte[] bytes, byte[] key, bool decrypt = false)
        {
            List<BitsArray> blocks = SplitIntoBlocks(bytes);
            if (key != this.key)
            {
                this.key = key;
                keyBits = new BitsArray(key);
                GenerateSubkeys(keyBits);
            }
            var subkeysInOrder = decrypt ? reversedSubkeys : subkeys;
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i] = CipherBlock(blocks[i], subkeysInOrder);
            }
            return new BitsArray(blocks).ToByteArray();
        }

        public virtual byte[] GenerateKey()
        {
            byte[] key = new byte[KeySize / sizeof(byte)];
            var random = new Random();
            for (int i = 0; i < key.Length; i++)
            {
                key[i] = (byte)random.Next(128);
                int onesCount = 0;
                for (int j = 0; j < sizeof(byte) - 1; j++)
                {
                    onesCount += key[i] & (1 << j);
                }
                if (onesCount % 2 == 0)
                {
                    key[i] |= 0b_10000000;
                }
            }
            return IsKeyWeakOrSemiweak(key) ? GenerateKey() : key;
        }

        protected BitsArray[] GenerateSubkeys(BitsArray key)
        {
            var subkeys = new BitsArray[RoundsNumber];
            key = Permutation(key, PermutedChoice1);
            var left = key.GetRange(0, EffectiveKeySize / 2);
            var right = key.GetRange(EffectiveKeySize / 2, EffectiveKeySize / 2);
            for (int i = 0; i < RoundsNumber; i++)
            {
                left.CircularLeftShift(BitsRotation[i]);
                right.CircularLeftShift(BitsRotation[i]);
                subkeys[i] = Permutation(new BitsArray(left, right), PermutedChoice2);
            }
            this.subkeys = subkeys;
            reversedSubkeys = (BitsArray[])subkeys.Clone();
            Array.Reverse(reversedSubkeys);
            return subkeys;
        }

        protected BitsArray CipherBlock(BitsArray block, BitsArray[] subkeys)
        {
            block = Permutation(block, InitialPermutation);
            var left = block.GetRange(0, block.Length / 2);
            var right = block.GetRange(block.Length / 2, block.Length / 2);
            for (int i = 0; i < RoundsNumber; i++)
            {
                BitsArray roundResult = CipherRound(left, right, subkeys[i]);
                left = right;
                right = roundResult;
            }
            right.Join(left);
            block = Permutation(right, FinalPermutation);
            return block;
        }

        private BitsArray CipherRound(BitsArray left, BitsArray right, BitsArray subkey)
        {
            return FeistelFunction(right, subkey).Xor(left);
        }

        private BitsArray FeistelFunction(BitsArray halfBlock, BitsArray subkey)
        {
            const int SboxInputBitsNumber = 6;
            const int SboxOutputBitsNumber = 4;

            BitsArray expandedBlock = Permutation(halfBlock, ExpansionPermutation);
            expandedBlock.Xor(subkey);
            BitsArray[] SBoxOutputs = new BitsArray[SubstitutionBoxes.GetLength(0)];
            for (int i = 0; i < SubstitutionBoxes.GetLength(0); i++)
            {
                BitsArray SboxInput = expandedBlock.GetRange(i * SboxInputBitsNumber, SboxInputBitsNumber);
                int row = SboxInput[0] + (SboxInput[SboxInputBitsNumber - 1] << 1);
                int column = SboxInput.GetRange(1, 4).ToByte();
                SBoxOutputs[i] = new BitsArray(SboxOutputBitsNumber, SubstitutionBoxes[i, row, column]);
            }
            expandedBlock = new BitsArray(SBoxOutputs);
            return Permutation(expandedBlock, PermutationBox);
        }
    }
}
