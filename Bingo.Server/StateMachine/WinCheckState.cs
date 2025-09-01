using Bingo.Server.Domain.Entities;
using Bingo.Server.Infrastructure;
using Bingo.Server.Domain.Services;

namespace Bingo.Server.StateMachine
{
    public class WinCheckState : IGameState
    {
        private readonly Match _match;
        private readonly JsonMatchRepository _repo;
        private readonly WinEvaluator _evaluator;

        public WinCheckState(Match match, JsonMatchRepository repo)
        {
            _match = match;
            _repo = repo;
            _evaluator = new WinEvaluator(); // Simplified for now
        }

        public string Name => "WinCheck";

        public void Enter() { }

        public void Execute()
        {
            foreach (var player in _match.Players)
            {
                foreach (var card in player.Cards)
                {
                    if (_evaluator.IsWinning(card))
                    {
                        Console.WriteLine($"\n {player.Name} wins!");
                        _match.End();
                        
                        // Send win message to client
                        Program.OnPlayerWins(player.Name, card);
                        return; // Exit early when someone wins
                    }
                }
            }

            _repo.UpdateMatch(_match);
        }

        public IGameState Next()
        {
            return _match.State == MatchState.Finished
                ? new ResultState(_match, _repo)
                : new DrawState(_match, _repo);
        }
    }
}
