using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBitStream
{
    public class BitStream
    {
        private Stream _stream;
        private long _positionByte;
        private int _positionBit;
        private byte _currentByte;

        public BitStream(Stream bytestream)
        {
            _positionByte = bytestream.Position;
            _positionBit = 0;
            _stream = bytestream;
        }
    }
}
