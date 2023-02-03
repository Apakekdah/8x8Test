using System.Collections.Generic;

namespace _8x8.Interfaces
{
    public interface IFilterRuleStrategy : IFilterInfo
    {
        public IFilterRule FilterRule { get; }
        public IEqualityComparer<IFilterRule> Comparer { get; }
    }
}
