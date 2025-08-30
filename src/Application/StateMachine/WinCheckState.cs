
namespace Bingo.src.Application.StateMachine
{
    public class WinCheckState : IGameState
    {
        public string Name => "WinCheck";
        public void Enter() => Console.WriteLine("Entering WinCheck: Evaluating winners...");
        public void Execute() => Console.WriteLine("Checking for winning cards...");
        public IGameState Next() => new ResultState();
    }
}
