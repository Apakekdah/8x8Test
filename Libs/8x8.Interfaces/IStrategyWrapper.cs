using System;

namespace _8x8.Interfaces
{
    public interface IStrategyWrapper : IDisposable, IFilterInfo
    {
        int RuleId { get; }
        int Priority { get; }
        IStrategy Strategy { get; }
    }
}
