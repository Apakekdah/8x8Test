using _8x8.Impls;
using _8x8.Interfaces;
using Autofac;

namespace _8x8.HashRulesStrategy
{
    public class HashStrategyWrapper : BaseStrategyWrapper<int>
    {
        public HashStrategyWrapper(ILifetimeScope life, IStrategy strategy)
            : base(life, strategy)
        { }
    }
}