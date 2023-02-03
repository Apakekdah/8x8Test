using _8x8.Impls;
using _8x8.Interfaces;
using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace _8x8
{
    public static class ParseExtensions
    {
        public static IEnumerable<TStrategy> ParseToStrategy<TStrategy>(this IEnumerable<IDictionary<string, string>> dataSetStrategy)
            where TStrategy : IStrategy, new()
        {
            var typeT = typeof(TStrategy);
            PropertyInfo pi;

            TStrategy strategy;
            Type nullType;

            TypeAccessor accessor = TypeAccessor.Create(typeof(TStrategy));
            foreach (var item in dataSetStrategy)
            {
                strategy = new TStrategy();
                foreach (var kvp in item)
                {
                    pi = typeT.GetProperty(kvp.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
                    if (pi != null)
                    {
                        if (!string.IsNullOrEmpty(kvp.Value))
                        {
                            if ((nullType = Nullable.GetUnderlyingType(pi.PropertyType)) == null)
                            {
                                nullType = pi.PropertyType;
                            }
                            accessor[strategy, pi.Name] = Convert.ChangeType(kvp.Value, nullType);
                        }
                    }
                }
                yield return strategy;
            }
        }

        public static IEnumerable<IStrategyWrapper> CreateRuleStrategy<TStrategy>(this IEnumerable<TStrategy> strategies)
            where TStrategy : IStrategy
        {
            foreach(var strategy in strategies)
            {
                yield return new HashStrategyWrapper(strategy);
            }
        }
    }
}
