using Bingo.src.Domain.Entities;
using Bingo.src.Infrastructure.Persistence;
using Bingo.src.Domain.Rules;
using Bingo.src.Domain.Abstractions;
using Bingo.src.Domain.Services;

namespace Bingo.src.Application.StateMachine
{
    public class WinCheckState : IGameState
    {
        private readonly Match _match;
        private readonly JSONMatchRepository _repo;
        private readonly WinEvaluator _evaluator;

        public WinCheckState(Match match, JSONMatchRepository repo)
        {
            _match = match;
            _repo = repo;
            _evaluator = new WinEvaluator(new IWinRule[]
            {
                new RowWinRule(),
                new ColumnWinRule(),
                new DiagonalWinRule()
            });
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
                        Program.PrintCard(card, player.Name);
                        _match.End();
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
