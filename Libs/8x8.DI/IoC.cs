using Autofac;

namespace _8x8
{
    public static class IoC
    {
        public static ILifetimeScope Life { get; private set; }
        public static void SetLifetime(IContainer container)
        {
            Life = container;
        }
    }
}
