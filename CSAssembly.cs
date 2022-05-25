namespace CSAssembly
{
    // Main Class
    // Handles all the Assembly parsing and executing logic
    class AssemblyHandler
    {
        // Defining all the Registers and assigning their names
        public static Register RegEAX = new Register("EAX"); // Accumulator
        public static Register RegEBX = new Register("EBX"); // Base
        public static Register RegECX = new Register("ECX"); // Counter
        public static Register RegEDX = new Register("EDX"); // Data
        public static Register RegEPI = new Register("EIP"); // Instruction Pointer
        public static Register RegESP = new Register("ESP"); // Stack Pointer
        public static Register RegEBP = new Register("EBP"); // Base Pointer (For Returning)
        public static Register RegESI = new Register("ESI"); // Source Index for String Operations
        public static Register RegEDI = new Register("EDI"); // Destination Index for String Operations
        
    }

    #region Custom Types

    // Implementation of a Register Class
    // It has a string as a Name
    // And it has a dynamic Value for the Register's contents
    class Register 
    {
        public readonly string Name = "";
        public dynamic Contents = 0;

        public Register(string Name, dynamic? Contents = default(dynamic)) {
            this.Name = Name;
            if (Contents != default(dynamic)) this.Contents = Contents; // If Contents isn't empty, it will be assigned the Value in the Register
            else this.Contents = -1; // Else it will be assigned a -1
        }
    }

    // Implementation of a Flag Class
    // It contains all the Flags that are used in Assembly
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

    #endregion
}