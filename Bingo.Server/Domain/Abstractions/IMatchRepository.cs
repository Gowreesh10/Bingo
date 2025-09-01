using System;
using Bingo.Server.Domain.Entities;

namespace Bingo.Server.Domain.Abstractions
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
