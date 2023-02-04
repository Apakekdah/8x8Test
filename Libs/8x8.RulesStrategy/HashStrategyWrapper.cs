using _8x8.Interfaces;
using Autofac;
using System.Collections.Generic;

namespace _8x8.HashRulesStrategy
{
    public class HashStrategyWrapper : Disposable, IStrategyWrapper
    {
        private readonly IFilterRuleStrategy filterRuleStrategy;

        public HashStrategyWrapper(IStrategy strategy, ILifetimeScope life)
        {
            Strategy = strategy;
            Init();
            filterRuleStrategy = life.ResolveNamed<IFilterRuleStrategy>(life.Tag.ToString(), new NamedParameter("filterRule", strategy));
        }

        private void Init()
        {
            IBaseRule rule = (IBaseRule)Strategy;
            if (rule != null)
            {
                Priority = rule.Priority;
                RuleId = rule.RuleId;
            }
        }

        public IStrategy Strategy { get; private set; }

        public IEnumerable<string> Segments => filterRuleStrategy.Segments;

        public int Hash => filterRuleStrategy.Hash;

        public int Priority { get; private set; }

        public int RuleId { get; private set; }

        protected override void DisposeCore()
        {
            base.DisposeCore();
            filterRuleStrategy.Dispose();
        }
    }
}