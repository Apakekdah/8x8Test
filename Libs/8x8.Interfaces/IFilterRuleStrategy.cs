using System;

namespace _8x8.Interfaces
{
    public interface IFilterRuleStrategy : IDisposable
    {

    }

    public interface IFilterRuleStrategy<T> : IFilterRuleStrategy, IFilterInfo<T>, IEquatable<IFilterRuleStrategy<T>>
    {
        public IFilterRule FilterRule { get; }
    }
}
