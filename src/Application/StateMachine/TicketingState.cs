
namespace Bingo.src.Application.StateMachine
{
    public class TicketingState : IGameState
    {
        public string Name => "Ticketing";
        public void Enter() => Console.WriteLine("Entering Ticketing: Players select cards...");
        public void Execute() => Console.WriteLine("Ticketing active...");
        public IGameState Next() => new DrawState();
    }
}
