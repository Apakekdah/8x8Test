using _8x8.HashRulesStrategy;
using _8x8.Interfaces;
using System.Collections.Generic;

namespace _8x8.Impls
{
    public class EngineStrategy : IEngineStrategy
    {
        private readonly IEnumerable<IStrategyWrapper> strategyWrappers;

        public EngineStrategy(IEnumerable<IStrategyWrapper> strategyWrappers)
        {
            this.strategyWrappers = strategyWrappers;
        }

        public IBaseRule FindRule(IFilterRule rule)
        {
            var filterRuleStrategy = new HashFilterRuleStrategy(rule);

            return null;
        }
    }
}