using System;
using System.Linq;
using Bingo.src.Domain.Entities;
using Bingo.src.Domain.Services;
using Bingo.src.Infrastructure.Persistence;
using Bingo.src.Application.StateMachine;

namespace Bingo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bingo Game (State Machine Version)\n");

            // Initialize JSON repository
            var repo = new JSONMatchRepository("matches.json");

            // Resume unfinished match or create new
            Match? match = repo.GetAllMatches().FirstOrDefault(m => m.State == MatchState.InProgress);

            if (match == null)
            {
                Console.WriteLine("No ongoing match found. Creating a new match...\n");
                Console.WriteLine("Enter player names separated by commas (e.g., Alice,Bob):");
                string? input = Console.ReadLine();
                var playerNames = (input ?? "Alice,Bob")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => n.Trim())
                    .ToList();

                var generator = new CardGenerator();
                match = new Match(new NumberDrawer());
                foreach (var name in playerNames)
                {
                    var card = generator.Generate();
                    var player = new Player(name);
                    player.AddCard(card);
                    match.AddPlayer(player);
                }

                match.Start();
                repo.SaveMatch(match);

                // Print initial cards for new match
                Console.WriteLine("\nInitial cards:");
                foreach (var player in match.Players)
                    foreach (var card in player.Cards)
                        PrintCard(card, player.Name);
            }
            else
            {
                Console.WriteLine($"Resuming previous match (ID: {match.Id}) with players:");
                foreach (var player in match.Players)
                    Console.WriteLine($"- {player.Name}");

                // Print current cards for resumed match
                Console.WriteLine("\nCurrent state of cards:");
                foreach (var player in match.Players)
                    foreach (var card in player.Cards)
                        PrintCard(card, player.Name);
            }

            var gameStateHandler = new GameStateHandler(match, repo);

            // Main game loop
            while (match.State != MatchState.Finished)
            {
                // Draw number, mark cards, print updated cards
                gameStateHandler.ExecuteCurrent();

                // Advance to next state (WinCheck or Result)
                gameStateHandler.MoveNext();
            }

            Console.WriteLine("\nMatch ended!");
        }

        public static void PrintCard(BingoCard card, string playerName)
        {
            Console.WriteLine($"\n{playerName}'s Card:");
            for (int r = 0; r < 5; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    var num = card.GetNumber(r, c);
                    if (num == null)
                        Console.Write("FREE ");
                    else if (num.IsMarked)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"{num.Value,2}* ");
                        Console.ResetColor();
                    }
                    else
                        Console.Write($"{num.Value,2}  ");
                }
                Console.WriteLine();
            }
        }
    }
}
