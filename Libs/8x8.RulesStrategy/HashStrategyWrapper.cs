using _8x8.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace _8x8.HashRulesStrategy
{
    public class HashStrategyWrapper : IStrategyWrapper
    {
        private readonly IStrategy strategy;
        private readonly IFilterRuleStrategy filterRuleStrategy;

        public HashStrategyWrapper(IStrategy strategy, ILifetimeScope life)
        {
            this.strategy = strategy;
            Init();
            filterRuleStrategy = life.ResolveNamed<IFilterRuleStrategy>(life.Tag.ToString(), new NamedParameter("filterRule", strategy));
        }

        private void Init()
        {
            IBaseRule rule = (IBaseRule)strategy;
            if (rule != null)
            {
                Priority = rule.Priority;
            }
        }

        public IStrategy Strategy => strategy;

        public IEnumerable<string> Segments => filterRuleStrategy.Segments;

        public int Hash => filterRuleStrategy.Hash;

        public int Priority { get; private set; }

        public int CompareTo([AllowNull] IStrategyWrapper other)
        {
            if (other == null) throw new ArgumentNullException("other");
            if (other.Hash != Hash)
                return 0;
            return 1;
        }
    }
}
