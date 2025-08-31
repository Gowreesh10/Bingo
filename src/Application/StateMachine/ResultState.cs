using Bingo.src.Domain.Entities;
using Bingo.src.Infrastructure.Persistence;

namespace Bingo.src.Application.StateMachine
{
    public class ResultState : IGameState
    {
        private readonly Match _match;
        private readonly JSONMatchRepository _repo;

        public string Name => "Result";

        public ResultState(Match match, JSONMatchRepository repo)
        {
            _match = match;
            _repo = repo;
        }

        public void Enter() => Console.WriteLine("Thanks for Playing!!");

        public void Execute()
        {
            Console.WriteLine("Displaying final results...\n");
            Console.WriteLine("Press ENTER to view all final cards...");
            Console.ReadLine();

            foreach (var player in _match.Players)
            {
                foreach (var card in player.Cards)
                {
                    Program.PrintCard(card, player.Name);
                }
            }

            _repo.UpdateMatch(_match);
        }

        public IGameState Next()
        {
            // Move back to Lobby after result
            return new LobbyState(_match, _repo);
        }
    }
}
