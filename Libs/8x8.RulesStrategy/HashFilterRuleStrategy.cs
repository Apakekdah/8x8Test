using _8x8.Interfaces;
using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _8x8.HashRulesStrategy
{
    public class HashFilterRuleStrategy : IFilterRuleStrategy
    {
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

        public bool Compare(int hash, IEnumerable<string> segments)
        {
            HashStorage hashStorage;
            var segmentKey = CreateSegmentKey(segments);
            if (!storage.ContainsKey(segmentKey))
            {
                var result = CreateStrategyHashSegment(false, FilterRule, segments.ToArray());
                storage.Add(result);
                hashStorage = result.Value;
            }
            else
            {
                hashStorage = storage[segmentKey];
            }
            return hashStorage.Hash == hash;
        }

        private void Init()
        {
            var result = CreateStrategyHashSegment(true, FilterRule);
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

        private KeyValuePair<string, HashStorage> CreateStrategyHashSegment(bool init, IFilterRule filterRule, params string[] segments)
        {
            int hash = 17;
            Type stringType = typeof(string);

            TypeAccessor accesor = TypeAccessor.Create(filterRule.GetType());
            IEnumerable<PropertyInfo> filters = GetInheritancePropertiesFromIFilterRule(filterRule);

            object value;
            ICollection<string> lstSegments = new HashSet<string>();

            if (IsEmptySegments(segments))
            {
                if (init)
                {
                    segments = filters.Select(c => c.Name).ToArray();
                }
                else
                {
                    return new KeyValuePair<string, HashStorage>(string.Empty, new HashStorage());
                }
            }

            int idx = 0;
            PropertyInfo pi;
            int segmentHash = 0;
            foreach (var segment in segments)
            {
                segmentHash = idx;
                pi = filters.FirstOrDefault(p => p.Name.Equals(segment));
                if (pi == null) continue;
                value = accesor[filterRule, pi.Name];
                if (pi.PropertyType.Equals(stringType))
                {
                    if (StrategyFilterRule.ANY.Equals((string)value, StringComparison.InvariantCultureIgnoreCase))
                        continue;
                    //hash += ((string)value).Select(x => x * (idx + 1)).Sum() * idx;
                    segmentHash += CalculateHash((string)value);
                    hash *= 23 + segmentHash;
                }
                else
                {
                    hash += value.GetHashCode();
                }
                idx++;
                lstSegments.Add(segment);
            }

            return new KeyValuePair<string, HashStorage>(CreateSegmentKey(lstSegments), new HashStorage
            {
                Segments = lstSegments.ToArray(),
                Hash = hash
            });
        }

        private bool IsEmptySegments(IEnumerable<string> segments)
        {
            return segments?.Any() == false;
        }

        private string CreateSegmentKey(IEnumerable<string> source)
        {
            return string.Join("+", source);
        }

        static int CalculateHash(string read)
        {
            //int hashedValue = 307445734;
            //for (int i = 0; i < read.Length; i++)
            //{
            //    hashedValue += read[i];
            //    hashedValue *= 307445734;
            //}
            //return hashedValue;
            var hash1 = (5381 << 16) + 5381;
            var hash2 = hash1;

            for (int i = 0; i < read.Length; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ read[i];
                if (i == read.Length - 1)
                {
                    break;
                }

                hash2 = ((hash2 << 5) + hash2) ^ read[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }

        class HashStorage
        {
            public IEnumerable<string> Segments { get; set; }
            public int Hash { get; set; }
        }
    }
}
