﻿using System;

namespace _8x8.Interfaces
{
    public interface IStrategyWrapper : IDisposable
    {
        int RuleId { get; }
        int Priority { get; }
        IStrategy Strategy { get; }
    }

    public interface IStrategyWrapper<T> : IStrategyWrapper
    {
        IFilterRuleStrategy<T> FilterRuleStrategy { get; }
    }
}
