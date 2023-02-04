using _8x8.HashRulesStrategy;
using _8x8.Impls;
using _8x8.Interfaces;
using _8x8.Models;
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

            IoC.SetLifetime(builder.Build());
        }
    }
}
