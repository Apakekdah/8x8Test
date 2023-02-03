using _8x8.Interfaces;
using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _8x8.RulesStrategy
{
    public class HashFilterRuleStrategy : IFilterRuleStrategy
    {
        private static readonly string ANY = "<ANY>";

        private readonly IDictionary<string, HashStorage> storage;

        public HashFilterRuleStrategy(IFilterRule filterRule)
        {
            FilterRule = filterRule;

            storage = new Dictionary<string, HashStorage>(StringComparer.InvariantCultureIgnoreCase);
            Init();
        }

        public IEnumerable<string> Segments { get; private set; }

        public int Hash { get; private set; }

        public IFilterRule FilterRule { get; private set; }

        public IEqualityComparer<IFilterRule> Comparer => throw new NotImplementedException();

        private void Init()
        {
            var result = CreateStrategyHashSegment(FilterRule);
            storage.Add(result);
            Segments = result.Value.Segments;
            Hash = result.Value.Hash;
        }

        private IEnumerable<PropertyInfo> GetInheritancePropertiesFromIFilterRule(IFilterRule filterRule)
        {
            Type typeFilterRule = typeof(IFilterRule);
            var types = filterRule.GetType().GetInterfaces().Where(c => typeFilterRule.IsAssignableFrom(c) && !typeFilterRule.Equals(c)).ToArray();
            foreach (var type in types)
            {
                foreach (var pi in type.GetProperties())
                {
                    yield return pi;
                }
            }
        }

        private (IEnumerable<string>, int) CreateHash(IFilterRule filterRule)
        {
            int hash = 0;
            List<string> segments = new List<string>();
            Type stringType = typeof(string);

            TypeAccessor accesor = TypeAccessor.Create(filterRule.GetType());
            IEnumerable<PropertyInfo> filters = GetInheritancePropertiesFromIFilterRule(filterRule);

            object value;

            int idx = 1;
            foreach (var filter in filters)
            {
                value = accesor[filterRule, filter.Name];
                if (filter.PropertyType.Equals(stringType))
                {
                    if (ANY.Equals((string)value, StringComparison.InvariantCultureIgnoreCase))
                        continue;
                    hash += ((string)value).Select(x => x * (idx + 1)).Sum();
                }
                else
                {
                    hash += value.GetHashCode();
                }
                idx++;

                segments.Add(filter.Name);
            }

            return (segments.ToArray(), hash);
        }

        private KeyValuePair<string, HashStorage> CreateStrategyHashSegment(IFilterRule filterRule, params string[] segments)
        {
            int hash = 0;
            Type stringType = typeof(string);

            TypeAccessor accesor = TypeAccessor.Create(filterRule.GetType());
            IEnumerable<PropertyInfo> filters = GetInheritancePropertiesFromIFilterRule(filterRule);

            object value;

            if (IsEmptySegments(segments))
            {
                segments = filters.Select(c => c.Name).ToArray();
            }

            int idx = 1;
            PropertyInfo pi;
            foreach (var segment in segments)
            {
                pi = filters.FirstOrDefault(p => p.Name.Equals(segment));
                if (pi == null) continue;
                value = accesor[filterRule, pi.Name];
                if (pi.PropertyType.Equals(stringType))
                {
                    if (ANY.Equals((string)value, StringComparison.InvariantCultureIgnoreCase))
                        continue;
                    hash += ((string)value).Select(x => x * (idx + 1)).Sum();
                }
                else
                {
                    hash += value.GetHashCode();
                }
                idx++;
            }

            return new KeyValuePair<string, HashStorage>(CreateSegemntKey(segments), new HashStorage
            {
                Segments = segments,
                Hash = hash
            });
        }

        private bool IsEmptySegments(IEnumerable<string> segments)
        {
            return segments?.Any() == false;
        }

        private string CreateSegemntKey(IEnumerable<string> source)
        {
            return string.Join("+", source);
        }

        class HashStorage
        {
            public IEnumerable<string> Segments { get; set; }
            public int Hash { get; set; }
        }
    }
}
