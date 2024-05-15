using FroniusDataLoggerShared.Models;

namespace FroniusInterfaceCardLibrary.Helpers
{
    public static class ByteHelpers
    {
        /// <summary>
        /// Creates the send byte array
        /// </summary>
        /// <param name="query">the query command to send</param>
        /// <param name="deviceType">The device th query</param>
        /// <param name="deviceNumber">The Id of the device</param>
        /// <returns></returns>
        public static byte[] SetSendSequence(QueryCommand query, DeviceType deviceType, int deviceNumber)
        {
            // Create the byte array to send a request
            var sendByte = new byte[8];

            // Add start sequence 808080
            sendByte[0] = 128;
            sendByte[1] = 128;
            sendByte[2] = 128;

            // Number of bytes in data field (1 byte => Querylength is always 0, because no data)
            sendByte[3] = 0;

            // Set the device 0x00 = Interface Card, 0x01 = Inverter, 0x02 = Sensor Card
            sendByte[4] = (byte)deviceType;

            // Set the Number of the device in question => if device is Interface number is 00 / ignored
            sendByte[5] = (byte)deviceNumber;

            // Get command (Queries the command to be executed)
            sendByte[6] = (byte)query;

            // +++Calculate Checksum:
            var byChkSum = (byte)((byte)deviceType + deviceNumber + (byte)query);
            var checkSend = byChkSum.ByteToString();                // get checksum string
            sendByte[7] = checkSend.StringToByte();

            return sendByte;
        }


        /// <summary>
        /// 
        ///METHOD **************** Buliding a Byte out of High and Low Chars ************
        ///                                                                                 
        ///       Converts char into a hex value (Byte: 0xXX)
        ///       e.g. 'A' ==> "41" or 'P' ==> "50"
        ///                                                                                
        ///       Date: 06.09.2005                                   Autor: B.M.           
        ///******************************************************************************
        /// </summary>
        /// <param name="sConvIn"></param>
        /// <returns></returns>
        internal static byte StringToByte(this string sConvIn)
        {
            byte byHighByte = 0;
            byte byLowByte = 0;
            byte byHlByte;
            byte byJ;
            char cAktByteHigh, cAktByteLow;
            char[] hexZahl = new char[16] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            sConvIn = sConvIn.ToUpper();
            cAktByteHigh = Convert.ToChar(sConvIn.Substring(0, 1));
            for (byJ = 0; byJ <= 15; byJ++)
            {
                byHighByte = byJ;                              // Dezimalwert dem Hexwert zuordnen
                if (cAktByteHigh.Equals(hexZahl[byJ]))         // Vergleichen auf Hexwert
                {
                    break;                                  // bei Übereinstimmung abbrechen
                }
            }
            cAktByteLow = Convert.ToChar(sConvIn.Substring(1, 1));     // Low Nibble Char einlesen
            for (byJ = 0; byJ <= 15; byJ++)
            {
                byLowByte = byJ;                               // Dezimalwert dem Hexwert zuordnen
                if (cAktByteLow.Equals(hexZahl[byJ]))          // Vergleichen auf Hexwert
                {
                    break;                                  // bei Übereinstimmung abbrechen
                }
            }

            byHlByte = Convert.ToByte(byHighByte * 16 + byLowByte);
            return byHlByte;
        }

        /// <summary>
        /// Adds a byte to a byte array
        /// </summary>
        /// <param name="bArray">The source array</param>
        /// <param name="newByte">The byte to add</param>
        /// <returns>the byte appended array</returns>
        internal static byte[] AddByteToArray(byte[] bArray, byte newByte)
        {
            var newArray = new byte[bArray.Length + 1];
            bArray.CopyTo(newArray, 0);
            newArray[bArray.Length] = newByte;
            return newArray;
        }

        /// <summary>
        /// METHOD ***************  Convert a "Byte-value" to Hex-String   *************************
        /// 
        ///     Converts a byte value into a hex-string
        ///     Umwandlung eines Byte-Wertes (0-255) in einen "Hex-String" 
        ///    
        ///     zB.: 35 ==> "23" oder 255 ==> "FF"
        /// 
        ///     Date: 02.10.2005                                   Autor: B.M.           
        /// ****************************************************************************************
        /// </summary>
        /// <param name="byIn"></param>
        /// <returns></returns>
        internal static string ByteToString(this byte byIn)
        {
            byte byChkUpper, byChkLower;
            char cChkUpper, cChkLower;


            byChkUpper = (byte)(byIn / 16);
            if (byChkUpper < 10)
            {
                cChkUpper = Convert.ToChar(byChkUpper + 48);
            }
            else
            {
                cChkUpper = (char)(byChkUpper + 55);
            }
            byChkLower = (byte)(byIn % 16);

            if (byChkLower < 10)
            {
                cChkLower = Convert.ToChar(byChkLower + 48);
            }
            else
            {
                cChkLower = (char)(byChkLower + 55);
            }
            return "" + cChkUpper + cChkLower;
        }

        /// <summary>
        /// An extension method to return the first index of a given byte array pattern 
        /// https://social.msdn.microsoft.com/Forums/azure/zh-CN/15514c1a-b6a1-44f5-a06c-9b029c4164d7/searching-a-byte-array-for-a-pattern-of-bytes?forum=csharpgeneral
        /// </summary>
        /// <param name="arrayToSearchThrough"></param>
        /// <param name="patternToFind"></param>
        /// <returns></returns>
        public static int GetIndexOfSequence(this byte[] arrayToSearchThrough, byte[] patternToFind)
        {
            // check if the pattern is larger than the array to search at
            if (patternToFind.Length > arrayToSearchThrough.Length)
                // return not found
                return -1;

            // starting from the beginning to the end minus the pattern length
            // NOTE: If the item you are searching for is at the end of the array, this won't work. It is off by 1 index. Here it is corrected with a <=, instead of <
            // NOTE: for (int i = 0; i <= arrayToSearchThrough.Length - patternToFind.Length; i++)
            for (int i = 0; i < arrayToSearchThrough.Length - patternToFind.Length; i++)
            {
                // initially set the found to true
                bool found = true;
                // loop through the pattern length and check each index of the pattern against the actual i index of the searcharray (i + pattern j to count up to the lenght of the pattern)
                for (int j = 0; j < patternToFind.Length; j++)
                {
                    // check if actual i index matches the pattern j index (i +j to check from the beginning to the end of the pattern length)
                    if (arrayToSearchThrough[i + j] != patternToFind[j])
                    {
                        // if no match set to false and get on with the next index of the search array
                        found = false;
                        break;
                    }
                }
                // if found
                if (found)
                {
                    // tell the start index of the search array
                    return i;
                }
            }
            // return not found
            return -1;
        }
    }
}
