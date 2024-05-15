namespace FroniusDataLoggerShared.Models
{
    /// <summary>
    /// All known error types for the LocalNet
    /// </summary>
    public enum ErrorType
    {
        //0x01 Unknown command
        UnknownCommand = 1,
        //0x02 Timeout
        //A command or measured value query has not been executed within a specified period of time in the LocalNet ring
        Timeout = 2,
        //0x03 Incorrect data structure
        IncorrectDataStructure = 3,
        //0x04 Queue of commands awaiting execution is full
        //Wait until the last command has been executed
        ExecutionQueueIsFull = 4,
        //0x05 Device or option not present
        //The device or option to which the command was directed is not present in the LocalNet ring
        DeviceNotPresent = 5,
        //0x06 No response from device or option
        //The device or option to which the command was directed is not responding
        NoResponseFromDevice = 6,
        //0x07 Sensor error
        //The device or option to which the command was directed is reporting a sensor error
        SensorError = 7,
        //0x08 Sensor not active
        //This is output when the selected channel is not active
        SensorNotActive = 8,
        //0x09 Incorrect command for device or option
        //The command cannot be executed in conjunction with the selected device or option
        IncorrectCommandForDevice = 9,
    }
}
