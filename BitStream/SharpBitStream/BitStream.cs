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
            _reader.CheckIfReadIsValid(bitLength, offsetByteStream, offsetBit);
            return _reader.GetBits(offsetByteStream, offsetBit, bitLength);
        }

        public ulong ReadUnsigned(int bitLength)
        {
            _reader.CheckIfReadIsValid(bitLength, _properties.PositionByte, _properties.PositionBit);
            return _reader.GetBits(_properties.PositionByte, _properties.PositionBit, bitLength);
        }

        public long ReadSigned(long offsetByteStream, int offsetBit, int bitLength)
        {
            _reader.CheckIfReadIsValid(bitLength, offsetByteStream, offsetBit);
            ulong val = _reader.GetBits(offsetByteStream, offsetBit, bitLength);
            return _reader.ConvertUnsignedToSigned(val, bitLength);
        }

        public long ReadSigned(int bitLength)
        {
            _reader.CheckIfReadIsValid(bitLength, _properties.PositionByte, _properties.PositionBit);
            ulong val = _reader.GetBits(_properties.PositionByte, _properties.PositionBit, bitLength);
            return _reader.ConvertUnsignedToSigned(val, bitLength);
        }

        public ulong WriteUnsigned(long offsetByteStream, int offsetBit, int bitLength, ulong value)
        {
            return _writer.PutBits(offsetByteStream, offsetBit, bitLength, value);
        }

        public ulong WriteUnsigned(int bitLength, ulong value)
        {
            return _writer.PutBits(_properties.PositionByte, _properties.PositionBit, bitLength, value);
        }

        public long WriteSigned(long offsetByteStream, int offsetBit, int bitLength, long value)
        {
            ulong val = _writer.ConvertSignedToUnsigned(value, bitLength);
            _writer.PutBits(offsetByteStream, offsetBit, bitLength, val);
            return _writer.ConvertUnsignedToSigned(val, bitLength);
        }

        public long WriteSigned(int bitLength, long value)
        {
            ulong val = _writer.PutBits(_properties.PositionByte, _properties.PositionBit, bitLength);
            return _writer.ConvertUnsignedToSigned(val, bitLength);
        }

        public void SetPosition(long bytePosition, int bitPosition)
        {
            _properties.PositionByte = bytePosition;
            _properties.PositionBit = bitPosition;
        }

        public long FlushBitPosition()
        {
            _properties.PositionByte++;
            _properties.PositionBit = 0;
            return _properties.PositionByte;
        }

        

    }
}
