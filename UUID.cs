using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;

namespace hiddentearrecoded
{

    //Code from https://www.b4x.com/android/forum/threads/b4x-account-class-get-a-deviceuid-to-create-a-account-without-registration.101943/
    class UUID
    {
        public static string Get()
        {
            StringBuilder sb = new StringBuilder();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_Processor");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                sb.Append(queryObj["NumberOfCores"]);
                sb.Append(queryObj["ProcessorId"]);
                sb.Append(queryObj["Name"]);
                sb.Append(queryObj["SocketDesignation"]);
            }

            searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                sb.Append(queryObj["Manufacturer"]);
                sb.Append(queryObj["Name"]);
                sb.Append(queryObj["Version"]);

            }

            searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                sb.Append(queryObj["Product"]);
            }

            var bytes = Encoding.ASCII.GetBytes(sb.ToString());
            SHA256Managed sha = new SHA256Managed();

            byte[] hash = sha.ComputeHash(bytes);

            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}
