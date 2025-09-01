using Bingo.Server.Domain.Entities;
using Bingo.Server.Infrastructure;

namespace Bingo.Server.StateMachine
{
    public class ResultState : IGameState
    {
        private readonly Match _match;
        private readonly JsonMatchRepository _repo;

        public string Name => "Result";

        public ResultState(Match match, JsonMatchRepository repo)
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
                    // Program.PrintCard(card, player.Name); // Commented out for now
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
