using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bingo.Server.Domain.Entities;
using Bingo.Server.Domain.Services;
using Bingo.Server.Infrastructure;
using Bingo.Shared.Models;
using Bingo.Shared.Infrastructure;
using Bingo.Server.StateMachine;

namespace Bingo.Server
{
    class Program
    {
        private static FileMessageQueue? _serverToClientQueue;
        private static FileMessageQueue? _clientToServerQueue;
        private static Match? _currentMatch;
        private static JsonMatchRepository? _repository;
        private static GameStateHandler? _gameStateHandler;
        private static bool _gameRunning = false;
        private static CancellationTokenSource? _cancellationTokenSource;
        private static string[]? _playerNames;

        static void Main(string[] args)
        {
            Console.WriteLine("Bingo Server Starting...");
            
            try
            {
                SetupMessageQueues();
                StartListeningForClientMessages();
                
                Console.WriteLine("Server is running. Waiting for client connections...");
                Console.WriteLine("Press 'q' to quit the server.");
                
                // Keep server running
                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
            }
            finally
            {
                Cleanup();
            }
        }

        private static void SetupMessageQueues()
        {
            const string serverToClientQueueName = "BingoServerToClient";
            const string clientToServerQueueName = "BingoClientToServer";

            // Create or connect to server-to-client queue
            _serverToClientQueue = new FileMessageQueue(serverToClientQueueName);
            if (!_serverToClientQueue.Exists())
            {
                _serverToClientQueue.Create();
                Console.WriteLine($"Created queue: {serverToClientQueueName}");
            }
            else
            {
                Console.WriteLine($"Connected to existing queue: {serverToClientQueueName}");
            }

            // Create or connect to client-to-server queue
            _clientToServerQueue = new FileMessageQueue(clientToServerQueueName);
            if (!_clientToServerQueue.Exists())
            {
                _clientToServerQueue.Create();
                Console.WriteLine($"Connected to existing queue: {clientToServerQueueName}");
            }
            else
            {
                Console.WriteLine($"Connected to existing queue: {clientToServerQueueName}");
            }
        }

        private static void StartListeningForClientMessages()
        {
            if (_clientToServerQueue == null) return;

            _cancellationTokenSource = new CancellationTokenSource();
            
            Task.Run(() =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        var message = _clientToServerQueue.Receive(TimeSpan.FromSeconds(1));
                        if (message != null)
                        {
                            ProcessClientMessage(message);
                        }
                    }
                    catch (Exception ex) when (ex is TimeoutException || ex is OperationCanceledException)
                    {
                        // Timeout is expected, continue listening
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing client message: {ex.Message}");
                    }
                }
            }, _cancellationTokenSource.Token);
        }

        private static void ProcessClientMessage(GameMessage gameMessage)
        {
            try
            {
                Console.WriteLine($"Received from client: {gameMessage.Type} - {gameMessage.Content}");

                switch (gameMessage.Type)
                {
                    case MessageTypes.PlayerNames:
                        HandlePlayerNames(gameMessage.Content);
                        break;
                    case MessageTypes.StartGame:
                        HandleStartGame();
                        break;
                    case MessageTypes.ContinueGame:
                        HandleContinueGame();
                        break;
                    default:
                        Console.WriteLine($"Unknown message type: {gameMessage.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing client message: {ex.Message}");
                SendMessageToClient(new GameMessage(MessageTypes.Error, $"Error processing message: {ex.Message}"));
            }
        }

        private static void HandlePlayerNames(string playerNamesInput)
        {
            _playerNames = playerNamesInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(n => n.Trim())
                .ToArray();

            Console.WriteLine($"Received player names: {string.Join(", ", _playerNames)}");
            
            // Initialize the game with the provided player names
            InitializeGame();
            
            SendMessageToClient(new GameMessage(MessageTypes.GameReady, $"Game initialized with players: {string.Join(", ", _playerNames)}"));
        }

        private static void HandleStartGame()
        {
            if (_currentMatch == null)
            {
                SendMessageToClient(new GameMessage(MessageTypes.Error, "Game not initialized. Please provide player names first."));
                return;
            }

            _gameRunning = true;
            SendMessageToClient(new GameMessage(MessageTypes.GameStarting, "Game starting!"));
            
            // Start the game loop
            StartGameLoop();
        }

        private static void HandleContinueGame()
        {
            if (_gameStateHandler != null)
            {
                // Execute current state (draw number, check wins, etc.)
                _gameStateHandler.ExecuteCurrent();
                
                // Advance to next state
                _gameStateHandler.MoveNext();
                
                // Check if game is finished
                if (_currentMatch?.State == MatchState.Finished)
                {
                    _gameRunning = false;
                    SendMessageToClient(new GameMessage(MessageTypes.GameEnded, "Game has ended!"));
                }
            }
        }

        private static void InitializeGame()
        {
            if (_playerNames == null || _playerNames.Length == 0)
            {
                _playerNames = new[] { "Player1", "Player2" };
            }

            _repository = new JsonMatchRepository("server_matches.json");
            
            // Create a new match
            var generator = new CardGenerator();
            _currentMatch = new Match(new NumberDrawer());
            
            foreach (var name in _playerNames)
            {
                var card = generator.Generate();
                var player = new Player(name);
                player.AddCard(card);
                _currentMatch.AddPlayer(player);
            }
            
            _currentMatch.Start();
            _repository.SaveMatch(_currentMatch);
            
            _gameStateHandler = new GameStateHandler(_currentMatch, _repository);
            
            Console.WriteLine("Game initialized with players:");
            foreach (var player in _currentMatch.Players)
            {
                Console.WriteLine($"- {player.Name}");
            }
            
            // Send initial cards to client
            SendInitialCards();
        }

        private static void SendInitialCards()
        {
            if (_currentMatch == null) return;
            
            foreach (var player in _currentMatch.Players)
            {
                foreach (var card in player.Cards)
                {
                    var cardData = FormatCardForClient(card, player.Name);
                    SendMessageToClient(new GameMessage(MessageTypes.CardUpdate, $"Initial card for {player.Name}", cardData));
                }
            }
        }

        private static void StartGameLoop()
        {
            if (_gameStateHandler == null) return;
            
            // The game loop will be controlled by client messages
            // Each time client sends "continue", we execute one step
            Console.WriteLine("Game loop started. Waiting for client to continue...");
        }

        public static void SendMessageToClient(GameMessage message)
        {
            if (_serverToClientQueue == null) return;
            
            try
            {
                _serverToClientQueue.Send(message);
                Console.WriteLine($"Sent to client: {message.Type} - {message.Content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to client: {ex.Message}");
            }
        }

        public static string FormatCardForClient(BingoCard card, string playerName)
        {
            var result = $"{playerName}'s Card:\n";
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    var num = card.GetNumber(r, c);
                    if (num == null)
                        result += "FREE ";
                    else if (num.IsMarked)
                        result += $"{num.Value,2}* ";
                    else
                        result += $"{num.Value,2}  ";
                }
                result += "\n";
            }
            return result;
        }

        // Hook into the existing game logic to send messages
        public static void OnNumberDrawn(int number)
        {
            SendMessageToClient(new GameMessage(MessageTypes.NumberDrawn, $"Number drawn: {number}", number));
        }

        public static void OnPlayerWins(string playerName, BingoCard winningCard)
        {
            var cardData = FormatCardForClient(winningCard, playerName);
            SendMessageToClient(new GameMessage(MessageTypes.PlayerWins, $"{playerName} wins!", cardData));
        }

        public static void OnCardUpdate(BingoCard card, string playerName)
        {
            var cardData = FormatCardForClient(card, playerName);
            SendMessageToClient(new GameMessage(MessageTypes.CardUpdate, $"Updated card for {playerName}", cardData));
        }

        private static void Cleanup()
        {
            _gameRunning = false;
            _cancellationTokenSource?.Cancel();
            
            _serverToClientQueue?.Dispose();
            _clientToServerQueue?.Dispose();
            
            Console.WriteLine("Server shutdown complete.");
        }
    }
}
