/*
 * BitStream was written and is copyrighted by Martin Weihrauch (c) 2022
 * It is open source and can be used under MIT license * 
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBitStream
{
    public class BitStream
    {
        private StreamProperties _properties;
        private Reader _reader;
        private Writer _writer;

        public BitStream(Stream bytestream)
        {
            _properties = new StreamProperties()
            {
                Stream = bytestream,
                PositionByte = bytestream.Position,
                PositionBit = 0,
                CurrentByte = 0
            };
            _reader = new Reader(_properties);
            _writer = new Writer(_properties);
        }

        public ulong ReadUnsigned(long offsetByteStream, int offsetBit, int bitLength)
        {
            _properties.PositionBit = offsetBit;
            _properties.PositionByte = offsetByteStream;
            return _reader.GetBits(offsetByteStream, offsetBit, bitLength);
        }

        public ulong ReadUnsigned(int bitLength)
        {
            return _reader.GetBits(_properties.PositionByte, _properties.PositionBit, bitLength);
        }

        public long ReadSigned(long offsetByteStream, int offsetBit, int bitLength)
        {
            _properties.PositionBit = offsetBit;
            _properties.PositionByte = offsetByteStream;
            ulong val = _reader.GetBits(offsetByteStream, offsetBit, bitLength);
            return Common.ConvertUnsignedToSigned(val, bitLength);
        }

        public long ReadSigned(int bitLength)
        {
            ulong val = _reader.GetBits(_properties.PositionByte, _properties.PositionBit, bitLength);
            return Common.ConvertUnsignedToSigned(val, bitLength);
        }

        public void WriteUnsigned(long offsetByteStream, int offsetBit, int bitLength, ulong value)
        {
            _properties.PositionBit = offsetBit;
            _properties.PositionByte = offsetByteStream;
            _writer.PutBits(offsetByteStream, offsetBit, bitLength, value);
        }

        public void WriteUnsigned(int bitLength, ulong value)
        {
            _writer.PutBits(_properties.PositionByte, _properties.PositionBit, bitLength, value);
        }

        public void WriteSigned(long offsetByteStream, int offsetBit, int bitLength, long value)
        {
            _properties.PositionBit = offsetBit;
            _properties.PositionByte = offsetByteStream;
            _writer.PutBits(offsetByteStream, offsetBit, bitLength, (ulong)value);
        }

        public void WriteSigned(int bitLength, long value)
        {
            _writer.PutBits(_properties.PositionByte, _properties.PositionBit, bitLength, (ulong)value);
        }

        public bool IsWriteValid(int bitLength, long value)
        {
            if(value >= 0)
            {
                if (value < Math.Pow(2, bitLength)){
                    return true;
                }
            }
            else
            {
                value *= -1;
                if (value <= Math.Pow(2, bitLength - 1))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsReadValid(int bitLength, long offsetByteStream = 0, int offsetBit = 0)
        {
            if (_reader.CheckIfReadIsValid(bitLength, offsetByteStream, offsetBit))
            {
                return true;
            }
            return false;
        }
        public bool IsReadValid(int bitLength)
        {
            if (_reader.CheckIfReadIsValid(bitLength, _properties.PositionByte, _properties.PositionBit))
            {
                return true;
            }
            return false;
        }

        public Position GetPosition()
        {
            return new Position(_properties.PositionByte, _properties.PositionBit);
        }

        public void SetPosition(long bytePosition, int bitPosition)
        {
            _properties.PositionByte = bytePosition;
            _properties.PositionBit = bitPosition;
        }
        public void SetPosition(Position position)
        {
            _properties.PositionByte = position.BytePosition;
            _properties.PositionBit = position.BitPosition;
        }


        public long FlushBitPosition()
        {
            if(_properties.PositionBit != 0)
            {
                _properties.PositionBit = 0;
                _properties.PositionByte++;
            }
            return _properties.PositionByte;
        }
    
    }
}
