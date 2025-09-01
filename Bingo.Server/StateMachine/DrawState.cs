using System;
using Bingo.Server.Domain.Entities;
using Bingo.Server.Infrastructure;

namespace Bingo.Server.StateMachine
{
    public class DrawState : IGameState
    {
        private readonly Match _match;
        private readonly JsonMatchRepository _repo;

        public DrawState(Match match, JsonMatchRepository repo)
        {
            _match = match;
            _repo = repo;
        }

        public string Name => "Draw";

        public void Enter() { }

        public void Execute()
        {
            int? drawn = _match.DrawNextNumber();

            if (drawn != null)
            {
                Console.WriteLine($"\n Number drawn: {drawn}");
                
                // Mark number on all player cards
                foreach (var player in _match.Players)
                    foreach (var card in player.Cards)
                        card.MarkNumber(drawn.Value);

                // Send number drawn message to client
                Program.OnNumberDrawn(drawn.Value);
                
                // Send updated cards to client
                foreach (var player in _match.Players)
                {
                    foreach (var card in player.Cards)
                    {
                        var cardData = Program.FormatCardForClient(card, player.Name);
                        Program.SendMessageToClient(new Bingo.Shared.Models.GameMessage(
                            Bingo.Shared.Models.MessageTypes.CardUpdate, 
                            $"Updated card for {player.Name}", 
                            cardData));
                    }
                }
            }
            else
            {
                Console.WriteLine("\nNo more numbers left to draw!");
                _match.End(); // stop the game if numbers exhausted
            }

            _repo.UpdateMatch(_match);
        }

        public IGameState Next() => new WinCheckState(_match, _repo);
    }
}
