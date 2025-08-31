using System;
using System.Collections.Generic;
using Bingo.src.Domain.Services;

namespace Bingo.src.Domain.Entities
{
    public class Match
    {
        public Guid Id { get; set; }  // public setter for JSON
        public List<Player> Players { get; set; } = new List<Player>();
        public MatchState State { get; set; }

        private NumberDrawer _drawer;

        // Parameterless constructor for JSON deserialization
        public Match()
        {
            _drawer = new NumberDrawer(); // recreate drawer when loading from JSON
        }

        // Constructor for new matches
        public Match(NumberDrawer drawer)
        {
            _drawer = drawer ?? throw new ArgumentNullException(nameof(drawer));
            Id = Guid.NewGuid();
            Players = new List<Player>();
            State = MatchState.Waiting;
        }

        public void AddPlayer(Player player)
        {
            if (State != MatchState.Waiting)
                throw new InvalidOperationException("Cannot add players after match has started.");

            Players.Add(player ?? throw new ArgumentNullException(nameof(player)));
        }

        public int? DrawNextNumber()
        {
            if (State != MatchState.InProgress)
                throw new InvalidOperationException("Match is not in progress.");

            return _drawer.Draw();
        }

        public bool HasNumbersLeft() => _drawer.HasNumbersLeft();

        public void Start()
        {
            if (State != MatchState.Waiting)
                throw new InvalidOperationException("Match already started or finished.");
            if (Players.Count == 0)
                throw new InvalidOperationException("Cannot start match without players.");

            State = MatchState.InProgress;
        }

        public void End() => State = MatchState.Finished;
    }

    public enum MatchState
    {
        Waiting,
        InProgress,
        Finished
    }
}
