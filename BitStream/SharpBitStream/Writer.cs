using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBitStream
{
    internal class Writer
    {
        private Stream _stream;
        private long _positionByte;
        private int _positionBit;
        private byte _currentByte;

        public Writer(Stream bytestream)
        {
            _positionByte = bytestream.Position;
            _positionBit = 0;
            _stream = bytestream;
        }

        public ulong WriteUnsigned(long offsetByteStream, int offsetBit, int bitLength, ulong value)
        {
            return PutBits(offsetByteStream, offsetBit, bitLength, value);
        }

        public ulong WriteUnsigned(int bitLength, ulong value)
        {
            return PutBits(_positionByte, _positionBit, bitLength, value);
        }

        public long WriteSigned(long offsetByteStream, int offsetBit, int bitLength, long value)
        {
            ulong val = ConvertSignedToUnsigned(value, bitLength);
            ulong val = PutBits(offsetByteStream, offsetBit, bitLength);
            return ConvertUnsignedToSigned(val, bitLength);
        }

        public long WriteSigned(int bitLength, long value)
        {
            ulong val = PutBits(_positionByte, _positionBit, bitLength);
            return ConvertUnsignedToSigned(val, bitLength);
        }

        public void SetPosition(long bytePosition, int bitPosition)
        {
            _positionByte = bytePosition;
            _positionBit = bitPosition;
        }

        public long FlushBitPosition()
        {
            _positionByte++;
            _positionBit = 0;
            return _positionByte;
        }

        private long ConvertUnsignedToSigned(ulong value, int bitLength)
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

        private ulong ConvertSignedToUnsigned(long value, int bitLength)
        {
            ulong temp = (ulong)(value & ~(~(long)0 << bitLength));
            return temp;
        }

        private void CheckIfWriteIsLegit(ulong value, int bitLength)
        {
            if (value > Math.Pow(2, bitLength) - 1) // If with 8 bits it is over > 255, etc
            {

            }
        }

        private void CheckIfReadIsValid(int bitLength, long offsetByteStream = 0, int offsetBit = 0)
        {
            if (offsetByteStream * 8 + offsetBit + bitLength > _stream.Length * 8)
            {
                throw new InvalidOperationException("You are trying to read more data than the stream offers at: offsetByteStream: "
                    + offsetByteStream + " / offsetBit: " + offsetBit);
            }
        }


        private bool IsBitSet(byte b, int bitPosition)
        {
            return (b & 1 << bitPosition) != 0;
        }

        public long GetSignedBits(long offsetByteStream, int offsetBit, int bitLength)
        {
            ulong result = PutBits(offsetByteStream, offsetBit, bitLength);
            return ConvertUnsignedToSigned(result, bitLength);
        }

        public ulong GetUnsignedBits(long offsetByteStream, int offsetBit, int bitLength)
        {
            return PutBits(offsetByteStream, offsetBit, bitLength);
        }

        private ulong PutBits(long offsetByteStream, int offsetBit, int bitLength, ulong valueOfBits)
        {
            _positionBit = offsetBit;
            int bitLeft = bitLength;
            _stream.Position = offsetByteStream;
            while (bitLeft > 0)
            {
                int restBitsInCurrentByte = 8 - _positionBit;
                _currentByte = (byte)_stream.ReadByte();
                _positionByte = _stream.Position;
                int numberBitsToGetFromCurrentByte = bitLeft > restBitsInCurrentByte ? restBitsInCurrentByte : bitLeft;
                valueOfBits <<= numberBitsToGetFromCurrentByte;
                valueOfBits += BitwiseCopy(_currentByte, _positionBit, 8 - numberBitsToGetFromCurrentByte, numberBitsToGetFromCurrentByte);
                _positionBit += numberBitsToGetFromCurrentByte;
                if (_positionBit == 8)
                {
                    _positionBit = 0;
                }
                bitLeft -= numberBitsToGetFromCurrentByte;
            }
            return valueOfBits;
        }

        private byte BitwiseCopy(byte sourceByte, int sourceStartBit, int destStartBit, int bitCount)
        {
            return (byte)((byte)((byte)(sourceByte << sourceStartBit) >> destStartBit)
                & (byte)(0xff << 8 - bitCount - destStartBit));
        }
    }
}
