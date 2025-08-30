
namespace Bingo.src.Application.StateMachine
{
    public class ResultState : IGameState
    {
        public string Name => "Result";
        public void Enter() => Console.WriteLine("Entering Result: Announcing winners...");
        public void Execute() => Console.WriteLine("Displaying results...");
        public IGameState Next() => new LobbyState(); 
    }
}
