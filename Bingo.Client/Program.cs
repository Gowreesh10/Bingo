using System;
using System.Threading;
using System.Threading.Tasks;
using Bingo.Shared.Models;
using Bingo.Shared.Infrastructure;

namespace Bingo.Client
{
    class Program
    {
        private static FileMessageQueue? _serverToClientQueue;
        private static FileMessageQueue? _clientToServerQueue;
        private static bool _isRunning = false;
        private static CancellationTokenSource? _cancellationTokenSource;
        private static bool _gameStarted = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Bingo Client Starting...");
            
            try
            {
                SetupMessageQueues();
                StartListeningForServerMessages();
                
                // Get player names from user
                GetPlayerNames();
                
                Console.WriteLine("Client is running. Press ENTER to continue the game...");
                Console.WriteLine("Press 'q' to quit the client.");
                
                // Keep client running
                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                    {
                        break;
                    }
                    else if (key.Key == ConsoleKey.Enter && _gameStarted)
                    {
                        // Send continue message to server
                        SendMessageToServer(new GameMessage(MessageTypes.ContinueGame, "Continue game"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client error: {ex.Message}");
            }
            finally
            {
                Cleanup();
            }
        }

        private static void GetPlayerNames()
        {
            Console.WriteLine("\nEnter player names separated by commas (e.g., Alice,Bob):");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
            {
                input = "Alice,Bob"; // Default names
            }
            
            SendMessageToServer(new GameMessage(MessageTypes.PlayerNames, input));
        }

        private static void SetupMessageQueues()
        {
            const string serverToClientQueueName = "BingoServerToClient";
            const string clientToServerQueueName = "BingoClientToServer";

            // Connect to existing queues
            _serverToClientQueue = new FileMessageQueue(serverToClientQueueName);
            if (_serverToClientQueue.Exists())
            {
                Console.WriteLine($"Connected to server-to-client queue: {serverToClientQueueName}");
            }
            else
            {
                Console.WriteLine($"Server-to-client queue not found: {serverToClientQueueName}");
                Console.WriteLine("Make sure the server is running first.");
                return;
            }

            _clientToServerQueue = new FileMessageQueue(clientToServerQueueName);
            if (_clientToServerQueue.Exists())
            {
                Console.WriteLine($"Connected to client-to-server queue: {clientToServerQueueName}");
            }
            else
            {
                Console.WriteLine($"Client-to-server queue not found: {clientToServerQueueName}");
                Console.WriteLine("Make sure the server is running first.");
                return;
            }
        }

        private static void StartListeningForServerMessages()
        {
            if (_serverToClientQueue == null) return;

            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = true;
            
            Task.Run(() =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested && _isRunning)
                {
                    try
                    {
                        var message = _serverToClientQueue.Receive(TimeSpan.FromSeconds(1));
                        if (message != null)
                        {
                            ProcessServerMessage(message);
                        }
                    }
                    catch (Exception ex) when (ex is TimeoutException || ex is OperationCanceledException)
                    {
                        // Timeout is expected, continue listening
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing server message: {ex.Message}");
                    }
                }
            }, _cancellationTokenSource.Token);
        }

        private static void ProcessServerMessage(GameMessage gameMessage)
        {
            try
            {
                switch (gameMessage.Type)
                {
                    case MessageTypes.GameReady:
                        HandleGameReady(gameMessage);
                        break;
                    case MessageTypes.GameStarting:
                        HandleGameStarting(gameMessage);
                        break;
                    case MessageTypes.NumberDrawn:
                        HandleNumberDrawn(gameMessage);
                        break;
                    case MessageTypes.PlayerWins:
                        HandlePlayerWins(gameMessage);
                        break;
                    case MessageTypes.CardUpdate:
                        HandleCardUpdate(gameMessage);
                        break;
                    case MessageTypes.GameEnded:
                        HandleGameEnded(gameMessage);
                        break;
                    case MessageTypes.Error:
                        HandleError(gameMessage);
                        break;
                    default:
                        Console.WriteLine($"Unknown message type: {gameMessage.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing server message: {ex.Message}");
            }
        }

        private static void HandleGameReady(GameMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n {message.Content}");
            Console.ResetColor();
            
            Console.WriteLine("\nPress ENTER to start the game...");
            Console.ReadLine();
            SendMessageToServer(new GameMessage(MessageTypes.StartGame, "Start game"));
        }

        private static void HandleGameStarting(GameMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n {message.Content}");
            Console.ResetColor();
            _gameStarted = true;
        }

        private static void HandleNumberDrawn(GameMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n {message.Content}");
            Console.ResetColor();
            
            // Wait for user to press ENTER before continuing
            Console.WriteLine("Press ENTER to continue to the next number...");
            Console.ReadLine();
            
            // Send continue message to server
            SendMessageToServer(new GameMessage(MessageTypes.ContinueGame, "Continue game"));
        }

        private static void HandlePlayerWins(GameMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n {message.Content}");
            Console.ResetColor();
            
            // Display the winning card
            if (message.Data is string cardData)
            {
                Console.WriteLine("\n" + cardData);
            }
            
            // Stop the game
            _gameStarted = false;
            Console.WriteLine("\n Game Over! Press any key to exit...");
            Console.ReadKey();
            _isRunning = false;
        }

        private static void HandleCardUpdate(GameMessage message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n {message.Content}");
            Console.ResetColor();
            
            // Display the card
            if (message.Data is string cardData)
            {
                Console.WriteLine("\n" + cardData);
            }
        }

        private static void HandleGameEnded(GameMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n {message.Content}");
            Console.ResetColor();
            _gameStarted = false;
        }

        private static void HandleError(GameMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n ERROR: {message.Content}");
            Console.ResetColor();
        }

        private static void SendMessageToServer(GameMessage message)
        {
            if (_clientToServerQueue == null) return;
            
            try
            {
                _clientToServerQueue.Send(message);
                Console.WriteLine($"Sent to server: {message.Type} - {message.Content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to server: {ex.Message}");
            }
        }

        private static void Cleanup()
        {
            _isRunning = false;
            _cancellationTokenSource?.Cancel();
            
            _serverToClientQueue?.Dispose();
            _clientToServerQueue?.Dispose();
            
            Console.WriteLine("Client shutdown complete.");
        }
    }
}
