using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CSPnet2
{
    public static class GetLocalIPAddress
    {
        public static IPAddress[] GetAllAddresses()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        }

        public static IPAddress GetOnlyLocalIPAddress()
        {
            var host = GetAllAddresses();
            foreach (var ip in host)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static String ConvertIPAddressToString(IPAddress address)
        {
            StringBuilder a = new StringBuilder();
            byte[] bytes = address.GetAddressBytes();
            for (int i = 0; i < bytes.Length; i++)
            {
                a.Append(bytes[i]);
                if (i < bytes.Length - 1)
                {
                    a.Append(".");
                }                
            }
            return a.ToString();
        }

        public static String ConvertLocalIPAddressToString()
        {
            return ConvertIPAddressToString(GetOnlyLocalIPAddress());
        }
    }
}
