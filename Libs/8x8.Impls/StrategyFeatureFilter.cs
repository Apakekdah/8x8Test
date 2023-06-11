using _8x8.Interfaces;
using Autofac;
using FastMember;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace _8x8.Impls
{
    public class StrategyFeatureFilter<TStrategy> : IStrategyFeatureFilter<TStrategy>
        where TStrategy : IStrategy, new()
    {
        private readonly ILifetimeScope scope;
        private readonly IStrategyStorage<IStrategyWrapper> storage;

        public StrategyFeatureFilter(ILifetimeScope life, IStrategyStorage<IStrategyWrapper> storage, string method)
        {
            this.scope = life.BeginLifetimeScope(method);
            this.storage = storage;
        }

        public TStrategy FindRule<T>(IFilterRule filterRule)
        {
            using (var filterRuleStrategy = scope.ResolveNamed<IFilterRuleStrategy>(scope.Tag.ToString(),
                new NamedParameter("filterRule", filterRule)))
            {
                filterRuleStrategy.CreateAllCombination();
                var founds = storage.Find<T>(filterRuleStrategy);
                if (founds.Any())
                {
                    founds = founds.OrderByDescending(s => s.Priority);
                }
                return (TStrategy)founds.FirstOrDefault()?.Strategy;
            }
        }

        public void Load(string path)
        {
            Load(path, ",");
        }

        public void Load(string path, string separator)
        {
            LoadCsv(path, separator);
        }

        #region Private

        private void LoadCsv(string path, string separator)
        {
            ICsvReader reader = scope.Resolve<ICsvReader>();

            reader.Separator = separator;
            var dicRows = reader.Reader(path);

            TypeAccessor accessor = TypeAccessor.Create(typeof(TStrategy));

            ConcurrentBag<IStrategyWrapper> bags = new ConcurrentBag<IStrategyWrapper>();

            dicRows.AsParallel().WithDegreeOfParallelism(3)
                .ForAll(d =>
                {
                    bags.Add(ParseToStrategyWrapper(accessor, d));
                    d.Clear();
                });

            storage.AddRange(bags);
            bags.Clear();
        }

        private IStrategyWrapper ParseToStrategyWrapper(TypeAccessor accessor, IDictionary<string, string> row)
        {
            var typeT = typeof(TStrategy);
            //PropertyInfo pi;

            Type nullType;

            TStrategy strategy = new TStrategy();
            var members = accessor.GetMembers();
            foreach (var kvp in row)
            {
                var member = members.First(m => m.Name.Equals(kvp.Key));
                //pi = typeT.GetProperty(kvp.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
                if (member != null)
                {
                    if (!string.IsNullOrEmpty(kvp.Value))
                    {
                        if ((nullType = Nullable.GetUnderlyingType(member.Type)) == null)
                        {
                            nullType = member.Type;
                        }
                        accessor[strategy, member.Name] = Convert.ChangeType(kvp.Value, nullType);
                    }
                }
            }

            return scope.ResolveNamed<IStrategyWrapper>(scope.Tag.ToString(), new NamedParameter("strategy", strategy));
        }

        #endregion
    }
}
