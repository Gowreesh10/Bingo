using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.src.Application.StateMachine
{
    public class LobbyState : IGameState
    {
        public string Name => "Lobby";
        public void Enter() => Console.WriteLine("Entering Lobby: Waiting for players...");
        public void Execute() => Console.WriteLine("Lobby active...");
        public IGameState Next() => new TicketingState();
    }
}
