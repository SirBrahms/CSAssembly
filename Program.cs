using System;
using CSAssembly;

// File used for Testing

//Console.WriteLine("Hello, World!");

AssemblyHandler.Run(@"MOV %eax $12 
                    MOV %ebq $100");

Console.WriteLine(RegisterHandler.Registers["EAX"]);
Console.WriteLine(RegisterHandler.Registers["EBX"]);
