namespace CSAssembly.Types
{
    /* This Namespace is for non-static Types */

    // Implementation of an Integer Struct to provide better conversion implementation
    public struct Int 
    {
        public int Value;

        // Constructor
        public Int(int Value) {
            this.Value = Value;
            return;
        }

        // Value assign Function
        public static implicit operator Int(int v) => new Int(v);

        // Overload of the Plus operator to add an Int and an int (builtin)
        public static Int operator +(Int a, int b) => new Int(a.Value + b);

        // Overload of the Plus operator to add two Ints
        public static Int operator +(Int a, Int b) => new Int(a.Value + b.Value);

        // Overload of the Minus operator to subtract an Int and an int (builtin)
        public static Int operator -(Int a, int b) => new Int(a.Value - b);

        // Overload of the Minus operator to subtract two Ints
        public static Int operator -(Int a, Int b) => new Int(a.Value - b.Value);

        // Overload of the Times operator to multiply an Int and an int (builtin)
        public static Int operator *(Int a, int b) => new Int(a.Value * b);

        // Overload of the Times operator to multiply two Ints
        public static Int operator *(Int a, Int b) => new Int(a.Value * b.Value);

        // Overload of the divide operator to divide an Int and an int (builtin)
        public static Int operator /(Int a, int b) => new Int(a.Value / b);

        // Overload of the divide operator to divide two Ints
        public static Int operator /(Int a, Int b) => new Int(a.Value / b.Value);

        // Overload of the ++ operator to increment
        public static Int operator ++(Int a) => a.Value++;

        // Overload of the == operator
        public static bool operator ==(Int lhs, int rhs) => lhs.Value == rhs;
        public static bool operator ==(Int lhs, Int rhs) => lhs.Value == rhs.Value;

        // Overload of the != operator
        public static bool operator !=(Int lhs, int rhs) => lhs.Value != rhs;
        public static bool operator !=(Int lhs, Int rhs) => lhs.Value != rhs.Value;
        
        // Returns a string Representation of the value
        public override string ToString() => Value.ToString();

        // Override of Object.Equals()
        public override bool Equals(object? obj)
        {
            if (obj == null) {
                return false;
            }
            return this == (Int) obj;
        }

        // Override of Object.GetHashCode()
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // Implement a better ParseInt Function that automatically converts from the right base
        public static Int ParseInt(string Value) {
            try
            {
                if (Value.StartsWith("0x")) {
                    // Hex Value Conversion / Parsing
                    // Remove 0x-part
                    Value = Value.Remove(0, 2);
                    return new Int(Convert.ToInt32(Value, 16)); // Parse and Return int
                }
                else if (Value.StartsWith("0b")) {
                    // Binary Value Conversion / Parsing
                    // Remove 0b-part
                    Value = Value.Remove(0, 2);
                    return new Int(Convert.ToInt32(Value, 2)); // Parse and Return int
                }
                else {
                    // Base 10-Number Parsing
                    return new Int(Convert.ToInt32(Value));
                }
            }
            catch
            {
                throw new FormatException("Value Specified was not covertable to an Int representation");
            }
        }

        // Function to get a "normal" int back
        public int ToNormalInt() {
            return this.Value;
        }
    }
    
    // Implementation of a RAM-Like Datastructure -> only suitable for bytes
    /* Unfinished! - Do not use! */
    class RandomAccessMemory
    {
        // Array for the simulation of 4kB RAM
        public byte[] RAM = new byte[4000];
        // Integer for holding the current highest free position
        public int ByteIndex {get; private set;}

        // Constructor for Initializing the byte Index
        public RandomAccessMemory()
        {
            ByteIndex = 0; // Initializing the Byte index
            return;
        }

        // Function to write one Specific Byte into the next available Position
        public void WriteByte(byte ByteToWrite) {
            // Check if memory isn't full
            if (ByteIndex < 4000) {
                RAM[ByteIndex] = ByteToWrite; // Write the Byte into RAM
                ByteIndex++; // Increase the ByteIndex
            }
            else throw new Exception("RAM exception, this will be Replaced");
        }

        // Function to read one byte from the specified position
        public byte ReadByte(int Address) {
            if (Address <= 4000) return RAM[Address]; // If the specified Position isn't out of bounds, return the Value
            else throw new Exception("RAM exception, this will be Replaced"); // Else throw an error
        }

        // Function to write multiple bytes into the next available positions
        public void WriteBytes(byte[] BytesToWrite) {
            // Check if there is enough space in memory
            if (ByteIndex + BytesToWrite.Length < 4000) {
                int OldByteIndex = ByteIndex; // Backup of the Byte Index before the writing Process

                // While not every byte was written
                for (int i = 0; i < OldByteIndex + BytesToWrite.Length; i++) {
                    RAM[ByteIndex] = BytesToWrite[i]; // Append the corresponding byte to Memory
                    ByteIndex++; // Increase the ByteIndex
                }

            }
            else throw new Exception("RAM exception, this will be Replaced");
        }

        // Function to read multiple bytes
        public byte[] ReadBytes(int From, int To) {
            byte[] Result = new byte[To - From]; // The Array that will be returned
            int Index = 0; // Int for iterating trough the RAM

            for (int i = From; i <= To; i++) {
                Result[Index] = ReadByte(i); // Getting whatever there is at that position
            }

            return Result;
        }

        public void FreeTopByte() {
            RAM[ByteIndex] = 0; // Zero out the byte
            ByteIndex--; // Decrease the byte Index
        }
    }

    

    // Implementation of a Register Class
    // It has a string as a Name
    // And it has a dynamic Value for the Register's contents

    /* Deprecated! */
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
}