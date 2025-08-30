
namespace Bingo.src.Application.StateMachine
{
    public interface IGameState
    {
        void Enter();       
        void Execute();     
        IGameState Next();  
        string Name { get; } 
    }
}
