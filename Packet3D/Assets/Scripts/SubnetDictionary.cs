using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SubnetDictionary : MonoBehaviour
{
    public static Dictionary<string, string> prefixAddressPairs = new Dictionary<string, string>()
    {
        {"255.0.0.0", "/8"},
        {"255.128.0.0", "/9"},
        {"255.192.0.0", "/10"},
        {"255.224.0.0", "/11"},
        {"255.240.0.0", "/12"},
        {"255.248.0.0", "/13"},
        {"255.252.0.0", "/14"},
        {"255.254.0.0", "/15"},
        {"255.255.0.0", "/16"},
        {"255.255.128.0", "/17"},
        {"255.255.192.0", "/18"},
        {"255.255.224.0", "/19"},
        {"255.255.240.0", "/20"},
        {"255.255.248.0", "/21"},
        {"255.255.252.0", "/22"},
        {"255.255.254.0", "/23"},
        {"255.255.255.0", "/24"},
        {"255.255.255.128", "/25"},
        {"255.255.255.192", "/26"},
        {"255.255.255.224", "/27"},
        {"255.255.255.240", "/28"},
        {"255.255.255.248", "/29"},
        {"255.255.255.252", "/30"},
        {"255.255.255.254", "/31"},
        {"255.255.255.255", "/32"},

    };

    public static Dictionary<string, int> prefixHostPairs = new Dictionary<string, int>()
    {
        {"/8", 16777214},
        {"/9", 8388606},
        {"/10", 4194302},
        {"/11", 2097150},
        {"/12", 1048574},
        {"/13", 524286},
        {"/14", 262142},
        {"/15", 131070},
        {"/16", 65534},
        {"/17", 32766},
        {"/18", 16382},
        {"/19", 8190},
        {"/20", 4094},
        {"/21", 2046},
        {"/22", 1022},
        {"/23", 510},
        {"/24", 254},
        {"/25", 126},
        {"/26", 62},
        {"/27", 30},
        {"/28", 14},
        {"/29", 6},
        {"/30", 2},
        {"/31",0 },
        {"/32",0 }
    };
    public static string ConvertCIDRToSubnetMask(int cidrSuffix)
    {
        if (cidrSuffix < 0 || cidrSuffix > 32)
        {
            throw new ArgumentOutOfRangeException(nameof(cidrSuffix), "CIDR suffix must be between 0 and 32.");
        }

        // Create a 32-bit binary number with 'cidrSuffix' 1s followed by 0s
        uint mask = uint.MaxValue << (32 - cidrSuffix);

        // Convert the binary number to its IP format
        return string.Format("{0}.{1}.{2}.{3}",
            (mask >> 24) & 0xFF,
            (mask >> 16) & 0xFF,
            (mask >> 8) & 0xFF,
            mask & 0xFF);
    }
    public static string getPrefix(string address)
    {
        string output;

        if(prefixAddressPairs.TryGetValue(address, out output))
        {
            return output;
        }
        else
        {
            return "/?";
        }

        
    }
    public static bool IsValidIPAddress(string ipString)
    {
        if (ipString.Split(".").Length == 4)
        {
            if (IPAddress.TryParse(ipString, out IPAddress ipAddress))
            {
                Debug.Log($"{ipString} is a valid IP address.");
                return true;
            }
            else
            {
                Debug.Log($"{ipString} is not a valid IP address.");
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Finds the last available IP address in a subnet.
    /// </summary>
    /// <param name="ip">The IP address.</param>
    /// <param name="mask">The subnet mask.</param>
    /// <returns>The last available IP address as a string.</returns>
    public static string GetLastAvailableIPAddress(string ip, string mask)
    {
        try
        {
            // Convert IP and mask to 32-bit unsigned integers
            uint ipUint = IPToUInt32(IPAddress.Parse(ip));
            uint maskUint = IPToUInt32(IPAddress.Parse(mask));

            // Calculate the broadcast address
            uint broadcastUint = ipUint | ~maskUint;

            // Convert the broadcast address back to a string
            return UInt32ToIP(broadcastUint);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in GetLastAvailableIPAddress: {ex.Message}");
            return null;
        }
    }
    public static string GetNetworkAddress(string ip, string mask)
    {
        try
        {
            // Convert IP and mask to 32-bit unsigned integers
            uint ipUint = IPToUInt32(IPAddress.Parse(ip));
            uint maskUint = IPToUInt32(IPAddress.Parse(mask));

            // Perform bitwise AND to get the network address
            uint networkUint = ipUint & maskUint;

            // Convert back to an IP address string
            return UInt32ToIP(networkUint);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in GetNetworkAddress: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Converts an IP address to a 32-bit unsigned integer.
    /// </summary>
    private static uint IPToUInt32(IPAddress ipAddress)
    {
        byte[] bytes = ipAddress.GetAddressBytes();
        Array.Reverse(bytes); // Ensure correct endianness
        return BitConverter.ToUInt32(bytes, 0);
    }

    /// <summary>
    /// Converts a 32-bit unsigned integer to an IP address string.
    /// </summary>
    private static string UInt32ToIP(uint ipAddress)
    {
        byte[] bytes = BitConverter.GetBytes(ipAddress);
        Array.Reverse(bytes); // Ensure correct endianness
        return new IPAddress(bytes).ToString();
    }
}
