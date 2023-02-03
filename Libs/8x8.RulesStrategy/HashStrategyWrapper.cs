using _8x8.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace _8x8.RulesStrategy
{
    public class HashStrategyWrapper : IStrategyWrapper
    {
        public static readonly string ANY = "<ANY>";

        private readonly IStrategy strategy;
        private readonly IFilterRuleStrategy filterRuleStrategy;

        public HashStrategyWrapper(IStrategy strategy)
        {
            this.strategy = strategy;
            filterRuleStrategy = new HashFilterRuleStrategy((IFilterRule)strategy);

            Init();
        }

        public IStrategy Strategy => strategy;

        public IEnumerable<string> Segments { get; private set; }

        public int Hash { get; private set; }

        public int Priority { get; private set; }

        public int CompareTo([AllowNull] IStrategyWrapper other)
        {
            if (other == null) throw new ArgumentNullException("other");
            if (other.Hash != Hash)
                return 0;
            return 1;
        }

        #region Private 

        private void Init()
        {
            ExtractStrategy();
        }

        private void ExtractStrategy()
        {
            IBaseRule rule = (IBaseRule)strategy;

            Priority = rule.Priority;
        }

        #endregion
    }
}
