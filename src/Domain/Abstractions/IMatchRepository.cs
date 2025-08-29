using System;
using Bingo.src.Domain.Entities;

namespace Bingo.src.Domain.Abstractions
{
    public interface IMatchRepository
    {
        void SaveMatch(Match match);

        Match? GetMatchById(Guid id);

        void UpdateMatch(Match match);

        void DeleteMatch(Guid id);

        IEnumerable<Match> GetAllMatches();
    }
}
