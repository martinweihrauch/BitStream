// See https://aka.ms/new-console-template for more information
using SharpBitStream;

int[] testDataUnsigned = { 5, 62, 17, 50, 33 };
var ms = new MemoryStream();
var bs = new BitStream(ms);

foreach(var bits in testDataUnsigned)
{
    bs.WriteUnsigned(6, (ulong)bits);
}

Console.WriteLine("The size of the stream is now: " + ms.Length + "\r\nand the bytes in it are: ");

ms.Position = 0;

byte[] ba = ms.ToArray();

for (int i = 0; i < ms.Length; i++)
{
    int bits = (int)ms.ReadByte();
    Console.WriteLine("Byte #" + Convert.ToString(i).PadLeft(4, '0') + ": " + Convert.ToString(bits, 2).PadLeft(8, '0'));
}