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

            Version4();

            Version4Regex();

            Version3();
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
    }
}
