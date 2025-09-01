using Bingo.Server.Domain.Entities;
using Bingo.Server.Domain.Abstractions;
using Bingo.Server.Domain.Rules;

namespace Bingo.Server.Domain.Services
{
    public class WinEvaluator
    {
        private readonly List<IWinRule> _winRules;

        public WinEvaluator()
        {
            _winRules = new List<IWinRule>
            {
                new RowWinRule(),
                new ColumnWinRule(),
                new DiagonalWinRule()
            };
        }

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
