using System.Runtime.InteropServices;

namespace FroniusDataLoggerShared.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ValueUnion
    {
        [FieldOffset(1)]
        public byte ValueUpperByte;
        [FieldOffset(0)]
        public byte ValueLowerByte;
        [FieldOffset(0)]
        public Int16 ValueUnsigned;
        [FieldOffset(0)]
        public UInt16 ValueSigned;
    }
}
