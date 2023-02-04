using _8x8.Interfaces;

namespace _8x8.Models
{
    public class StrategyRule3<TFilter1, TFilter2, TFilter3> : BaseRule, IFilterRule<TFilter1, TFilter2, TFilter3>, IStrategy
    {
        public TFilter1 Filter1 { get; set; }
        public TFilter2 Filter2 { get; set; }
        public TFilter3 Filter3 { get; set; }
    }
}
