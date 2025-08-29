using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bingo.src.Domain.Entities
{
    public class Match
    {
        public Guid Id { get; }
        public List<Player> Players { get; }
        public Queue<int> DrawPool { get; }
        public MatchState State { get; private set; }

        public Match(IEnumerable<int> drawNumbers)
        {
            Id = Guid.NewGuid();
            Players = new List<Player>();
            DrawPool = new Queue<int>(drawNumbers);
            State = MatchState.Waiting;
        }

        public void AddPlayer(Player player)
        {
            if (State != MatchState.Waiting)
                throw new InvalidOperationException("Cannot add players after match has started.");

            Players.Add(player);
        }

        public int? DrawNextNumber()
        {
            if (DrawPool.Count == 0)
                return null;

            return DrawPool.Dequeue();
        }

        public void Start()
        {
            if (State != MatchState.Waiting)
                throw new InvalidOperationException("Match already started or finished.");

            State = MatchState.InProgress;
        }

        public void End()
        {
            State = MatchState.Finished;
        }
    }

    public enum MatchState
    {
        Waiting,
        InProgress,
        Finished
    }
}
