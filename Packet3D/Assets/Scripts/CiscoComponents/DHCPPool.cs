using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

[System.Serializable]
public class DHCPPool
{
    public string DHCPName;
    [Tooltip("Format: 192.168.1.1/24")]
    public string network;
    [Tooltip("Format: 192.168.1.1")]
    public string defaultGateway;
    [Tooltip("Format: 192.168.1.1")]
    public string defaultDNS;
    public string exceptionRangeStart = "0.0.0.0";
    public string exceptionRangeEnd = "0.0.0.0";
    public string ipRangeStart = ""; // Start of IP range
    public string ipRangeEnd = "";
    public List<string> existingIPs = new List<string>();
    public string GetNextAvailableIP()
    {
        if (ipRangeStart == "" || ipRangeStart == null) ipRangeStart = network.Split("/")[0];
        if (ipRangeEnd == "" || ipRangeEnd == null) ipRangeEnd = SubnetDictionary.GetLastAvailableIPAddress(network.Split("/")[0], SubnetDictionary.ConvertCIDRToSubnetMask(int.Parse(network.Split("/")[1])));
        if (defaultGateway == "" || defaultGateway == null) defaultGateway = "0.0.0.0";
        if (defaultDNS == "" || defaultDNS == null) defaultDNS = "0.0.0.0";
        if (exceptionRangeStart == "" || exceptionRangeStart == null) exceptionRangeStart = "0.0.0.0";
        if (exceptionRangeEnd == "" || exceptionRangeEnd == null) exceptionRangeEnd = "0.0.0.0";


        try
        {
            // Parse the start and end IP addresses
            IPAddress startIP = IPAddress.Parse(ipRangeStart);
            IPAddress endIP = IPAddress.Parse(ipRangeEnd);

            // Convert to integers for iteration
            uint start = IPToUInt32(startIP);
            uint end = IPToUInt32(endIP);
            uint exceptionStart = IPToUInt32(IPAddress.Parse(exceptionRangeStart));
            uint exceptionEnd = IPToUInt32(IPAddress.Parse(exceptionRangeEnd));

            // Iterate through the range to find an available IP
            for (uint current = start; current <= end; current++)
            {
                string candidateIP = UInt32ToIP(current);

                // Skip IPs in the exception range or already assigned
                if ((current >= exceptionStart && current <= exceptionEnd) || existingIPs.Contains(candidateIP))
                {
                    continue;
                }

                // Mark the IP as used and return it
                existingIPs.Add(candidateIP);
                return candidateIP;
            }

            // No available IP found
            Debug.LogWarning("No available IP addresses in the specified range.");
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in GetNextAvailableIP: {ex.Message}");
            return null;
        }
    }
    /// <summary>
    /// Converts an IP address to a 32-bit unsigned integer.
    /// </summary>
    private uint IPToUInt32(IPAddress ipAddress)
    {
        byte[] bytes = ipAddress.GetAddressBytes();
        Array.Reverse(bytes); // Ensure correct endianness
        return BitConverter.ToUInt32(bytes, 0);
    }

    /// <summary>
    /// Converts a 32-bit unsigned integer to an IP address string.
    /// </summary>
    private string UInt32ToIP(uint ipAddress)
    {
        byte[] bytes = BitConverter.GetBytes(ipAddress);
        Array.Reverse(bytes); // Ensure correct endianness
        return new IPAddress(bytes).ToString();
    }
}
