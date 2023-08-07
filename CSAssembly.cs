using System;
using System.Collections.Generic;
using CSAssembly.Types;

namespace CSAssembly
{
    #region Handlers

    // Main Class
    // Handles all the Assembly parsing and executing logic
    // Main API-Entry Point
    static class AssemblyHandler
    {
        // The "RAM" of the Program
        public static IMemory RAM = new DynamicRAM();

        // The Delegate that will do the Interrupt handling
        // int -> Parameter Type
        // int -> Return type
        public static Func<int, int>? InterruptHandler;

        // The Array of Strings that represent the Assembly-Program
        public static string[]? AssemblySplit;
        // Variable for Iterating trough the array "AssemblySplit"
        public static int i = 0;

        // Main Program Entry Point
        public static int Run(string Assembly) {
            // Preprocessing the String to remove all the junk
            Assembly = Assembly.Trim();
            Assembly = Assembly.Replace(",", "");
            Assembly = Assembly.ToUpper(); // Making the Entire String Uppercase

            AssemblySplit = Assembly.Split(" "); // Splitting the input string by all Spaces

            // Executing the Assembly
            int line = 1; // Variable for Holding the Current Line Number
            while (i < AssemblySplit.Length) {
                if (AssemblySplit[i] == "" || AssemblySplit[i] == " ") {
                    i++;
                    continue; // Ignore the spaces
                }
                if (AssemblySplit[i].Contains("\n")) {
                    line++; // Increase the Line count if a newline character is encounterd
                    i++; // Increse the toatal count
                    continue; // Ignore this instruction
                }

                if(Instruction.LookupInstruction(AssemblySplit[i]) != 1) i++; // Incrementing trough the Array if the Instruction is correct
                else {
                    // Throw Error
                    Console.WriteLine($"Error at index {i} -> line {line}, unexpected Token: \"{AssemblySplit[i]}\"");
                    return 1; // 1 = Failure
                }
            }
            
            return 0; // 0 = Success!
        }

        private static int ConvertToBytecode(string[] Convert) {
            

            return 0;
        }
        
    }

    // Implementation of a class for Executing all the Assembly-Instructions
    // Consists of functions that behave according to the instructions
    // Cannot be instantiated (as it is static too)
    static class InstructionHandler
    {
        // Private Helper Values:
        // -------------------------------------
        private static int ConsumeIndex = AssemblyHandler.i;

        // Public Instruction Handling Definitions
        // -------------------------------------

        // Restrictions for Instruction-Definitions:
        // - Must return an int (0 = Success, 1 = Failure)
        // - Must be static
        // - Must be public
        // Naming:
        // - Instruction, followed by the actual name of the Assembly Instruction

        // ADD Instruction
        // Adds two operands
        public static int InstructionADD() {
            int OperandA = ResolveValue(ConsumeNext());
            int OperandB = ResolveValue(ConsumeNext());

            return 0;
        }

        // MOV Instruction
        // Moves Value into Register Destination
        public static int InstructionMOV() {
            string Destination = ConsumeNext(); // Get the Register
            string Value = ConsumeNext(); // Get the thing to put into the Register

            if (Destination.StartsWith('%')) { // If the Destination is a register
                Destination = Destination.Remove(0, 1); // Remove the %
                if (RegisterHandler.IsRegister(Destination)) // Check if the destination is really a register
                    RegisterHandler.Registers[Destination] = ResolveValue(Value); // Set the Register with the Value specified
                else return 1;
            }
            else return 1; // If the destination isn't a register we return Failure
            return 0; // If everything went well we report success
        }

        // INT Instruction
        // Calls the "Kernel" (in this case the predefined Function)
        public static int InstructionINT() {
            if (AssemblyHandler.InterruptHandler != null) { // If the Interrupt Handler has been initialized
                AssemblyHandler.InterruptHandler(ResolveValue(ConsumeNext())); // Executes the interrupt function with the next Element in the List as an Argument
            }
            else throw new InterruptHandlerException("Interrupt Handler has not been defined"); // Throw an exception about the Delegate being null
            return 0;
        }



        // Private Helper Functions:
        // -------------------------------------

        // Function to update all arithmetic Flags according to the parameter
        private static void UpdateArithmeticFlags(int Value) {
            // Check and Update Sign
            Flags.Sign = !(Value <= 0);
            // Check and Update Zero Flag
            Flags.Zero = Value == 0;
            // Check and Update Parity Flag
            bool ParityFlag = false;

            while (Value != 0) {
                Value &= (Value - 1);
                ParityFlag = !(ParityFlag);
            }
            Flags.Parity = ParityFlag;
            return;
        }

        // Function to get the next element in the AssemblySplit Array and replace it with a NOP
        private static string ConsumeNext() {
            if (AssemblyHandler.AssemblySplit != null && AssemblyHandler.AssemblySplit.Length >= 2) { // If "AssemblyHandler.AssemblySplit" isn't null
                try
                {
                    ConsumeIndex = AssemblyHandler.i; // Set the Consume index according to the real index
                    while (AssemblyHandler.AssemblySplit[ConsumeIndex + 1] == "NOP") { // If The next element is "NOP"
                        ConsumeIndex++; // Increase the index at which the Result will be grabbed
                    }

                    string Result = AssemblyHandler.AssemblySplit[ConsumeIndex + 1]; // Get the next NonNOP Element
                    AssemblyHandler.AssemblySplit[ConsumeIndex + 1] = "NOP"; // Set the grabbed value to a NOP
                    ConsumeIndex = AssemblyHandler.i; // Reset the Consume index
                    return Result; // Return the Result
                }
                catch (IndexOutOfRangeException)
                {
                    throw new ConsumeException("Error whilst getting the next element in the input assembly: Too few elements");
                }
            }

            throw new ConsumeException("Error whilst getting the next element in the input assembly: Might be null"); // Throw an exception about "AssemblyHandler.AssemblySplit" being null
        }

        // Function that resolves a value into its correct form
        // (String prefixed with '$' -> int etc...)
        private static dynamic ResolveValue(string Value) {
            if (Value.StartsWith('$')) { // If it starts with a '$' it's an integer
                try
                {
                    Value = Value.Remove(0, 1); // Remove the '$' sign
                    int Result = Int.ParseInt(Value).ToNormalInt(); // Parse the int
                    return Result; // return the parsed int
                }
                catch (FormatException)
                {
                    throw new NumberException("Value Prefixed by '$' was not a proper Int32"); // If the integer conversion Fails, throw this Exception
                }
            }
            else if (Value.StartsWith('%')) { // If it starts with a '%' it's a register
                Value = Value.Remove(0, 1); // Remove the '%' sign
                return RegisterHandler.Registers[Value]; // Return whatever there is inside the Register
            }
            else {
                throw new Exception("Resolve Value, this will be replaced"); // If the value cannot be resolved, throw an error
            }
        }
    }

    // Implementation of a Handler Class for Registers
    // Contains all the Registers and functions to check the Existance of Registers
    // (Is static)
    static class RegisterHandler
    {
        // Dictionary to hold Registers mnemonic Names and their bytecode Values (Register Translation Table RTT for Bytecode Converter)
        // string -> Mnemonic Name of the Register
        // byte -> Byte opcode of the Register
        public static readonly Dictionary<string, byte> RegisterTranslationTable = new Dictionary<string, byte>()
        {
            {"EAX", 0xFF}, // Accumulator
            {"EBX", 0xFE}, // Base
            {"ECX", 0xFD}, // Counter
            {"EDX", 0xFC}, // Data
            {"EIP", 0xFB}, // Instruction Pointer
            {"ESP", 0xFA}, // Stack Pointer
            {"EBP", 0xF9}, // Base Pointer (For Returning)
            {"ESI", 0xF8}, // Source Index for String Operations
            {"EDI", 0xF7}  // Destination Index for String Operations
        };

        // Defining all the Registers and assigning their names
        // string -> Name of the Register
        // Int32 -> Contents of the Register
        public static Dictionary<string, Int32> Registers = new Dictionary<string, Int32>
        {
            {"EAX", 0}, // Accumulator
            {"EBX", 0}, // Base
            {"ECX", 0}, // Counter
            {"EDX", 0}, // Data
            {"EIP", 0}, // Instruction Pointer
            {"ESP", 0}, // Stack Pointer
            {"EBP", 0}, // Base Pointer (For Returning)
            {"ESI", 0}, // Source Index for String Operations
            {"EDI", 0}  // Destination Index for String Operations
        };

        // Function to check if the mentioned Register Exists
        public static bool IsRegister(string Reg) {
            return Registers.ContainsKey(Reg);
        }
    }

    #endregion

    #region Static Types
    // Implementation of a Flag Class
    // It contains all the Flags that are used by the different instructions
    // Cannot be instantiated (as it is static)
    static class Flags
    {
        /* 
        * It contains the carry of 0 or 1 from a high-order bit (leftmost)
        * after an arithmetic operation. It also stores the contents of last bit of a shift or rotate operation. \/
        */
        public static int Carry = 0b0;

        /*
        * It contains the carry from bit 3 to bit 4 following an arithmetic operation; used for specialized arithmetic.
        * The AF is set when a 1-byte arithmetic operation causes a carry from bit 3 into bit 4. \/
        */
        public static int AuxiliaryCarry = 0b0;

        /*
        * It indicates the overflow of a high-order bit (leftmost bit) of data after a signed arithmetic operation. \/
        */
        public static bool Overflow = false;

        /*
        * It determines left or right direction for moving or comparing string data. 
        * When the DF value is false, the string operation takes left-to-right direction
        * and when the value is set to true, the string operation takes right-to-left direction. \/
        */
        public static bool Direction = false; 

        /*
        * It determines whether the external interrupts like keyboard entry, etc., are to be ignored or processed. 
        * It disables the external interrupt when the value is false and enables interrupts when set to true. \/
        */
        public static bool Interrupts = false;

        /*
        * Sets the Processor into "Single Step Mode" if set to true. 
        * Can be used in debugging Programs, to step trough the instructions one at a time \/
        */
        public static bool Trap = false;

        /*
        * Sets the arithmetical Sign of an operation.
        * If set to true: Negative Result (-)
        * If set to false: Postitive Result (+) \/
        */
        public static bool Sign = false;

        /*
        * It indicates the result of a mathematical operation or comparison.
        * If that result turns out to be zero, this flag will be set to true \/
        */
        public static bool Zero = false;

        /*
        * It indicates the total number of 1-bits in the result of a mathematical operation.
        * An even number of 1-bits sets this Flag to false (i.E: 0b11110000)
        * An odd number of 1-bits sets this Flag to true (i.E: 0b10000000)
        */
        public static bool Parity = false;
    }

    // Implementation of a class for Holding all the Assembly-Instructions
    // Consists of a big dictionary with functions being mapped to strings and a Lookup function
    // Cannot be instantiated (as it is static as well)
    static class Instruction
    {
        // A Constant for Replacing the NOP-Instruction where no operation is performed (For better Readability)
        private const int NOP = 0;

        // Dictionary for Holding all the Readable Assembly mnemonics. (Alias Translation Table ATT for Bytecode Converter)
        // string -> Mnemonic Name
        // byte -> Byte opcode
        public static readonly Dictionary<string, byte> AliasTranslationTable = new Dictionary<string, byte>
        {
            {"ADD-RR", 0x1}, // ADD reg, reg
            {"ADD-CR", 0x5}, // ADD reg, const
            {"MOV-RR", 0x89}, // MOV src (reg), dest (reg)
            {"NOP", 0x90}, // NOP
            {"MOV-CR", 0xB8}, // MOV src (const), dest (reg)
            {"INT", 0xCC}, // INT const

        };

        // Dictionary for holding the Assembly Instructions Names and corresponding functions
        // string -> Name of the Instruction
        // Func<int> -> Function returning an integer that corresponds to the Instruction
        private static Dictionary<string, Func<int>> Instrs = new Dictionary<string, Func<int>> 
        {
            {"ADD", () => NOP},
            {"INT", () => InstructionHandler.InstructionINT()},
            {"NOP", () => NOP},
            {"MOV", () => InstructionHandler.InstructionMOV()}
        };

        // Function to Look up an Instruction and execute it, if the instruction exists
        public static int LookupInstruction(string Instruction) {
            try 
            {
                int ret = Instrs[Instruction](); // Invoke the Function referenced by the Instructions Name
                if (ret == 1) return 1; // 1 = Failure (Whilst performing the operation)
            }
            catch (KeyNotFoundException) 
            {
                // If the Key doesn't exist, return a 1
                return 1; // 1 = Failure
            }
            return 0; // 0 = Success
        }
    }

    
    
    #endregion

    #region Exceptions

    // Implementing an exception that is thrown if the ConsumeNext function Fails
    class ConsumeException : Exception
    {
        public ConsumeException() {}
        public ConsumeException(string Message) : base(Message) {}
        public ConsumeException(string Message, Exception Inner) : base(Message, Inner) {}
    }

    // Implementing an exception for when a register doesn't exist
    class RegisterException : Exception
    {
        public RegisterException() {}
        public RegisterException(string Message) : base(Message) {}
        public RegisterException(string Message, Exception Inner) : base(Message, Inner) {}
    }

    // Implementing an exception for when a register doesn't exist
    class NumberException : Exception
    {
        public NumberException() {}
        public NumberException(string Message) : base(Message) {}
        public NumberException(string Message, Exception Inner) : base(Message, Inner) {}
    }

    // Implementing an exception for when the InterruptHandler hasn't been specified
    class InterruptHandlerException : Exception
    {
        public InterruptHandlerException() {}
        public InterruptHandlerException(string Message) : base(Message) {}
        public InterruptHandlerException(string Message, Exception Inner) : base(Message, Inner) {}
    }

    #endregion
}