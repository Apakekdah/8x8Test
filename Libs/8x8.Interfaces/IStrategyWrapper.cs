namespace _8x8.Interfaces
{
    public interface IStrategyWrapper : IFilterInfo
    {
        int RuleId { get; }
        int Priority { get; }
        IStrategy Strategy { get; }
    }
}
