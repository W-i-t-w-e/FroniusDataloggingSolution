namespace FroniusDataLoggerShared.Models
{
    /// <summary>
    /// All possible query commands for the inverter
    /// </summary>
    public enum QueryCommand
    {
        // only a Placeholder
        NotSet = 0,
        // 0x01 = GetVersion
        GetVersion = 1,
        // 0x02 = Get Device,
        GetDevice = 2,
        // 0x03 = Get Time,
        GetTime = 3,
        // 0x04 = Get active inverter numbers,
        GetActiveInverterNumbers = 4,
        // 0x05 = Get active sensor card numbers,
        GetActiveSensorCardNumbers = 5,
        // 0x06 = Get LocalNet status
        GetLocalNetStatus = 6,

        // 0x0E Error Message
        ErrorMessage = 14,

        //0x10 Get power - NOW W
        GetPowerNow = 16,
        // 0x11 Get energy - TOTAL kWh
        GetEnergyTotal = 17,
        // 0x12 Get energy - DAY kWh
        GetEnergyDay = 18,
        // 0x13 Get energy - YEAR kWh
        GetEnergyYear = 19,
        // 0x14 Get AC current - NOW A
        GetAcCurrentNow = 20,
        // 0x15 Get AC voltage - NOW V
        GetAcVoltageNow = 21,
        // 0x16 Get AC frequency - NOW Hz
        GetAcFrequencyNow = 22,
        // 0x17 Get DC current - NOW A
        GetDcCurrentNow = 23,
        // 0x18 Get DC voltage - NOW V
        GetDcVoltageNow = 24,
        // 0x19 Get yield - DAY Curr.
        GetYieldDay = 25,
        // 0x1a Get maximum power - DAY W
        GetMaxPowerDay = 26,
        // 0x1b Get maximum AC voltage - DAY V
        GetMaxAcVoltageDay = 27,
        // 0x1c Get minimum AC voltage - DAY V
        GetMinAcVoltageDay = 28,
        // 0x1d Get maximum DC voltage - DAY V
        GetMaxDcVoltageDay = 29,
        // 0x1e Get operating hours - DAY Minutes
        GetOperatingHoursDay = 30,
        // 0x1f Get yield - YEAR Curr.
        GetYieldYear = 31,
        // 0x20 Get maximum power - YEAR W
        GetMaxPowerYear = 32,
        // 0x21 Get maximum AC voltage - YEAR V
        GetMaxAcVoltageYear = 33,
        // 0x22 Get minimum AC voltage - YEAR V
        GetMinAcVoltageYear = 34,
        // 0x23 Get maximum DC voltage - YEAR V
        GetMaxDcVoltageYear = 35,
        // 0x24 Get operating hours - YEAR Minutes
        GetOperatingHoursYear = 36,
        // 0x25 Get yield - TOTAL Curr.
        GetYieldTotal = 37,
        // 0x26 Get maximum power - TOTAL W
        GetMaxPowerTotal = 38,
        // 0x27 Get maximum AC voltage - TOTAL V
        GetMaxAcVoltageTotal = 39,
        // 0x28 Get minimum AC voltage - TOTAL V
        GetMinAcVoltageTotal = 40,
        // 0x29 Get maximum DC voltage - TOTAL V
        GetMaxDcVoltageTotal = 41,
        // 0x2a Get operating hours - TOTAL Minutes
        GetOperatingHoursTotal = 42,

        #region NotUsedForIG35Plus
        //// ----3 Phase Inverters
        //// 0x2b Get phase current for phase 1 A
        //GetPhase1Current = 43,
        //// 0x2c Get phase current for phase 2 A
        //GetPhase2Current = 44,
        //// 0x2d Get phase current for phase 3 A
        //GetPhase3Current = 45,
        //// 0x2e Get phase voltage for phase 1 V
        //GetPhase1Voltage = 46,
        //// 0x2f Get phase voltage for phase 2 V
        //GetPhase2Voltage = 47,
        //// 0x30 Get phase voltage for phase 3 V
        //GetPhase3Voltage = 48,
        // 0x31 Ambient temperature ° C
        GetAmbientTemperature = 49,
        //// 0x32 Front left fan rotation speed rpm
        //GetFrontLeftFanSpeed = 50,
        //// 0x33 Front right fan rotation speed rpm
        //GetFrontRightFanSpeed = 51,
        //// 0x34 Rear left fan rotation speed rpm
        //GetRearLeftFanSpeed = 52,
        //// 0x35 Rear right fan rotation speed rpm
        //GetRearRightFanSpeed = 52,

        //// ---SensorCard---
        // 0xe0 Get temperature channel 1 - NOW (2)
        GetTempCh1Now = 224,
        // 0xe1 Get temperature channel 2 - NOW (2)
        GetTempCh2Now = 225,
        // 0xe2 Get insolation - NOW W/m²
        GetInsolationNow = 226,
        // 0xe3 Get minimal temperature channel 1 - DAY (2)
        GetMinTempCh1Day = 227,
        // 0xe4 Get maximum temperature channel 1 - DAY (2)
        GetMaxTempCh1Day = 228,
        // 0xe5 Get minimal temperature channel 1 - YEAR (2)
        GetMinTempCh1Year = 229,
        // 0xe6 Get maximum temperature channel 1 - YEAR (2)
        GetMaxTempCh1Year = 230,
        // 0xe7 Get minimal temperature channel 1 - TOTAL (2)
        GetMinTempCh1Total = 231,
        // 0xe8 Get maximum temperature channel 1 - TOTAL (2)
        GetMaxTempCh1Total = 232,
        // 0xe9 Get minimal temperature channel 2 - DAY (2)
        GetMinTempCh2Day = 233,
        // 0xea Get maximum temperature channel 2 - DAY (2)
        GetMaxTempCh2Day = 234,
        // 0xeb Get minimal temperature channel 2 - YEAR (2)
        GetMinTempCh2Year = 235,
        // 0xec Get maximum temperature channel 2 - YEAR (2)
        GetMaxTempCh2Year = 236,
        // 0xed Get minimal temperature channel 2 - TOTAL (2)
        GetMinTempCh2Total = 237,
        // 0xee Get maximum temperature channel 2 - TOTAL (2)
        GetMaxTempCh2Total = 238,
        // 0xef Get maximum insolation - DAY W/m²
        GetMaxInsolationDay = 239,
        // 0xf0 Get maximum insolation - YEAR W/m²
        GetMaxInsolationYear = 240,
        // 0xf1 Get maximum insolation - TOTAL W/m²
        GetMaxInsolationTotal = 241,
        // 0xf2 Get value of digital channel 1 - NOW (2)
        GetValueOfDigitalCh1Now = 242,
        // 0xf3 Get value of digital channel 2 - NOW (2)
        GetValueOfDigitalCh2Now = 243,
        // 0xf4 Get maximum of digital channel 1 - DAY (2)
        GetMaxOfDigitalCh1Day = 244,
        // 0xf5 Get maximum of digital channel 1 - YEAR (2)
        GetMaxOfDigitalCh1Year = 245,
        // 0xf6 Get maximum of digital channel 1 - TOTAL (2)
        GetMaxOfDigitalCh1Total = 246,
        // 0xf7 Get maximum of digital channel 2 - DAY (2)
        GetMaxOfDigitalCh2Day = 247,
        // 0xf8 Get maximum of digital channel 2 - YEAR (2)
        GetMaxOfDigitalCh2Year = 248,
        // 0xf9 Get maximum of digital channel 2 - TOTAL (2)
        GetMaxOfDigitalCh2Total = 249

        //// (2) Depending on the settings on the device (e.g. °C or °F) 
        #endregion
    }
}
