// See https://aka.ms/new-console-template for more information
using SharpBitStream;

uint[] testDataUnsigned = { 5, 62, 17, 50, 33 };
var ms = new MemoryStream();
var bs = new BitStream(ms);
Console.WriteLine("Test1: \r\nFirst testing writing and reading small numbers of a max of 6 bits.");

Console.WriteLine("There are 5 unsigned ints , which shall be written into 6 bits each as they are all small than 64: 5, 62, 17, 50, 33");
foreach(var bits in testDataUnsigned)
{
    bs.WriteUnsigned(6, (ulong)bits);
}

Console.WriteLine("The original data are of the size: " + testDataUnsigned.Length + " bytes. The size of the stream is now: " + ms.Length + " bytes\r\nand the bytes in it are: ");

ms.Position = 0;

Console.WriteLine("The resulting bytes in the stream look like this: ");
for (int i = 0; i < ms.Length; i++)
{
    uint bits = (uint)ms.ReadByte();
    Console.WriteLine("Byte #" + Convert.ToString(i).PadLeft(4, '0') + ": " + Convert.ToString(bits, 2).PadLeft(8, '0'));
}

Console.WriteLine("\r\nNow reading the bits back:");
ms.Position = 0;
bs.SetPosition(0, 0);

foreach (var bits in testDataUnsigned)
{
    ulong number = (uint)bs.ReadUnsigned(6);
    Console.WriteLine("Number read: " + number);
}

Console.WriteLine("\r\nTest2:\r\nNow a large number spanning multiple bytes in a stream - over 30 bits: 715827882"
    +"\r\nwhich looks in binary like this: 101010101010101010101010101010");

var ms2 = new MemoryStream();
var bs2 = new BitStream(ms2);
bs2.WriteUnsigned(30, (ulong)715827882);
Console.WriteLine("The bytes within the memory stream are: ");
ms2.Position = 0;
for(int i = 0; i < ms2.Length; i++)
{
    uint bits = (uint)ms2.ReadByte();
    Console.WriteLine("Byte #" + Convert.ToString(i).PadLeft(4, '0') + ": " + Convert.ToString(bits, 2).PadLeft(8, '0'));
}

Console.WriteLine("\r\nNow reading the bits back:");
ms2.Position = 0;
bs2.SetPosition(0, 0);
ulong number2 = bs2.ReadUnsigned(30);
Console.WriteLine("The number read is: " + number2);
    