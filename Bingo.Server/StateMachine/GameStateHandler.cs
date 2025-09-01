using Bingo.Server.Domain.Entities;
using Bingo.Server.Infrastructure;

namespace Bingo.Server.StateMachine
{
    public class GameStateHandler
    {
        private IGameState _currentState;

        public GameStateHandler(Match match, JsonMatchRepository repo)
        {
            _currentState = new DrawState(match, repo); // Start with drawing numbers
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
