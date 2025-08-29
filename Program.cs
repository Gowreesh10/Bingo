using System;
using System.Collections.Generic;
using Bingo.src.Domain.Entities;

namespace Bingo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🎉 Bingo Game Simulation 🎉\n");

            // Step 1: Create numbers for Bingo cards (5x5 example)
            int[,] numbers1 = new int[5, 5]
            {
                { 1, 16, 31, 46, 61 },
                { 2, 17, 32, 47, 62 },
                { 3, 18, 33, 48, 63 },
                { 4, 19, 34, 49, 64 },
                { 5, 20, 35, 50, 65 }
            };

            int[,] numbers2 = new int[5, 5]
            {
                { 10, 25, 40, 55, 70 },
                { 11, 26, 41, 56, 71 },
                { 12, 27, 42, 57, 72 },
                { 13, 28, 43, 58, 73 },
                { 14, 29, 44, 59, 74 }
            };

            // Step 2: Create Bingo cards
            BingoCard card1 = new BingoCard(numbers1);
            BingoCard card2 = new BingoCard(numbers2);

            // Step 3: Create players and assign cards
            Player p1 = new Player("Alice");
            p1.AddCard(card1);

            Player p2 = new Player("Bob");
            p2.AddCard(card2);

            // Step 4: Create a Match with a pool of numbers
            var drawNumbers = new List<int>() { 1, 17, 32, 47, 62, 29, 44, 59, 74 };
            Match match = new Match(drawNumbers);

            match.AddPlayer(p1);
            match.AddPlayer(p2);

            match.Start();
            Console.WriteLine("Match started!\n");

            // Step 5: Draw numbers and mark cards
            int? drawn;
            while ((drawn = match.DrawNextNumber()) != null)
            {
                Console.WriteLine($"Number drawn: {drawn}");

                foreach (var player in match.Players)
                {
                    foreach (var card in player.Cards)
                    {
                        card.MarkNumber(drawn.Value);
                    }
                }
            }

            match.End();
            Console.WriteLine("\nMatch ended!");
        }
    }
}
