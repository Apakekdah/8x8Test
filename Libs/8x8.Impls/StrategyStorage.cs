using _8x8.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace _8x8.Impls
{
    public class StrategyStorage<TStrategy> : IStrategyStorage<TStrategy>
        where TStrategy : IStrategyWrapper
    {
        private readonly ICollection<TStrategy> strategies;

        public StrategyStorage()
        {
            strategies = new HashSet<TStrategy>();
        }

        public void Add(TStrategy strategy)
        {
            strategies.Add(strategy);
        }

        public IEnumerable<TStrategy> Find(IFilterRuleStrategy strategy)
        {
            return strategies.Where(t => strategy.Compare(t.Hash, t.Segments)).ToArray();
        }

        public void Remove(TStrategy strategy)
        {
            strategies.Remove(strategy);
        }
    }
}