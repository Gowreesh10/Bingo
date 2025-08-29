using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.src.Domain.Entities
{
    public class Player
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public List<BingoCard> Cards { get; }

        public Player(string name)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Cards = new List<BingoCard>();
        }

        public void AddCard(BingoCard card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            Cards.Add(card);
        }
    }
}
