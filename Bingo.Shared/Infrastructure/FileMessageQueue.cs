using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Bingo.Shared.Models;

namespace Bingo.Shared.Infrastructure
{
    public class FileMessageQueue
    {
        private readonly string _queueDirectory;
        private readonly string _queueName;
        private readonly string _queuePath;
        private readonly Timer _cleanupTimer;

        public FileMessageQueue(string queueName)
        {
            _queueName = queueName;
            _queueDirectory = Path.Combine(Path.GetTempPath(), "BingoQueues");
            _queuePath = Path.Combine(_queueDirectory, _queueName);
            
            // Ensure directory exists
            Directory.CreateDirectory(_queueDirectory);
            
            // Cleanup old messages every 30 seconds
            _cleanupTimer = new Timer(CleanupOldMessages, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
        }

        public void Send(GameMessage message)
        {
            try
            {
                var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}.json";
                var filePath = Path.Combine(_queuePath, fileName);
                
                // Ensure queue directory exists
                Directory.CreateDirectory(_queuePath);
                
                var json = JsonConvert.SerializeObject(message, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        public GameMessage? Receive(TimeSpan timeout)
        {
            var startTime = DateTime.Now;
            
            while (DateTime.Now - startTime < timeout)
            {
                try
                {
                    if (!Directory.Exists(_queuePath))
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    var files = Directory.GetFiles(_queuePath, "*.json");
                    if (files.Length > 0)
                    {
                        // Get the oldest file
                        var oldestFile = files.OrderBy(f => File.GetCreationTime(f)).First();
                        
                        try
                        {
                            var json = File.ReadAllText(oldestFile);
                            var message = JsonConvert.DeserializeObject<GameMessage>(json);
                            
                            // Delete the processed message
                            File.Delete(oldestFile);
                            
                            return message;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing message file {oldestFile}: {ex.Message}");
                            // Delete corrupted file
                            try { File.Delete(oldestFile); } catch { }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error receiving message: {ex.Message}");
                }
                
                Thread.Sleep(100);
            }
            
            return null; // Timeout
        }

        public bool Exists()
        {
            return Directory.Exists(_queuePath);
        }

        public void Create()
        {
            Directory.CreateDirectory(_queuePath);
        }

        private void CleanupOldMessages(object? state)
        {
            try
            {
                if (!Directory.Exists(_queuePath)) return;
                
                var files = Directory.GetFiles(_queuePath, "*.json");
                var cutoffTime = DateTime.Now.AddMinutes(-5); // Remove messages older than 5 minutes
                
                foreach (var file in files)
                {
                    if (File.GetCreationTime(file) < cutoffTime)
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch
                        {
                            // Ignore deletion errors
                        }
                    }
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        public void Dispose()
        {
            _cleanupTimer?.Dispose();
        }
    }
}
