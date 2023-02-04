using _8x8.Interfaces;
using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _8x8.HashRulesStrategy
{
    public class HashFilterRuleStrategy : Disposable, IFilterRuleStrategy
    {
        public readonly static int DefaultHash = 1;

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
            if (string.IsNullOrEmpty(segmentKey) && !storage.ContainsKey(segmentKey))
            {
                hashStorage = new HashStorage
                {
                    Hash = DefaultHash,
                    Segments = Array.Empty<string>()
                };
                var result = new KeyValuePair<string, HashStorage>("", hashStorage);
                storage.Add(result);
            }
            else if (!storage.ContainsKey(segmentKey))
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

        protected override void DisposeCore()
        {
            base.DisposeCore();

            storage?.Clear();
        }

        #region Private 

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
            int hash = DefaultHash;
            Type stringType = typeof(string);

            int idx = 0;
            PropertyInfo pi;
            int segmentHash = 0;

            TypeAccessor accesor = TypeAccessor.Create(filterRule.GetType());
            IEnumerable<PropertyInfo> filters = GetInheritancePropertiesFromIFilterRule(filterRule);

            object value;
            ICollection<string> lstSegments = new HashSet<string>();

            if (segments.IsEmptyArray())
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
                    segmentHash += CalculateHash((string)value);
                    hash *= segmentHash;
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

        private string CreateSegmentKey(IEnumerable<string> source)
        {
            return string.Join("+", source);
        }


        /// <summary>
        /// Copy from : https://stackoverflow.com/a/9545731
        /// But downgrade to be able use with int
        /// </summary>
        /// <param name="read">string to hash</param>
        /// <returns></returns>
        static int CalculateHash(string read)
        {
            int hashedValue = 307445734;
            for (int i = 0; i < read.Length; i++)
            {
                hashedValue += read[i];
                hashedValue *= 307445734;
            }
            return hashedValue;
        }

        class HashStorage
        {
            public IEnumerable<string> Segments { get; set; }
            public int Hash { get; set; }
        }

        #endregion
    }
}
