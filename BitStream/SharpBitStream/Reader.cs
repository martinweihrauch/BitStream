using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBitStream
{
    internal class Reader
    {
        private StreamProperties _properties;
        public Reader(StreamProperties streamProperties)
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

        public bool CheckIfReadIsValid(int bitLength, long offsetByteStream = 0, int offsetBit = 0)
        {
            if (offsetByteStream * 8 + offsetBit + bitLength > _properties.Stream.Length * 8)
            {
                return false;
            }
            return true;
        }

        public bool IsBitSet(byte b, int bitPosition)
        {
            return (b & 1 << bitPosition) != 0;
        }

        public long GetSignedBits(long offsetByteStream, int offsetBit, int bitLength)
        {
            ulong result = GetBits(offsetByteStream, offsetBit, bitLength);
            return ConvertUnsignedToSigned(result, bitLength);
        }

        public ulong GetUnsignedBits(long offsetByteStream, int offsetBit, int bitLength)
        {
            return GetBits(offsetByteStream, offsetBit, bitLength);
        }

        public ulong GetBits(long offsetByteStream, int offsetBit, int bitLength)
        {
            _properties.PositionBit = offsetBit;
            ulong valueOfBits = 0;
            int bitLeft = bitLength;
            _properties.Stream.Position = offsetByteStream;
            while (bitLeft > 0)
            {
                int restBitsInCurrentByte = 8 - _properties.PositionBit;
                _properties.CurrentByte = (byte)_properties.Stream.ReadByte();
                _properties.Stream.Position--;
                _properties.PositionByte = _properties.Stream.Position;
                int numberBitsToGetFromCurrentByte = bitLeft > restBitsInCurrentByte ? restBitsInCurrentByte : bitLeft;
                valueOfBits <<= numberBitsToGetFromCurrentByte;
                byte targetByte = 0;
                valueOfBits += Common.CopyBitsIntoByte(_properties.CurrentByte, targetByte, _properties.PositionBit, 8 - numberBitsToGetFromCurrentByte, numberBitsToGetFromCurrentByte);
                _properties.PositionBit += numberBitsToGetFromCurrentByte;
                if (_properties.PositionBit == 8)
                {
                    _properties.PositionBit = 0;
                    _properties.PositionByte++;
                    _properties.Stream.Position = _properties.PositionByte;
                }
                bitLeft -= numberBitsToGetFromCurrentByte;
            }
            return valueOfBits;
        }


    }
}
