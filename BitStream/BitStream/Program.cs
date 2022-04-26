// See https://aka.ms/new-console-template for more information
using SharpBitStream;

Console.WriteLine("Hello, World!");

int[] testData = { 5, 127, -128, 50, -50 };
var ms = new MemoryStream();
var test = new BitStream(ms);
int test2 = 277388;

