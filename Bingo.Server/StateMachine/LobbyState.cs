using Bingo.Server.Domain.Entities;
using Bingo.Server.Infrastructure;

namespace Bingo.Server.StateMachine
{
    public class LobbyState : IGameState
    {
        private readonly Match _match;
        private readonly JsonMatchRepository _repo;

        public LobbyState(Match match, JsonMatchRepository repo)
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
