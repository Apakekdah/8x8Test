using _8x8.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace _8x8.RegexRulesStrategy
{
    public class RegexFilterRuleStrategy : Disposable, IFilterRuleStrategy<string>
    {
        public IFilterRule FilterRule => throw new NotImplementedException();

        public IEnumerable<string> Segments => throw new NotImplementedException();

        public string Hash => throw new NotImplementedException();

        public bool Equals([AllowNull] IFilterRuleStrategy<string> other)
        {
            throw new NotImplementedException();
        }
    }
}