
# BitStream - A library to write and read bits to/from streams
## Introduction
When you are creating a storage of binary data, maybe creating your own file format or some way of compression, you will see that you are restricted by the 8-bite = 1 byte representation, when it comes to saving space. E.g., it does not matter, whether you are storing the number 5 or number 255 - both need 8 bit. 
With Bitstream, you can write any unsigned or signed value to a number of bits (set by you) and the bits of consecutive numbers are merged in the bytes of the underlying byte stream.

## Quick Start
Here are 2 examples of how to first write numbers to a BitStream and then how to read them:


1. Example of 5 numbers, which could be represented by 6 bits (0-63 unsigned), using Bitstream:
![Example 1](https://github.com/martinweihrauch/Bitstream/raw/main/BitStream/TestApplication/images/slide1.jpg?raw=true)
2. Example of 2 numbers, one with a 4 bit representation and one with 28 bits, both unsigned:
![Example 2](https://github.com/martinweihrauch/Bitstream/raw/main/BitStream/TestApplication/images/slide2.jpg?raw=true)


The code for realizing this: 

```csharp
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
```
	
## Step by step explanation: Writing
### Just the quick stuff

1. Have a Stream available, e. g. a MemoryStream(), to which you want to write. 
2. Connect this Stream to a new Bitstream

```csharp
using SharpBitStream;

uint[] testDataUnsigned = { 5, 62, 17, 50, 33 };
var ms = new MemoryStream();
var bs = new BitStream(ms);
```

3. Now, you can start writing to the BitStream like this:

```csharp
foreach(var bits in testDataUnsigned)
{
    bs.WriteUnsigned(6, (ulong)bits);
}
```
That's it!

### I need to know more about writing...
The bits will be concatenated sequentially irrespective of the byte "boundaries". This is, why you can save space using BitStream.
Note that you - of course - always have to provide the desired bitlength as a parameter when writing and reading (in this example "6").
Values can only be provided as either long or ulong. This is for ease of use. If you have e. g. uint, just convert them with:
```csharp	
uint num = 5;
//Do stuff with num...
ulong numLong = (ulong)num;
```
Writing can be done as above by only providing the bitlength and the value, but you of course also have full controll of exactly where to write the bits like so:
```csharp	
bs.WriteUnsigned(3, 2, 4, 5);
// Overloaded signature of WriteUnsigned:
// public void WriteUnsigned(long offsetByteStream, int offsetBit, int bitLength, ulong value)
// For signed numbers (e. g. -17), use
// bs.WriteSigned(3, 2, 4, -5);
```
This means, you can control that you write to the 4th byte (3, because starting at 0) in the underlying byte Stream, 
starting from the the 3rd (=2) position of the byte with a length of 6 bits and the value 5 (=0b0101);
If you have written a couple of bits to the stream and want that the next read/write will be starting at the next "fresh byte" and you are still at some 
arbitrary bit position, you can "flush":
```csharp	
long bytePos = bs.FlushBitPosition();
```
The returned long value tells you the location of the byte within the stream.
Note: When working with streams, always remember to "rewind" a stream to 0, when e. g. you are done with writing and want to read - this can be done with 
e. g. Stream.Position = 0; Also, you can use the method of BitStream:
```csharp	
bs.SetPosition(0, 0);
```

## Reading
Reading from BitStream is straightforward and simple. Just read any bits from the BitStream, either signed or unsigned into a (u)long variable.
There are also - exactly like for writing - 2 methods each for signed and unsigned: one is to read the next bits (with given bitlength) and the other
is with full control over byte position and bit position:
1. Just read the next 6 bits, wherever your byte and bit position is (e. g. for loops, etc):
```csharp	
ulong number = bs.ReadUnsigned(6);
// For Signed, use 
// long number = bs.ReadSigned(6);
```
2. Read a specific position, in this example read 4 bits from 3rd byte in Stream (2= 3rd position), starting with bit #0:
```csharp	
ulong number = bs.ReadUnsigned(2, 0, 4);
// For signed, use
// long number = bs.ReadSigned(2, 0, 4);
```

## Getting / setting postions:
You can set the current position, just by passing either the numbers for byte position and bit position or passing a Position object.
```csharp	
bs2.SetPosition(0, 0);
// OR: 
var pos = new Position(0, 0)
bs2.SetPosition(pos);
// READ the position:
Position temp = bs2.GetPosition();
```
	
## How do I know that reading or writing is "allowed"/valid?
Before you **read**, you can check with:
```csharp	
if(bs.IsReadValid(6))
{
	// Check, if allowed to read the next 6 bits...
}
// OR: 
if(bs.IsReadValid(3, 6, 6))
{
	// Check, if allowed to read the 6 bits from 4th byte, starting at bit position 6 for 6 bits.
}

```
Before you **write**, you can check, whether the value you want to pass will fit into your bitlength with this:
```csharp	
public bool IsWriteValid(int bitLength, long value)
```

## About signed and unsigned - why is there a difference?
Unsigned and signed numbers are in the end all stored in bytes. In negative numbers, you basically use up one bit for the sign, so for "-" or "+".
So, for you it is important to know, what the number ranges are you can store with BitStream while writing. You can always check it with
using the method "IsWriteValid(...)" as shown above.
Internally, BitStream uses Two's complement [Wikipedia](https://en.wikipedia.org/wiki/Two%27s_complement).
**Unsigned** number ranges:
| Number of bits | Range of values |
| ----------- | ----------- |
| 2 | 0-3 |
| 3 | 0-7 | 
| 4 | 0-15 | 
| 5 | 0-31 |
| 6 | 0-63 |
| 7 | 0-127 |
...etc

**Signed** number ranges:
| Number of bits | Range of values |
| ----------- | ----------- |
| 2 | -2, -1, 0, 1 |
| 3 | -4 - +3 | 
| 4 | -8 - +7 | 
| 5 | -16 - +15 |
| 6 | -32 - +31 |
| 7 | -64 - +63 |

Please note that in signed numbers with "Two's complement", there is always one more number in the negative side than in the positive side of the range.
	
