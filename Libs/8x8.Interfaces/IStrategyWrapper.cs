using System;
using System.Collections.Generic;
using System.Text;

namespace _8x8.Interfaces
{
    public interface IStrategyWrapper : IFilterInfo, IComparable<IStrategyWrapper>
    {
        int Priority { get; }
        IStrategy Strategy { get; }
    }
}
