using Bingo.Server.Domain.Entities;
using Bingo.Server.Infrastructure;

namespace Bingo.Server.StateMachine
{
    public class TicketingState : IGameState
    {
        private readonly Match _match;
        private readonly JsonMatchRepository _repo;

        public TicketingState(Match match, JsonMatchRepository repo)
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
