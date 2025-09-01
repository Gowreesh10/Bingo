using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using Bingo.Server.Domain.Entities;
using Bingo.Server.Domain.Abstractions;

namespace Bingo.Server.Infrastructure
{
    public class JsonMatchRepository : IMatchRepository
    {
        private readonly string _filePath;

        public JsonMatchRepository(string filePath)
        {
            _filePath = filePath;
            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        public void SaveMatch(Match match)
        {
            var matches = GetAllMatches().ToList();
            matches.Add(match);
            File.WriteAllText(_filePath, JsonSerializer.Serialize(matches, new JsonSerializerOptions { WriteIndented = true }));
        }

        public Match? GetMatchById(Guid id)
        {
            return GetAllMatches().FirstOrDefault(m => m.Id == id);
        }

        public void UpdateMatch(Match match)
        {
            var matches = GetAllMatches().ToList();
            var index = matches.FindIndex(m => m.Id == match.Id);
            if (index != -1)
                matches[index] = match;

            File.WriteAllText(_filePath, JsonSerializer.Serialize(matches, new JsonSerializerOptions { WriteIndented = true }));
        }

        public void DeleteMatch(Guid id)
        {
            var matches = GetAllMatches().Where(m => m.Id != id).ToList();
            File.WriteAllText(_filePath, JsonSerializer.Serialize(matches, new JsonSerializerOptions { WriteIndented = true }));
        }

        public IEnumerable<Match> GetAllMatches()
        {
            var content = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Match>>(content) ?? new List<Match>();
        }
    }
}
