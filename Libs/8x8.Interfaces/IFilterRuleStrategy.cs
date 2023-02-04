using System;
using System.Collections.Generic;

namespace _8x8.Interfaces
{
    public interface IFilterRuleStrategy : IDisposable, IFilterInfo
    {
        public IFilterRule FilterRule { get; }
        public IEqualityComparer<IFilterRule> Comparer { get; }
        public bool Compare(int hash, IEnumerable<string> segments);
    }
}
