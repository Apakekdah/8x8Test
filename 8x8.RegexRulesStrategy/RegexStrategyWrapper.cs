using _8x8.Interfaces;
using System;
using System.Collections.Generic;

namespace _8x8.RegexRulesStrategy
{
    public class RegexStrategyWrapper : Disposable, IStrategyWrapper<string>
    {
        public IFilterRuleStrategy<string> FilterRuleStrategy => throw new NotImplementedException();

        public int RuleId => throw new NotImplementedException();

        public int Priority => throw new NotImplementedException();

        public IStrategy Strategy => throw new NotImplementedException();

        public IEnumerable<string> Segments => throw new NotImplementedException();

        public string Hash => throw new NotImplementedException();
    }
}