using System.IO;

namespace hiddentearrecoded
{
    class Logger
    {
        public static void Log(string message)
        {
            try { File.AppendAllText(Path.GetTempPath() + "\\LogFile.txt", message + "\n"); } catch { }
        }
    }
}
