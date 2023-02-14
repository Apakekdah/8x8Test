using System;

namespace _8x8.Interfaces
{
    public interface IFilterRuleStrategy : IDisposable
    {
        void CreateAllCombination();
    }

    public interface IFilterRuleStrategy<T> : IFilterRuleStrategy, IFilterInfo<T>, IEquatable<IFilterRuleStrategy<T>>
    {
        IFilterRule FilterRule { get; }
    }
}