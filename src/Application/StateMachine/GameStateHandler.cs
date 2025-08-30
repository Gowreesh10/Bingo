
namespace Bingo.src.Application.StateMachine
{
    public class GameStateHandler
    {
        private IGameState _currentState;

        public GameStateHandler()
        {
            _currentState = new LobbyState(); 
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
