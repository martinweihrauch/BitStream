/*
 * BitStream was written and is copyrighted by Martin Weihrauch (c) 2022
 * It is open source and can be used under MIT license * 
 * 
 */

using SharpBitStream;


uint[] testDataUnsigned = { 5, 62, 17, 50, 33 };
var ms = new MemoryStream();
var bs = new BitStream(ms);
Console.WriteLine("Test1: \r\nFirst testing writing and reading small numbers of a max of 6 bits.");

Console.WriteLine("There are 5 unsigned ints , which shall be written into 6 bits each as they are all small than 64: 5, 62, 17, 50, 33");
foreach(var bits in testDataUnsigned)
{
    //Can I really write this (optional):
    if(!bs.IsWriteValid(6, (long)bits))
    {
        Console.WriteLine("Something went wrong with writing!");
        break;
    }
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
    if (!bs.IsReadValid(6))
    {
        Console.WriteLine("Ooops, there is an error - cannot read!");
        break;
    }
    ulong number = bs.ReadUnsigned(6);
    Console.WriteLine("Number read: " + number);
}

Console.WriteLine("-------------------------------------------------------");

Console.WriteLine("\r\nTest2:\r\nNow a small number (14) over 4 bits and a large number with 28 bits: 178,956,970"
    + "\r\nwhich looks in binary like this: 1110 and 1010101010101010101010101010");

var ms2 = new MemoryStream();
var bs2 = new BitStream(ms2);
bs2.WriteUnsigned(4, 14);
bs2.WriteUnsigned(28, 178956970);
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
Position temp = bs2.GetPosition();
ulong number14 = bs2.ReadUnsigned(4);
ulong numberLong = bs2.ReadUnsigned(28);
Console.WriteLine("The number read are: " + number14 + " and: " + numberLong);



Console.WriteLine("-------------------------------------------------------");
Console.WriteLine("There are 5 SIGNED ints , which shall be written into 7 bits each as they are all small than -64 to 63: 5, -62, 17, -50, -33, 0");


int[] testDataSigned = { 5, -62, 17, -50, -33, 0 };
var msSigned = new MemoryStream();
var bsSigned = new BitStream(msSigned);
Console.WriteLine("Test1: \r\nFirst testing writing and reading signed numbers of a max of 7 bits.");

foreach (var bits in testDataSigned)
{
    //Can I really write this (optional):
    if (!bsSigned.IsWriteValid(7, (long)bits))
    {
        Console.WriteLine("Something went wrong with writing!");
        break;
    }
    bsSigned.WriteSigned(7, (long)bits);
}


Console.WriteLine("\r\nNow reading the bits back:");
msSigned.Position = 0;
bsSigned.SetPosition(0, 0);

foreach (var bits in testDataSigned)
{
    if (!bsSigned.IsReadValid(7))
    {
        Console.WriteLine("Ooops, there is an error - cannot read!");
        break;
    }
    long number = bsSigned.ReadSigned(7);
    Console.WriteLine("Number read: " + number);
}

Console.WriteLine("-------------------------------------------------------");
Console.WriteLine("There are 5 SIGNED ints , which shall be written into 11 bits each as they are all small than -1024 to 1023: -1023, -19, 1019, -50");


int[] testDataSignedLong = { -1023, -19, 1019, -50 };
var msSignedLong = new MemoryStream();
var bsSignedLong = new BitStream(msSignedLong);
Console.WriteLine("Test1: \r\nWriting and reading signed numbers of a max of 11 bits.");

foreach (var bits in testDataSignedLong)
{
    //Can I really write this (optional):
    if (!bsSignedLong.IsWriteValid(11, (long)bits))
    {
        Console.WriteLine("Something went wrong with writing!");
        break;
    }
    bsSignedLong.WriteSigned(11, (long)bits);
}

msSignedLong.Position = 0;

Console.WriteLine("The resulting bytes in the stream look like this: ");
for (int i = 0; i < msSignedLong.Length; i++)
{
    uint bits = (uint)msSignedLong.ReadByte();
    Console.WriteLine("Byte #" + Convert.ToString(i).PadLeft(4, '0') + ": " + Convert.ToString(bits, 2).PadLeft(8, '0'));
}



Console.WriteLine("\r\nNow reading the bits back:");
msSignedLong.Position = 0;
bsSignedLong.SetPosition(0, 0);

foreach (var bits in testDataSignedLong)
{
    if (!bsSignedLong.IsReadValid(11))
    {
        Console.WriteLine("Ooops, there is an error - cannot read!");
        break;
    }
    long number = bsSignedLong.ReadSigned(11);
    Console.WriteLine("Number read: " + number);
}
