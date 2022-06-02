using System;
using CSAssembly;

// File used for Testing

//Console.WriteLine("Hello, World!");

// Defining the Interrupt Handler:
int InterruptHandler(int IntCode) {
    Console.WriteLine($"Called INT! -> {IntCode}");
    return 0; // Must return 0 for indicating success
}

AssemblyHandler.InterruptHandler = InterruptHandler;
AssemblyHandler.Run(@"MOV %eax $55 INT %eax");



Console.WriteLine("-------------------------------");
Console.WriteLine($"EAX: {RegisterHandler.Registers["EAX"]}");
Console.WriteLine($"EBX: {RegisterHandler.Registers["EBX"]}");
