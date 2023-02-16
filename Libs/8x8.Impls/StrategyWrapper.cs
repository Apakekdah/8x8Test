using _8x8.Interfaces;
using Autofac;

namespace _8x8.Impls
{
    public class BaseStrategyWrapper<T> : Disposable, IStrategyWrapper<T>
    {
        public BaseStrategyWrapper(ILifetimeScope life, IStrategy strategy)
        {
            Life = life;
            Strategy = strategy;
            FilterRuleStrategy = life.ResolveNamed<IFilterRuleStrategy<T>>(life.Tag.ToString(), new NamedParameter("filterRule", strategy));
            Init();
        }

        public IFilterRuleStrategy<T> FilterRuleStrategy { get; private set; }

        public int RuleId { get; private set; }

        public int Priority { get; private set; }

        public IStrategy Strategy { get; private set; }

        protected override void DisposeCore()
        {
            base.DisposeCore();
            FilterRuleStrategy.Dispose();
        }

        #region Protected

        protected ILifetimeScope Life { get; private set; }

        protected virtual void Init()
        {
            IBaseRule rule = (IBaseRule)Strategy;
            Priority = rule.Priority;
            RuleId = rule.RuleId;
        }

        #endregion
    }
}
