using Bingo.src.Domain.Entities;
using Bingo.src.Infrastructure.Persistence;

namespace Bingo.src.Application.StateMachine
{
    public class TicketingState : IGameState
    {
        private readonly Match _match;
        private readonly JSONMatchRepository _repo;

        public TicketingState(Match match, JSONMatchRepository repo)
        {
            _match = match;
            _repo = repo;
        }

        public string Name => "Ticketing";

        public void Enter() { }

        public void Execute() {}

        public IGameState Next() => new DrawState(_match, _repo);
    }
}
