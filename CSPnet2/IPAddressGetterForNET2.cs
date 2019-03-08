using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CSPnet2
{
    public static class IPAddressGetterForNET2
    {
        public static IPAddress[] GetAllLocalAddresses()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList;
        }

        public static IPAddress GetOnlyLocalIPAddress()
        {
            var host = GetAllLocalAddresses();
            foreach (var ip in host)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Debug.WriteLine("Local IPAddress found: " + ip.ToString());
                    //Check if there is a connection - Karol Pasierb
                    var connectionExisit = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
                    Debug.WriteLine("Connection status " + connectionExisit);
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
    }
}
