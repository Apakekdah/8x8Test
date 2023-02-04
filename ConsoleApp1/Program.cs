using _8x8;
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

            //var sff = IoC
            //   .Life
            //   .ResolveNamed<IStrategyFeatureFilter<StrategyRule3<string, string, string>>>(KeyDI.STRATEGY_FILTER_3,
            //       new NamedParameter("method", KeyDI.HASH));

            sff.Load("SampleData.csv");

            sw.Stop();

            Console.WriteLine($"Load time Elapsed : {sw.Elapsed}");

            sw.Restart();

            var strategy = sff.FindRule(new FilterRule<string, string, string, string>("AAA", "BBB", "CCC", "DDD"));
            //var strategy = sff.FindRule(new FilterRule<string, string, string>("AAA", "BBB", "DDD"));

            sw.Stop();

            Console.WriteLine($"Find Elapsed : {sw.Elapsed}");

            Console.WriteLine($"Found : \n{strategy}");
        }

        static void Register()
        {
            RegisterDI.Register();
        }
    }
}
