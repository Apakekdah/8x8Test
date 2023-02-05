using _8x8.HashRulesStrategy;
using _8x8.Impls;
using _8x8.Interfaces;
using _8x8.Models;
using _8x8.RegexRulesStrategy;
using Autofac;

namespace _8x8
{
    public class RegisterDI
    {
        public static void Register()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<StrategyFeatureFilter<StrategyRule4<string, string, string, string>>>()
                .Named<IStrategyFeatureFilter<StrategyRule4<string, string, string, string>>>(KeyDI.STRATEGY_FILTER_4)
                .SingleInstance();

            builder.RegisterType<StrategyFeatureFilter<StrategyRule3<string, string, string>>>()
                .Named<IStrategyFeatureFilter<StrategyRule3<string, string, string>>>(KeyDI.STRATEGY_FILTER_3)
                .SingleInstance();

            builder.RegisterType<MicrosoftCsvReader>().As<ICsvReader>();

            builder.RegisterType<StrategyStorage<IStrategyWrapper>>().As<IStrategyStorage<IStrategyWrapper>>().SingleInstance();

            builder.RegisterType<StrategyRule4<string, string, string, string>>().AsImplementedInterfaces().InstancePerLifetimeScope();

            /// Hash
            builder.RegisterType<HashStrategyWrapper>().Named<IStrategyWrapper<int>>(KeyDI.HASH);
            builder.RegisterType<HashStrategyWrapper>().Named<IStrategyWrapper>(KeyDI.HASH);
            builder.RegisterType<HashFilterRuleStrategy>().Named<IFilterRuleStrategy<int>>(KeyDI.HASH);
            builder.RegisterType<HashFilterRuleStrategy>().Named<IFilterRuleStrategy>(KeyDI.HASH);

            builder.RegisterType<RegexStrategyWrapper>().Named<IStrategyWrapper<string>>(KeyDI.REGEX);
            builder.RegisterType<RegexStrategyWrapper>().Named<IStrategyWrapper>(KeyDI.REGEX);
            builder.RegisterType<RegexFilterRuleStrategy>().Named<IFilterRuleStrategy<string>>(KeyDI.REGEX);
            builder.RegisterType<RegexFilterRuleStrategy>().Named<IFilterRuleStrategy>(KeyDI.REGEX);

            IoC.SetLifetime(builder.Build());
        }
    }
}
