using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bingo.src.Domain.Abstractions;

namespace Bingo.src.Infrastructure.Rules.Dynamic
{
    public static class RuleLoader
    {
        public static IEnumerable<IWinRule> LoadRules()
        {
            var ruleType = typeof(IWinRule);
            var types = Assembly.GetExecutingAssembly()
                                .GetTypes()
                                .Where(t => ruleType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is IWinRule rule)
                    yield return rule;
            }
        }
    }
}
