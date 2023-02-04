using System;
using System.Collections.Generic;

namespace _8x8.Interfaces
{
    public interface IStrategyStorage<TStrategy> : IDisposable
        where TStrategy : IStrategyWrapper
    {
        void Add(TStrategy strategy);
        void AddRange(IEnumerable<TStrategy> strategies);
        void Remove(TStrategy strategy);
        IEnumerable<TStrategy> Find<T>(IFilterRuleStrategy strategy);
    }
}
