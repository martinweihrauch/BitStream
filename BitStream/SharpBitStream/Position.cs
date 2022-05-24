using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBitStream
{
    public class Position
    {
        public long BytePosition { get; set; }
        public int BitPosition { get; set; }

        public Position(long bytePosition, int bitPosition)
        {
            BytePosition = bytePosition;
            BitPosition = bitPosition;
        }

        public void AddBits(int bits)
        {
            long totalBits = BytePosition * 8 + BitPosition + bits;
            BytePosition = (long)Math.Floor((double)totalBits / 8);
            BitPosition = (int)(totalBits % 8);
        }

        public void Rewind()
        {
            BytePosition = 0;
            BitPosition = 0;
        }
    }

}
