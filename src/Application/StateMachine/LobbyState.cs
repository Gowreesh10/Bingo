using Bingo.src.Domain.Entities;
using Bingo.src.Infrastructure.Persistence;

namespace Bingo.src.Application.StateMachine
{
    public class LobbyState : IGameState
    {
        private readonly Match _match;
        private readonly JSONMatchRepository _repo;

        public LobbyState(Match match, JSONMatchRepository repo)
        {
            _match = match;
            _repo = repo;
        }

        public string Name => "Lobby";

        public void Enter() { }

        public void Execute() {}

        public IGameState Next() => new TicketingState(_match, _repo);
    }
}
