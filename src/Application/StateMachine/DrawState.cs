
namespace Bingo.src.Application.StateMachine
{
    public class DrawState : IGameState
    {
        public string Name => "Draw";
        public void Enter() => Console.WriteLine("Entering Draw: Drawing numbers...");
        public void Execute() => Console.WriteLine("Drawing numbers...");
        public IGameState Next() => new WinCheckState();
    }
}
