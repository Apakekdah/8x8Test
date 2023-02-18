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
        public readonly static int DefaultHash = 3;

        private readonly IDictionary<string, HashStorage> storage;

        private readonly ICollection<int> hashed;

        private IEnumerable<PropertyInfo> filterProperties;
        private IEnumerable<string> filterSegments;

        public HashFilterRuleStrategy(IFilterRule filterRule) : base(filterRule)
        {
            storage = new Dictionary<string, HashStorage>(StringComparer.InvariantCultureIgnoreCase);
            hashed = new HashSet<int>();
            Init();
        }

        public override bool Equals([AllowNull] IFilterRuleStrategy<int> other)
        {
            if (other == null) return false;

            return hashed.Contains(other.Hash);

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
                    var result = CreateStrategyHashSegment(false, other.Segments.ToArray());
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

            storage.Clear();
            hashed.Clear();
        }

        public override void CreateAllCombination()
        {
            base.CreateAllCombination();

            storage.Clear();

            IEnumerable<string> filters = GetInheritancePropertiesFromIFilterRule(FilterRule).Select(pi => pi.Name).OrderBy(c => c);
            KeyValuePair<string, HashStorage> result;

            foreach (var segments in Combinations)
            {
                //var result = CreateStrategyHashSegment(false, (string[])Convert.ChangeType(segments, typeof(string[])));
                //var result = CreateStrategyHashSegment(false, Array.ConvertAll(segments, c => c?.ToString() ?? ""));
                result = CreateStrategyHashSegment(false, CreateFakeSegments(filters, segments));
                storage.Add(result);
                Segments = result.Value.Segments;
                Hash = result.Value.Hash;
                hashed.Add(result.Value.Hash);
            }

            result = CreateStrategyHashSegment(false);
            storage.Add(result);
            Segments = result.Value.Segments;
            Hash = result.Value.Hash;
            hashed.Add(result.Value.Hash);
        }

        #region Private 

        private void Init()
        {
            filterProperties = GetInheritancePropertiesFromIFilterRule(FilterRule).ToArray();
            filterSegments = filterProperties.Select(pi => pi.Name).OrderBy(c => c).ToArray();

            var result = CreateStrategyHashSegment(true);
            storage.Add(result);
            Segments = result.Value.Segments;
            Hash = result.Value.Hash;
        }

        private string[] CreateFakeSegments(IEnumerable<string> filters, object[] segments)
        {
            ICollection<string> newSegments = new HashSet<string>();
            foreach (var fs in filters)
            {
                if (segments.Contains(fs))
                {
                    newSegments.Add(fs);
                }
                else
                {
                    newSegments.Add($"Fake_{fs}");
                }
            }
            return newSegments.ToArray();
        }

        private KeyValuePair<string, HashStorage> CreateStrategyHashSegment(bool init, params string[] segments)
        {
            int hash = DefaultHash;
            Type stringType = typeof(string);

            int idx = 0;
            PropertyInfo pi;
            int segmentHash = 0;

            object value;
            ICollection<string> lstSegments = new HashSet<string>();

            if (segments.IsEmptyArray())
            {
                if (init)
                {
                    segments = filterSegments.ToArray();
                }
                else
                {
                    for (idx = 1; idx <= filterProperties.Count(); idx++)
                    {
                        segmentHash = idx;
                        segmentHash += hash;
                        hash *= segmentHash;
                    }
                    return new KeyValuePair<string, HashStorage>(string.Empty, new HashStorage()
                    {
                        Hash = hash
                    });
                }
            }

            foreach (var segment in segments)
            {
                segmentHash = ++idx;
                pi = filterProperties.FirstOrDefault(p => p.Name.Equals(segment));
                if (pi == null)
                {
                    segmentHash += hash;
                    hash *= segmentHash;
                    continue;
                }
                value = Accesor[FilterRule, pi.Name];
                if (pi.PropertyType.Equals(stringType))
                {
                    if (StrategyFilterRule.ANY.Equals((string)value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        segmentHash += hash;
                        hash *= segmentHash;
                        continue;
                    }
                    segmentHash += CalculateHash((string)value);
                    hash *= segmentHash;
                }
                else
                {
                    hash += value.GetHashCode();
                }
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
            if (read == null) return Hash;
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
