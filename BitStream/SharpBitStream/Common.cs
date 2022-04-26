using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBitStream
{
    internal static class Common
    {
		public static byte CopyBitsIntoByte(byte sourceByte, byte destinationByte, int sourceStartBit, int destStartBit, int bitCount)
		{
			int[] mask = { 0, 1, 3, 7, 15, 31, 63, 127, 255 };
			byte sourceMask = (byte)(mask[bitCount] << (8 - sourceStartBit - bitCount));
			byte destinationMask = (byte)(~(mask[bitCount] << (8 - destStartBit - bitCount)));
			byte destinationToCopy = (byte)(destinationByte & destinationMask);
			int diff = destStartBit - sourceStartBit;
			byte sourceToCopy;
			if (diff >= 0)
			{
				sourceToCopy = (byte)((sourceByte & sourceMask) >> (diff));
			}
			else
			{
				sourceToCopy = (byte)((sourceByte & sourceMask) << (diff * (-1)));
			}
			return (byte)(sourceToCopy | destinationToCopy);
		}

		/*
		 * 
		    private void Write32(ulong value, byte[] buffer, int offset)
			{
				buffer[offset++] = (byte)((value >> 24) & 0xFF);
				buffer[offset++] = (byte)((value >> 16) & 0xFF);
				buffer[offset++] = (byte)((value >> 8) & 0xFF);
				buffer[offset] = (byte)(value & 0xFF);
			}
		 */
	}
}
