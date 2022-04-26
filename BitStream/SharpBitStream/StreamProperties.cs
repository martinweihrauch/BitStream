using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBitStream
{
    internal class StreamProperties
    {
        public Stream Stream { get; set; }
        public long PositionByte { get; set; }
        public int PositionBit { get; set; }
        public byte CurrentByte { get; set; }
    }
}
