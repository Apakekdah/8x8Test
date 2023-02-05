using _8x8.Impls;
using _8x8.Interfaces;
using Autofac;

namespace _8x8.RegexRulesStrategy
{
    public class RegexStrategyWrapper : BaseStrategyWrapper<string>
    {
        public RegexStrategyWrapper(ILifetimeScope life, IStrategy strategy)
            : base(life, strategy)
        {
        }
    }
}