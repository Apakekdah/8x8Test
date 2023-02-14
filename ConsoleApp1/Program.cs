using _8x8;
using _8x8.Impls;
using _8x8.Interfaces;
using _8x8.Models;
using Autofac;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Register();

            CreateFilterRaw();

            Version4();

            Version4Regex();

            Version3();

            Version4ManyFilter();
        }

        static void Register()
        {
            RegisterDI.Register();
        }

        static void Version4()
        {
            Console.WriteLine("Start rule 4 Hash");

            using (var scope = IoC.Life.BeginLifetimeScope())
            {
                Stopwatch sw = new Stopwatch();

                sw.Start();

                var sff4 = scope
                    .ResolveNamed<IStrategyFeatureFilter<StrategyRule4<string, string, string, string>>>(KeyDI.STRATEGY_FILTER_4,
                        new NamedParameter("method", KeyDI.HASH));

                sff4.Load(StrategyFilterRule.SAMPLE_DATA);

                sw.Stop();

                Console.WriteLine($"Load time Elapsed : {sw.Elapsed}");

                sw.Restart();

                var strategy4 = sff4.FindRule<int>(new FilterRule<string, string, string, string>("AAA", "BBB", "CCC", "DDD"));

                sw.Stop();

                Console.WriteLine($"Find Elapsed : {sw.Elapsed}");

                Console.WriteLine($"Found : \n{strategy4}");
            }
        }

        static void Version4Regex()
        {
            Console.WriteLine("Start rule 4 Regex");

            using (var scope = IoC.Life.BeginLifetimeScope())
            {
                Stopwatch sw = new Stopwatch();

                sw.Start();

                var sff4 = scope
                    .ResolveNamed<IStrategyFeatureFilter<StrategyRule4<string, string, string, string>>>(KeyDI.STRATEGY_FILTER_4,
                        new NamedParameter("method", KeyDI.REGEX));

                sff4.Load(StrategyFilterRule.SAMPLE_DATA);

                sw.Stop();

                Console.WriteLine($"Load time Elapsed : {sw.Elapsed}");

                sw.Restart();

                var strategy4 = sff4.FindRule<string>(new FilterRule<string, string, string, string>("AAA", "BBB", "CCC", "DDD"));

                sw.Stop();

                Console.WriteLine($"Find Elapsed : {sw.Elapsed}");

                Console.WriteLine($"Found : \n{strategy4}");
            }
        }

        static void Version3()
        {
            Console.WriteLine("Start rule 3 Hash");
            using (var scope = IoC.Life.BeginLifetimeScope())
            {
                Stopwatch sw = new Stopwatch();

                sw.Start();

                var sff3 = scope
                   .ResolveNamed<IStrategyFeatureFilter<StrategyRule3<string, string, string>>>(KeyDI.STRATEGY_FILTER_3,
                       new NamedParameter("method", KeyDI.HASH));

                sff3.Load(StrategyFilterRule.SAMPLE_DATA);

                sw.Stop();

                Console.WriteLine($"Load time Elapsed : {sw.Elapsed}");

                sw.Restart();

                var strategy3 = sff3.FindRule<int>(new FilterRule<string, string, string>("AAA", "BBB", "DDD"));

                sw.Stop();

                Console.WriteLine($"Find Elapsed : {sw.Elapsed}");

                Console.WriteLine($"Found : \n{strategy3}");
            }
        }

        static void Version4ManyFilter()
        {
            Console.WriteLine("Start rule 4 Hash Many");

            using (var scope = IoC.Life.BeginLifetimeScope())
            {
                Stopwatch sw = new Stopwatch();

                sw.Start();

                var sff4 = scope
                    .ResolveNamed<IStrategyFeatureFilter<StrategyRule4<string, string, string, string>>>(KeyDI.STRATEGY_FILTER_4,
                        new NamedParameter("method", KeyDI.HASH));

                sff4.Load(StrategyFilterRule.SAMPLE_DATA);

                sw.Stop();

                Console.WriteLine($"Load time Elapsed : {sw.Elapsed}");

                var fRules = CreateFilterRaw();

                Console.WriteLine("Begin seeks");
                ConcurrentBag<int> lst = new ConcurrentBag<int>();

                sw.Restart();

                fRules.
                    AsParallel()
                    .WithDegreeOfParallelism(1)
                    .ForAll(fr =>
                    {
                        var found = sff4.FindRule<int>(fr);
                        lst.Add(found.RuleId);
                    });

                sw.Stop();

                Console.WriteLine($"Find Elapsed : {sw.Elapsed}");

                int idx = 1;
                StringBuilder sb = new StringBuilder();
                foreach (var id in lst)
                {
                    sb.AppendLine($"{idx} \t\t{id}");
                    idx++;
                }
                Console.WriteLine($"Found : {sb}");

                lst.Clear();
            }
        }

        static IEnumerable<IFilterRule> CreateFilterRaw()
        {
            var typeT = typeof(FilterRule<string, string, string, string>);

            var reader = IoC.Life.Resolve<ICsvReader>();

            reader.Separator = ",";
            var raws = reader.Reader("DataFilters100.csv");

            FastMember.TypeAccessor accessor = FastMember.TypeAccessor.Create(typeT);
            PropertyInfo pi;

            ICollection<IFilterRule> rules = new HashSet<IFilterRule>();

            foreach (var dic in raws)
            {
                IFilterRule filter = (IFilterRule)Activator.CreateInstance(typeT);

                foreach (var kvp in dic)
                {
                    pi = typeT.GetProperty(kvp.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
                    if (pi != null)
                    {
                        if (!string.IsNullOrEmpty(kvp.Value))
                        {
                            accessor[filter, pi.Name] = Convert.ChangeType(kvp.Value, pi.PropertyType);
                        }
                    }
                }

                rules.Add(filter);

                dic.Clear();
            }

            return rules;
        }
    }
}
