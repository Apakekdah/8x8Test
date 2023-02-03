using System.Collections.Generic;

namespace _8x8.Interfaces
{
    public interface IStrategyStorage<TStrategy>
        where TStrategy : IStrategyWrapper
    {
        void Add(TStrategy strategy);
        void Remove(TStrategy strategy);
        IEnumerable<TStrategy> Find(IFilterRuleStrategy strategy);
    }
}
