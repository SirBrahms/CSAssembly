namespace CSAssembly
{
    class AssemblyHandler
    {
        public Register RegEAX = new Register("EAX");
        public Register RegEBX = new Register("EBX");
        public Register RegECX = new Register("ECX");
        public Register RegEDX = new Register("EDX");
        
    }

    struct Register 
    {
        public string Name = "";
        public dynamic Contents = 0;

        public Register(string Name, dynamic? Contents = default(dynamic)) {
            this.Name = Name;
            if (Contents != default(dynamic)) this.Contents = Contents; // If Contents isn't empty, it will be assigned the Value in the Register
            else this.Contents = -1; // Else it will be assigned a -1
        }
    }
}