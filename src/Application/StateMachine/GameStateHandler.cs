using Bingo.src.Domain.Entities;
using Bingo.src.Infrastructure.Persistence;

namespace Bingo.src.Application.StateMachine
{
    public class GameStateHandler
    {
        private IGameState _currentState;

        public GameStateHandler(Match match, JSONMatchRepository repo)
        {
            _currentState = new LobbyState(match, repo); // pass dependencies to first state
            _currentState.Enter();
        }

        public string CurrentStateName => _currentState.Name;

        public void MoveNext()
        {
            _currentState = _currentState.Next();
            _currentState.Enter();
        }

        public void ExecuteCurrent() => _currentState.Execute();
    }
}
