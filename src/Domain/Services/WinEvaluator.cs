using Bingo.src.Domain.Entities;
using Bingo.src.Domain.Abstractions;

namespace Bingo.src.Domain.Services
{
    public class WinEvaluator
    {
        private readonly List<IWinRule> _winRules;

        public WinEvaluator(IEnumerable<IWinRule> winRules)
        {
            _winRules = new List<IWinRule>(winRules);
        }

        public bool IsWinning(BingoCard card)
        {
            foreach (var rule in _winRules)
            {
                if (rule.IsWinning(card))
                    return true;
            }
            return false;
        }
    }
}
