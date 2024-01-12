namespace SimplySpoof
{
    internal class Models
    {
        internal struct SpoofPointersStruct
        {
            internal SpoofPointersStruct(string version, string didPtr, string mcidPtr)
            {
                this.version = version;
                this.didPtr = didPtr;
                this.mcidPtr = mcidPtr;
            }
            internal string version;
            internal string didPtr;
            internal string mcidPtr;
        }
    }
}
