using _8x8.Impls;
using _8x8.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace _8x8.HashRulesStrategy
{
    public class HashFilterRuleStrategy : BaseFilterRuleStrategy<int>
    {
        public readonly static int DefaultHash = 1;

        private readonly IDictionary<string, HashStorage> storage;

        private readonly object locker = new object();

        public HashFilterRuleStrategy(IFilterRule filterRule) : base(filterRule)
        {
            storage = new Dictionary<string, HashStorage>(StringComparer.InvariantCultureIgnoreCase);
            Init();
        }

        public override bool Equals([AllowNull] IFilterRuleStrategy<int> other)
        {
            if (other == null) return false;

            HashStorage hashStorage;
            //lock (locker)
            {
                var segmentKey = CreateSegmentKey(other.Segments);
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
                    var result = CreateStrategyHashSegment(false, FilterRule, other.Segments.ToArray());
                    storage.Add(result);
                    hashStorage = result.Value;
                }
                else
                {
                    hashStorage = storage[segmentKey];
                }
            }
            return hashStorage.Hash == other.Hash;
        }

        protected override void DisposeCore()
        {
            base.DisposeCore();

            storage?.Clear();
        }

        public override void CreateAllCombination()
        {
            base.CreateAllCombination();

            storage.Clear();

            foreach (var segments in Combinations)
            {
                //var result = CreateStrategyHashSegment(false, FilterRule, (string[])Convert.ChangeType(segments, typeof(string[])));
                var result = CreateStrategyHashSegment(false, FilterRule, Array.ConvertAll(segments, c => c?.ToString() ?? ""));
                storage.Add(result);
                Segments = result.Value.Segments;
                Hash = result.Value.Hash;
            }
        }

        #region Private 

        private void Init()
        {
            var result = CreateStrategyHashSegment(true, FilterRule);
            storage.Add(result);
            Segments = result.Value.Segments;
            Hash = result.Value.Hash;
        }

        private KeyValuePair<string, HashStorage> CreateStrategyHashSegment(bool init, IFilterRule filterRule, params string[] segments)
        {
            int hash = DefaultHash;
            Type stringType = typeof(string);

            int idx = 0;
            PropertyInfo pi;
            int segmentHash = 0;
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
                value = Accesor[filterRule, pi.Name];
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

            var arr = lstSegments.ToArray();

            lstSegments.Clear();

            return new KeyValuePair<string, HashStorage>(CreateSegmentKey(arr), new HashStorage
            {
                Segments = arr,
                Hash = hash
            });
        }

        private string CreateSegmentKey(IEnumerable<string> source) => string.Join("+", source);

        /// <summary>
        /// Copy from : https://stackoverflow.com/a/9545731
        /// But downgrade to be able use with int
        /// </summary>
        /// <param name="read">string to hash</param>
        /// <returns></returns>
        static int CalculateHash(string read)
        {
            const int Hash = 307445734;
            int hashedValue = Hash;
            for (int i = 0; i < read.Length; i++)
            {
                hashedValue += read[i];
                hashedValue *= Hash;
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
