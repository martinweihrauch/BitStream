using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBitStream
{
    internal class Writer
    {
        private StreamProperties _properties;
        public Writer(StreamProperties streamProperties)
        {
            _properties = streamProperties;
        }

 
        public long ConvertUnsignedToSigned(ulong value, int bitLength)
        {
            long temp = (long)value & 1 << bitLength - 1;
            if (temp != 0) // != 0 means there is a 1 at the most significant bit
            {
                temp = -1 * (~(long)value + 1 & ~(~(long)0 << bitLength));
            }
            else
            {
                temp = (long)value;
            }
            return temp;
        }

        public ulong ConvertSignedToUnsigned(long value, int bitLength)
        {
            ulong temp = (ulong)(value & ~(~(long)0 << bitLength));
            return temp;
        }

        public void CheckIfWriteIsLegit(ulong value, int bitLength)
        {
            if (value > Math.Pow(2, bitLength) - 1) // If with 8 bits it is over > 255, etc
            {

            }
        }

        public void CheckIfReadIsValid(int bitLength, long offsetByteStream = 0, int offsetBit = 0)
        {
            if (offsetByteStream * 8 + offsetBit + bitLength > _properties.Stream.Length * 8)
            {
                throw new InvalidOperationException("You are trying to read more data than the stream offers at: offsetByteStream: "
                    + offsetByteStream + " / offsetBit: " + offsetBit);
            }
        }


        public bool IsBitSet(byte b, int bitPosition)
        {
            return (b & 1 << bitPosition) != 0;
        }

         public ulong PutBits(long offsetByteStream, int offsetBit, int bitLength, ulong sourceValue)
        {
            _properties.PositionBit = offsetBit;
            int bitLeft = bitLength;
            _properties.Stream.Position = offsetByteStream;
            _properties.CurrentByte = (byte)_properties.Stream.ReadByte();
            _properties.Stream.Position = offsetByteStream; // Rewind to the byte, because has to be rewritten
            int sourceValueBitOffset = 64 - bitLeft; // ulong has 64 bit, we read from left to right

            while (bitLeft > 0)
            {
                int restBitsInCurrentByte = 8 - _properties.PositionBit;
                int numberBitsToWriteIntoCurrentByte = bitLeft > restBitsInCurrentByte ? restBitsInCurrentByte : bitLeft;
                byte tempSource = (byte)sourceValue; // Get the most right 8 bits
                sourceValue >>= numberBitsToWriteIntoCurrentByte;

                _properties.CurrentByte = Common.CopyBitsIntoByte( , _properties.CurrentByte, _properties.PositionBit, 8 - numberBitsToWriteIntoCurrentByte, numberBitsToWriteIntoCurrentByte);




                _properties.PositionBit += numberBitsToWriteIntoCurrentByte;
                if (_properties.PositionBit == 8)
                {
                    _properties.PositionBit = 0;
                }
                bitLeft -= numberBitsToWriteIntoCurrentByte;
            }
            return sourceValue;
        }

    }
}
