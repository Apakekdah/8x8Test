namespace _8x8.Interfaces
{
    public interface IStrategyFeatureFilter<TStrategy> : IDataLoader
        where TStrategy : IStrategy
    {
    }
}
