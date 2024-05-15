namespace FroniusDataLoggerShared.Models
{
    public class ResponseModel
    {
        // Number of bytes in data field (1 byte)
        public int Length { get; set; }

        // Type, e.g.inverter, sensor box etc. (1 byte)
        public DeviceType Device { get; set; }

        // Number of the device in question (1 byte)
        public int Number { get; set; }

        // Queries the command to be executed (1 byte)
        public QueryCommand Command { get; set; }

        // Contains the value of the checked command (max. 127 bytes)
        public List<byte> Data { get; set; } = new();

    }
}
