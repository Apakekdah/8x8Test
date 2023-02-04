using _8x8.Interfaces;
using Autofac;
using System.Collections.Generic;

namespace _8x8.HashRulesStrategy
{
    public class HashStrategyWrapper : Disposable, IStrategyWrapper<int>
    {
        public HashStrategyWrapper(IStrategy strategy, ILifetimeScope life)
        {
            Strategy = strategy;
            Init();
            FilterRuleStrategy = life.ResolveNamed<IFilterRuleStrategy<int>>(life.Tag.ToString(), new NamedParameter("filterRule", strategy));
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

        public IEnumerable<string> Segments => FilterRuleStrategy.Segments;

        public int Hash => FilterRuleStrategy.Hash;

        public int Priority { get; private set; }

        public int RuleId { get; private set; }

        public IFilterRuleStrategy<int> FilterRuleStrategy { get; private set; }

        protected override void DisposeCore()
        {
            base.DisposeCore();
            FilterRuleStrategy.Dispose();
        }
    }
}