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

    #endregion
}