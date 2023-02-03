using _8x8.Interfaces;
using Autofac;
using FastMember;
using System;
using System.Linq;
using System.Reflection;

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

        public TStrategy FindRule(IFilterRule filterRule)
        {
            var filterRuleStrategy = scope.ResolveNamed<IFilterRuleStrategy>(scope.Tag.ToString(),
                new NamedParameter("filterRule", filterRule));

            var founds = storage.Find(filterRuleStrategy);
            if (founds.Any())
            {
                founds = founds.OrderByDescending(s => s.Priority);
            }
            return (TStrategy)founds.FirstOrDefault()?.Strategy;
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
            Microsoft.VisualBasic.FileIO.TextFieldParser tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(path)
            {
                TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited,
                HasFieldsEnclosedInQuotes = true,
                TrimWhiteSpace = true,
            };
            tfp.SetDelimiters(separator);

            var header = tfp.ReadFields();

            while (!tfp.EndOfData)
            {
                var rows = tfp.ReadFields();
                storage.Add(ParseToStrategyWrapper(header, rows));
            }
        }

        private IStrategyWrapper ParseToStrategyWrapper(string[] header, string[] rawData)
        {
            var typeT = typeof(TStrategy);
            PropertyInfo pi;

            Type nullType;

            TypeAccessor accessor = TypeAccessor.Create(typeof(TStrategy));
            TStrategy strategy = new TStrategy();
            for (int n = 0; n < header.Length; n++)
            {
                pi = typeT.GetProperty(header[n], BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
                if (pi != null)
                {
                    if (!string.IsNullOrEmpty(rawData[n]))
                    {
                        if ((nullType = Nullable.GetUnderlyingType(pi.PropertyType)) == null)
                        {
                            nullType = pi.PropertyType;
                        }
                        accessor[strategy, pi.Name] = Convert.ChangeType(rawData[n], nullType);
                    }
                }
            }

            return scope.ResolveNamed<IStrategyWrapper>(scope.Tag.ToString(), new NamedParameter("strategy", strategy));
        }

        #endregion
    }
}
