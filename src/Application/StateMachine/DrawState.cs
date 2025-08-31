using System;
using Bingo.src.Domain.Entities;
using Bingo.src.Infrastructure.Persistence;

namespace Bingo.src.Application.StateMachine
{
    public class DrawState : IGameState
    {
        private readonly Match _match;
        private readonly JSONMatchRepository _repo;

        public DrawState(Match match, JSONMatchRepository repo)
        {
            _match = match;
            _repo = repo;
        }

        public string Name => "Draw";

        public void Enter() { }

        public void Execute()
        {
            int? drawn = _match.DrawNextNumber();

            if (drawn != null)
            {
                Console.WriteLine($"\n Number drawn: {drawn}");
                
                // Wait for user input before printing updated cards
                Console.WriteLine("\nPress ENTER to view updated cards...");
                Console.ReadLine();

                // Mark number on all player cards
                foreach (var player in _match.Players)
                    foreach (var card in player.Cards)
                        card.MarkNumber(drawn.Value);

                // Print updated cards
                foreach (var player in _match.Players)
                    foreach (var card in player.Cards)
                        Program.PrintCard(card, player.Name);
            }
            else
            {
                Console.WriteLine("\nNo more numbers left to draw!");
                _match.End(); // stop the game if numbers exhausted
            }

            _repo.UpdateMatch(_match);
        }

        public IGameState Next() => new WinCheckState(_match, _repo);
    }
}
