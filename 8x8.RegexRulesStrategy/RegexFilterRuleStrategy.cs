using _8x8.Impls;
using _8x8.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace _8x8.RegexRulesStrategy
{
    public class RegexFilterRuleStrategy : BaseFilterRuleStrategy<string>
    {
        public RegexFilterRuleStrategy(IFilterRule filterRule) : base(filterRule)
        {
            Init();
        }

        public override bool Equals([AllowNull] IFilterRuleStrategy<string> other)
        {
            throw new NotImplementedException();
        }

        private string ValueMatch { get; set; }
        private string Pattern { get; set; }

        #region Private

        private void Init()
        {
            //var result = CreateStrategyHashSegment(true, FilterRule);
            //storage.Add(result);
            //Segments = result.Value.Segments;
            //Hash = result.Value.Hash;
        }

        private void CreatePattern(bool init, IFilterRule filterRule, params string[] segments)
        {
            Type stringType = typeof(string);

            PropertyInfo pi;
            IEnumerable<PropertyInfo> filters = GetInheritancePropertiesFromIFilterRule(filterRule);

            object value;
            string val;
            ICollection<string> lstSegments = new HashSet<string>();

            if (segments.IsEmptyArray())
            {
                if (init)
                {
                    segments = filters.Select(c => c.Name).ToArray();
                }
            }

            ICollection<string> colPattern = new HashSet<string>();
            ICollection<string> colValue = new HashSet<string>();

            foreach (var segment in segments)
            {
                pi = filters.FirstOrDefault(p => p.Name.Equals(segment));
                if (pi == null) continue;
                value = Accesor[filterRule, pi.Name];
                if (pi.PropertyType.Equals(stringType))
                {
                    val = (string)value;
                }
                else
                {
                    val = (string)Convert.ChangeType(value, stringType);
                }
                if (StrategyFilterRule.ANY.Equals((string)value, StringComparison.InvariantCultureIgnoreCase))
                {
                    colPattern.Add("(.*?)");
                }
                else
                {
                    colPattern.Add($"[{val}]{{{val.Length}}}");
                }
                colValue.Add(val);
                lstSegments.Add(segment);
            }

            Pattern = string.Join("-", colPattern);
        }

        #endregion
    }
}