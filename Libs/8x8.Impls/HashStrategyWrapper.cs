using _8x8.Interfaces;
using FastMember;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace _8x8.Impls
{
    public class HashStrategyWrapper : IStrategyWrapper
    {
        public static readonly string ANY = "<ANY>";

        private readonly IStrategy strategy;

        public HashStrategyWrapper(IStrategy strategy)
        {
            this.strategy = strategy;
            Init();
        }

        public IStrategy Strategy => strategy;

        public IEnumerable<string> Segments { get; private set; }

        public int Hash {get; private set; }

        public int Priority { get; private set; }

        public int CompareTo([AllowNull] IStrategyWrapper other)
        {
            if(other == null) throw new ArgumentNullException("other");
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
            IFilterRule filterRule = (IFilterRule)strategy;

            Priority = rule.Priority;

            var result = CreateHash(filterRule);

            Segments = result.Item1;
            Hash = result.Item2;
        }

        private IEnumerable<PropertyInfo> GetInheritancePropertiesFromIFilterRule(IFilterRule filterRule)
        {
            Type typeFilterRule = typeof(IFilterRule);
            var types = filterRule.GetType().GetInterfaces().Where(c => typeFilterRule.IsAssignableFrom(c) && !typeFilterRule.Equals(c)).ToArray();
            foreach (var type in types)
            {
                foreach(var pi in type.GetProperties())
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
            foreach(var filter in filters)
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

            return (segments.ToArray() ,hash);
        }

        #endregion
    }
}
