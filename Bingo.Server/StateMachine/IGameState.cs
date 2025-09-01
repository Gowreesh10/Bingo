
namespace Bingo.Server.StateMachine
{
    public interface IGameState
    {
        void Enter();       
        void Execute();     
        IGameState Next();  
        string Name { get; } 
    }
}
