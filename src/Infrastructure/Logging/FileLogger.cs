using System;
using System.IO;

namespace Bingo.src.Infrastructure.Logging
{
    public class FileLogger
    {
        private readonly string _filePath;

        public FileLogger(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "");
        }

        public void LogInfo(string message) => Log("[INFO]", message);
        public void LogWarning(string message) => Log("[WARN]", message);
        public void LogError(string message) => Log("[ERROR]", message);

        private void Log(string level, string message)
        {
            File.AppendAllText(_filePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {level} {message}{Environment.NewLine}");
        }
    }
}
