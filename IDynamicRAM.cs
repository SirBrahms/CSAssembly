namespace CSAssembly.Types
{
    public interface IDynamicRAM
    {
        // List that Represents the RAM (add Private set in implementation)
        public LinkedList<byte> RAM {get;}

        // Function to write one Byte to the next free Position in memory
        public bool WriteByte(byte ByteToWrite);
        
        // Function to read one byte from a specified Address
        public byte ReadByte(int Address);

        // Function to Write multiple bytes to the next Location that is big enough to fit all the Bytes chained together
        public bool WriteBytes(byte[] BytesToWrite);

        // Function to Read a range of bytes From Memory
        public byte[] ReadBytes(int From, int To);

        // Function to free Resources
        public bool Free(int From, int To);
    }
}