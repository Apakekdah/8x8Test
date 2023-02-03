using _8x8;
using _8x8.HashRulesStrategy;
using _8x8.Impls;
using _8x8.Interfaces;
using _8x8.Models;
using Autofac;
using System;
using System.Diagnostics;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Register();

            Stopwatch sw = new Stopwatch();

            sw.Start();

            var sff = IoC
                .Life
                .ResolveNamed<IStrategyFeatureFilter<StrategyRule4<string, string, string, string>>>(KeyDI.STRATEGY_FILTER_4,
                    new NamedParameter("method", KeyDI.HASH));
            sff.Load("sample_rules.csv");

            sw.Stop();

            Console.WriteLine($"Load time Elapsed : {sw.Elapsed}");

            sw.Restart();

            var strategy = sff.FindRule(new FilterRule4<string, string, string, string>("AAA", "BBB", "CCC", "AAA"));

            sw.Stop();

            Console.WriteLine($"Find Elapsed : {sw.Elapsed}");

            Console.WriteLine($"Found : \n{strategy}");
            //IDataLoader loader = IoC.Life.ResolveNamed<IDataLoader>(KeyDI.DATA_LOADER_CSV, new NamedParameter("separator", ","));
            //var dataset = loader.Load<StrategyRule4<string, string, string, string>, IStrategyWrapper>("SampleData.csv");

            //foreach (var stg in dataset)
            //{
            //    Console.WriteLine(stg);
            //}

            //IEngineStrategy engineStrategy = new EngineStrategy(dataset);
            //engineStrategy.FindRule(CommonExtensions.ParseToFilterRule("AAA", "BBB", "CCC", "DDD"));
        }

        static void Register()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //builder.RegisterType<CsvLoader>().Named<IDataLoader>(KeyDI.DATA_LOADER_CSV).InstancePerLifetimeScope();
            //builder.RegisterGeneric(typeof(CsvLoader<StrategyRule4<string, string, string, string>,>)).Name.SingleInstance();

            builder.RegisterType<StrategyFeatureFilter<StrategyRule4<string, string, string, string>>>()
                .Named<IStrategyFeatureFilter<StrategyRule4<string, string, string, string>>>(KeyDI.STRATEGY_FILTER_4)
                .SingleInstance();

            builder.RegisterType<StrategyStorage<IStrategyWrapper>>().As<IStrategyStorage<IStrategyWrapper>>().SingleInstance();

            builder.RegisterType<StrategyRule4<string, string, string, string>>().AsImplementedInterfaces().InstancePerLifetimeScope();

            /// Hash
            builder.RegisterType<HashStrategyWrapper>().Named<IStrategyWrapper>(KeyDI.HASH).InstancePerDependency();
            builder.RegisterType<HashFilterRuleStrategy>().Named<IFilterRuleStrategy>(KeyDI.HASH);

            IoC.SetLifetime(builder.Build());
        }
    }
}
