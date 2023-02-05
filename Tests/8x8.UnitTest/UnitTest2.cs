using _8x8.Impls;
using _8x8.Interfaces;
using _8x8.Models;
using Autofac;
using NUnit.Framework;

namespace _8x8.UnitTest
{
    public class TestFilterRulesRegex
    {
        private IStrategyFeatureFilter<StrategyRule4<string, string, string, string>> engine;

        [SetUp]
        public void Setup()
        {
            RegisterDI.Register();

            engine = IoC
                .Life
                .ResolveNamed<IStrategyFeatureFilter<StrategyRule4<string, string, string, string>>>(KeyDI.STRATEGY_FILTER_4,
                    new NamedParameter("method", KeyDI.REGEX));

            engine.Load("SampleData.csv");
        }

        [Test]
        public void TestHashFilter1()
        {
            var frule = new FilterRule<string, string, string, string>("AAA", "BBB", "CCC", "AAA");

            IBaseRule rule = engine.FindRule<string>(frule);

            Assert.AreEqual(rule.RuleId, 4);
            Assert.AreEqual(rule.OutputValue, 10);
        }

        [Test]
        public void TestHashFilter2()
        {
            var frule = new FilterRule<string, string, string, string>("AAA", "BBB", "CCC", "DDD");

            IBaseRule rule = engine.FindRule<string>(frule);

            Assert.AreEqual(rule.RuleId, 4);
            Assert.AreEqual(rule.OutputValue, 10);
        }

        [Test]
        public void TestHashFilter3()
        {
            var frule = new FilterRule<string, string, string, string>("AAA", "AAA", "AAA", "AAA");

            IBaseRule rule = engine.FindRule<string>(frule);

            Assert.AreEqual(rule.RuleId, 2);
            Assert.AreEqual(rule.OutputValue, 1);
        }

        [Test]
        public void TestHashFilter4()
        {
            var frule = new FilterRule<string, string, string, string>("BBB", "BBB", "BBB", "BBB");

            IBaseRule rule = engine.FindRule<string>(frule);

            Assert.AreEqual(rule.RuleId, 6);
            Assert.AreEqual(rule.OutputValue, 0);
        }

        [Test]
        public void TestHashFilter5()
        {
            var frule = new FilterRule<string, string, string, string>("BBB", "CCC", "CCC", "CCC");

            IBaseRule rule = engine.FindRule<string>(frule);

            Assert.AreEqual(rule.RuleId, 3);
            Assert.AreEqual(rule.OutputValue, 7);
        }
    }
}