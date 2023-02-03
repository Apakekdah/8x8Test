using _8x8.Interfaces;
using System.Collections.Generic;

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

        public void Remove(TStrategy strategy)
        {
            strategies.Remove(strategy);
        }
    }
}