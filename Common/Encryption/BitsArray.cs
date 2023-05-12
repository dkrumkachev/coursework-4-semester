using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Encryption
{
    public class BitsArray
    {
        private bool[] bits;
        public int Length { get { return bits.Length; } private set { Array.Resize(ref bits, value); } }

        public int this[int index]
        {
            get { return bits[index] ? 1 : 0; }
            set { bits[index] = value != 0; }
        }

        public BitsArray()
        {
            bits = Array.Empty<bool>();
        }

        public BitsArray(int size)
        {
            bits = new bool[size];
        }

        public BitsArray(byte[] values)
        {
            bits = new bool[values.Length * sizeof(byte)];
            for (int i = 0; i < values.Length; i++)
            {
                for (int j = 0; j < sizeof(byte) * sizeof(byte); j++)
                {
                    this[i * sizeof(byte) + j] = (values[i] & (1 << j));
                }
            }
        }

        public BitsArray(int[] values)
        {
            bits = new bool[values.Length * sizeof(byte)];
            for (int i = 0; i < values.Length; i++)
            {
                for (int j = 0; j < sizeof(int) * sizeof(byte); j++)
                {
                    this[i * sizeof(byte) + j] = (values[i] & (1 << j));
                }
            };
        }

        public BitsArray(int size, int value) : this(size)
        {
            for (int i = 0; i < size; i++)
            {
                this[i] = value & (1 << i);
            }
        }

        public BitsArray(IList<BitsArray> others) : this(0)
        {
            Join(others);
        }

        public BitsArray(params BitsArray[] others) : this(0)
        {
            Join(others);
        }

        public BitsArray GetRange(int startIndex, int length)
        {
            if (startIndex + length > Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            BitsArray subArray = new BitsArray(length);
            for (int i = startIndex; i < startIndex + length; i++)
            {
                subArray.bits[i - startIndex] = bits[i];
            }
            return subArray;
        }

        public BitsArray Join(params BitsArray[] others)
        {
            return Join(others.AsEnumerable());
        }

        public BitsArray Join(IEnumerable<BitsArray> others)
        {
            int newLength = bits.Length;
            foreach (var other in others)
            {
                newLength += other.Length;
            }
            int currentLength = bits.Length;
            Length += newLength;
            foreach (var other in others)
            {
                for (int i = 0; i < other.Length; i++)
                {
                    bits[i + currentLength] = other.bits[i];
                }
                currentLength += other.Length;
            }
            return this;
        }

        public BitsArray Xor(BitsArray other)
        {
            int smallerLength = int.Min(bits.Length, other.Length);
            for (int i = 0; i < smallerLength; i++)
            {
                bits[i] = bits[i] ^ other.bits[i];
            }
            return this;
        }

        public BitsArray LeftShift(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (count >= bits.Length)
            {
                Array.Fill(bits, false);
                return this;
            }
            for (int i = bits.Length - 1; i >= count; i--)
            {
                bits[i] = bits[i - count];
            }
            Array.Fill(bits, false, 0, count);
            return this;
        }

        public BitsArray CircularLeftShift(int count)
        {
            var ejectedBits = GetRange(Length - count, count);
            LeftShift(count);
            for (int i = 0; i < count; i++)
            {
                bits[i] = ejectedBits.bits[i];
            }
            return this;
        }

        public byte ToByte()
        {
            if (bits.Length > sizeof(byte))
            {
                throw new InvalidCastException();
            }
            byte value = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                value += (byte)(this[i] << i);
            }
            return value;
        }

        public ulong ToUlong()
        {
            if (bits.Length > 64)
            {
                throw new InvalidCastException();
            }
            ulong value = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                value += (uint)this[i] << i;
            }
            return value;
        }

        public byte[] ToByteArray()
        {
            if (bits.Length % sizeof(byte) != 0)
            {
                Length += sizeof(byte) - bits.Length % sizeof(byte);
            }
            byte[] bytes = new byte[bits.Length / sizeof(byte)];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)GetRange(i * sizeof(byte), sizeof(byte)).ToByte();
            }
            return bytes;
        }
    }
}

