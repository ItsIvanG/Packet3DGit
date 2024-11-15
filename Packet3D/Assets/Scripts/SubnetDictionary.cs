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
}
