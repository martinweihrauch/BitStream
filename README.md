
# Bitstream - A library to write and read a bit stream
When you are creating a storage of binary data, maybe creating your own file format or some way of compression, you will see that you are restricted by the 8-bite = 1 byte representation, when it comes to saving space. E.g., it does not matter, whether you are storing the number 5 or number 255 - both need 8 bit. 
With Bitstream, you can write any unsigned or signed number to a number of bits and the bits of consecutive numbers are merged in the bytes of the underlying byte stream.
Here are 2 examples of what this means:

1. Example of 5 numbers, which could be represented by 6 bits (0-63 unsigned), using Bitstream:
![Example 1](https://github.com/martinweihrauch/Bitstream/raw/main/BitStream/TestApplication/images/slide1.jpg?raw=true)
2. Example of 2 numbers, one with a 4 bit representation and one with 28 bits, both unsigned:
![Example 2](https://github.com/martinweihrauch/Bitstream/raw/main/BitStream/TestApplication/images/slide2.jpg?raw=true)
And this is how easy it is to use:
- Create a regular stream (memory or file stream) and then 
- create a bitstream based on that stream.
- You can either write consecutive bits 

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
	


