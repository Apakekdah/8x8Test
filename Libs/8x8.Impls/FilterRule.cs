using _8x8.Interfaces;

namespace _8x8.Impls
{
    public class FilterRule<TFilter1, TFilter2, TFilter3, TFilter4> : IFilterRule<TFilter1, TFilter2, TFilter3, TFilter4>
    {
        public FilterRule(TFilter1 filter1, TFilter2 filter2, TFilter3 filter3, TFilter4 filter4)
        {
            Filter1 = filter1;
            Filter2 = filter2;
            Filter3 = filter3;
            Filter4 = filter4;
        }

        public TFilter1 Filter1 { get; set; }
        public TFilter2 Filter2 { get; set; }
        public TFilter3 Filter3 { get; set; }
        public TFilter4 Filter4 { get; set; }
    }

    public class FilterRule<TFilter1, TFilter2, TFilter3> : IFilterRule<TFilter1, TFilter2, TFilter3>
    {
        public FilterRule(TFilter1 filter1, TFilter2 filter2, TFilter3 filter3)
        {
            Filter1 = filter1;
            Filter2 = filter2;
            Filter3 = filter3;
        }

        public TFilter1 Filter1 { get; set; }
        public TFilter2 Filter2 { get; set; }
        public TFilter3 Filter3 { get; set; }
    }
}