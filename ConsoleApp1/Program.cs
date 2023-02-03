using _8x8;
using _8x8.Impls;
using _8x8.Interfaces;
using _8x8.Models;
using System;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IDataLoader loader = new CsvLoader(",");
            var dataset = loader.Load("SampleData.csv")
                            .ParseToStrategy<StrategyRule4<string, string, string, string>>()
                            .CreateRuleStrategy();

            foreach(var stg in dataset)
            {
                Console.WriteLine(stg);
            }

            IEngineStrategy engineStrategy = new EngineStrategy();
            engineStrategy.FindRule(CommonExtensions.ParseToFilterRule("AAA", "BBB", "CCC", "DDD"));
        }
    }
}
