using _8x8.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace _8x8.Impls
{
    public class StrategyStorage<TStrategy> : Disposable, IStrategyStorage<TStrategy>
        where TStrategy : IStrategyWrapper
    {
        private readonly List<TStrategy> strategies;

        public StrategyStorage()
        {
            strategies = new List<TStrategy>();
        }

        public void Add(TStrategy strategy)
        {
            strategies.Add(strategy);
        }

        public void AddRange(IEnumerable<TStrategy> strategies)
        {
            this.strategies.AddRange(strategies);
        }

        public IEnumerable<TStrategy> Find<T>(IFilterRuleStrategy strategy)
        {
            return strategies.Where(t => strategy.Equals(((IStrategyWrapper<T>)t).FilterRuleStrategy)).ToArray();
        }

        public void Remove(TStrategy strategy)
        {
            strategies.Remove(strategy);
        }

        protected override void DisposeCore()
        {
            base.DisposeCore();

            strategies.AsParallel().WithDegreeOfParallelism(3).ForAll(s => s.Dispose());
            strategies.Clear();
        }
    }
}