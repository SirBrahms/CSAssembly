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

    // Implementation of a Dynamic RAM-Datastructure (still bytes only)
    class DynamicRAM : IDynamicRAM
    {
        // List that Represents the RAM (add Private set in implementation)
        private List<byte?> RAM = new List<byte?>();
        // Int Dictionary that represents all "Holes" in memory (NULLs) and their sizes
        // First int -> Address of the Hole
        // Second int -> Size of the Hole
        private Dictionary<int, int> Holes = new Dictionary<int, int>();

        // Function to write one Byte to the next free Position in memory
        public bool WriteByte(byte ByteToWrite) {
            try
            {
                int Address = GetLowestSizeableHole(1); // Get the hole to put the Byte into
                if (Address == -1) { // If no Hole was found
                    RAM.Add(ByteToWrite); // Add the byte to the end
                    return true; // Return success
                }   
                else { // otherwise
                    RAM[Address] = ByteToWrite; // Replace the null byte with the byte to write
                    return true; // Return success
                }
            }
            catch 
            {
                return false;
            }
        }

        // Function to Write multiple bytes to the next Location that is big enough to fit all the Bytes chained together
        public bool WriteBytes(byte[] BytesToWrite) {
            try
            {
                int Size = BytesToWrite.Count();
                int StartingAddress = GetLowestSizeableHole(Size);

                if (StartingAddress == -1) { // If there is no sizeable Hole
                    for (int i = 0; i < Size; i++) {
                        RAM.Add(BytesToWrite[i]); // Add the Bytes to the Top of the List
                    }
                    return true;
                }
                else {
                    int j = 0;
                    for (int Address = StartingAddress; Address < Size + StartingAddress; Address++) {
                        RAM[Address] = BytesToWrite[j]; // Write the bytes into Memory
                        j++; // Increase the Indexer for BytesToWrite
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        
        // Function to read one byte from a specified Address
        public byte? ReadByte(int Address) {
            if (RAM[Address] != null) { // If the Value is not Null, return it
                return RAM[Address];
            }
            else {
                throw new Exception("RAM exception, segfault, this will be replaced!"); // Else throw an exception
            }   
        }

        

        // Function to Read a range of bytes From Memory
        public byte?[] ReadBytes(int From, int To) {
            byte?[] Result = new byte?[(To - From) + 1];

            try
            {
                int i = 0; // Indexer for the Result array
                for (int Index = From; Index < (To - From) + 1; Index++) {
                    if (RAM[Index] != null)
                        Result[i] = RAM[Index]; // Add the Read value to the Result array
                    else 
                        Result[i] = 0x0; // If a null is encountered, substitute it with a 0x0 byte
                    i++; // Increase the Indexer
                }

                return Result;
            }
            catch
            {
                throw new Exception("RAM Exception, this will be Replaced!");
            }
        }

        // Function to free Resources
        public bool Free(int From, int To) {
            try
            {
                // Loop the amount of times that are Required
                for (int i = 0; i <= To - From; i++) {
                    RAM[To - i] = null; // Set the Corresponding bytes to null in Reverse order
                }

                Holes.Add(From, (To - From) + 1); // Add the Address and size of the Hole to the Hole dictionary

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Function to Free one Byte
        public bool Free(int Address) {
            try 
            {
                RAM[Address] = null; // Null-ify the byte (indicating that it's free)
                Holes.Add(Address, 1); // Add the Location and size of the Hole to the dictionary 

                return true; // Return success
            }
            catch
            {
                return false; // Return failure
            }
        }

        // Function to get the Lowest Fitting Hole in the Hole List 
        private int GetLowestSizeableHole(int Size) {
            try
            {
                // Find the Lowest Value in the Dictionary
                KeyValuePair<int, int> Result = Holes.Where(x => x.Value == Holes.Min(x2 => x2.Value) 
                                                            && x.Value >= Size).First();

                if (Result.Value > Size) { // If the Result.Value is bigger than the Requested size
                    // Make a new Key-Value Pair with adjusted values, since the hole wasn't fully filled
                    Holes.Remove(Result.Key); // Remove the Original value

                    Holes.Add((Result.Key + Result.Value) - 1, Result.Value - Size); // Add a new Entry with the Corrected Address and Size
                }
                else
                    Holes.Remove(Result.Key); // Remove the Hole From the Dictionary
                
                return Result.Key; // Return the Address of the Hole
            }
            catch
            {
                return -1; // If no Hole was found return -1 since -1 will never be a real index in the list
            }
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