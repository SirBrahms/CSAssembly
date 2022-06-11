namespace CSAssembly.Types
{
    // This Namespace is for non-static Types
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
}