namespace _8x8.Interfaces
{
    public interface IStrategyWrapper : IFilterInfo
    {
        int Priority { get; }
        IStrategy Strategy { get; }
    }
}
