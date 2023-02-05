using _8x8.Impls;
using _8x8.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

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
            if (other == null) return false;

            return Regex.IsMatch(ValueMatch, other.Hash, RegexOptions.Singleline);
        }

        private string ValueMatch { get; set; }

        #region Private

        private void Init()
        {
            CreatePattern(FilterRule);
        }

        private void CreatePattern(IFilterRule filterRule, params string[] segments)
        {
            Type stringType = typeof(string);

            PropertyInfo pi;
            IEnumerable<PropertyInfo> filters = GetInheritancePropertiesFromIFilterRule(filterRule);

            object value;
            string val;

            ICollection<string> colPattern = new List<string>();
            ICollection<string> colValue = new List<string>();
            ICollection<string> lstSegments = new HashSet<string>();

            segments = filters.Select(c => c.Name).ToArray();

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

            Hash = string.Join("-", colPattern);
            ValueMatch = string.Join("-", colValue);
            Segments = lstSegments.ToArray();

            colPattern.Clear();
            colValue.Clear();
            lstSegments.Clear();
        }

        #endregion
    }
}