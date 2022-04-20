using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitStream
{
    public class Reader
    {
        private Stream _stream;
        private long _positionByte;
        private int _positionBit;
        private bool _littleEndian;
        private byte _currentByte;

        public Reader(Stream bytestream, bool littleEndian = true)
        {
            _positionByte = bytestream.Position;
            _positionBit = 0;
            _stream = bytestream;
            _littleEndian = littleEndian;
        }

        public uint ReadUnsigned(long offsetByteStream, int offsetBit, int bitLength)
        {
            return GetBits(offsetByteStream, offsetBit, bitLength);   
        }

        public uint ReadUnsigned(int bitLength)
        {
            return GetBits(_positionByte, _positionBit, bitLength);
        }

        public int ReadSigned(long offsetByteStream, int offsetBit, int bitLength)
        {
            uint val = GetBits(offsetByteStream, offsetBit, bitLength);
            
        }

        public int ReadSigned(int bitLength)
        {
            return GetBits(_positionByte, _positionBit, bitLength);
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

        private int ConvertUnsignedToSigned(uint value, int bitLength)
        {
            long temp = value & (1 << (bitLength - 1));
            if (temp != 0) // != 0 means there is a 1 at the most significant bit
            {
                return (int)(-1 * (value & ~(1 << (bitLength - 1))));
            }
            else
            {
                return (int)(-1 * value);
            }
        }

        private bool CheckIfReadIsValid(int bitLength, long offsetByteStream = 0, int offsetBit = 0)
        {
            if(offsetByteStream * 8 + offsetBit + bitLength > _stream.Length * 8)
            {
                return false;
            }
            return true;

        }

        private bool IsBitSet(byte b, int bitPosition)
        {
            return (b & (1 << bitPosition)) != 0;
        }

        public int GetSignedBits(long offsetByteStream, int offsetBit, int bitLength)
        {
            uint result = GetBits(offsetByteStream, offsetBit, bitLength);
        }

        public uint GetUnsignedBits(long offsetByteStream, int offsetBit, int bitLength)
        {
            return GetBits(offsetByteStream, offsetBit, bitLength);
        }

        private uint GetBits(long offsetByteStream, int offsetBit, int bitLength)
        {
            _positionBit = offsetBit;
            uint valueOfBits = 0;
            int bitLeft = bitLength;
            _stream.Position = offsetByteStream;
            while(bitLeft > 0)
            {
                int restBitsInCurrentByte = 8 - _positionBit;
                _currentByte = (byte)_stream.ReadByte();
                _positionByte = _stream.Position;
                int numberBitsToGetFromCurrentByte = bitLeft > restBitsInCurrentByte ? restBitsInCurrentByte : bitLeft;
                valueOfBits <<= numberBitsToGetFromCurrentByte;
                valueOfBits += BitwiseCopy(_currentByte, _positionBit, 8-numberBitsToGetFromCurrentByte, numberBitsToGetFromCurrentByte);
                _positionBit += numberBitsToGetFromCurrentByte;
                if(_positionBit == 8)
                {
                    _positionBit = 0;
                }
                bitLeft -= numberBitsToGetFromCurrentByte;
            }
            return valueOfBits;
        }

        private byte BitwiseCopy(byte sourceByte, int sourceStartBit, int destStartBit, int bitCount)
        {
            return (byte)(((byte)((byte)(sourceByte << sourceStartBit) >> destStartBit)) 
                & (byte)(0xff << 8 - bitCount - destStartBit));
        }

    }
}
