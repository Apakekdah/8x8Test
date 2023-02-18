using _8x8.Interfaces;
using System;

namespace _8x8.Impls
{
    public class FilterRule<TFilter1, TFilter2, TFilter3, TFilter4> : IFilterRule<TFilter1, TFilter2, TFilter3, TFilter4>
        where TFilter1 : notnull
        where TFilter2 : notnull
        where TFilter3 : notnull
        where TFilter4 : notnull
    {
        public FilterRule()
        { }

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

        public static implicit operator FilterRule<TFilter1, TFilter2, TFilter3, TFilter4>(object[] value)
        {
            return new FilterRule<TFilter1, TFilter2, TFilter3, TFilter4>()
            {
                Filter1 = (TFilter1)Convert.ChangeType(value[0], typeof(TFilter1)),
                Filter2 = (TFilter2)Convert.ChangeType(value[1], typeof(TFilter2)),
                Filter3 = (TFilter3)Convert.ChangeType(value[2], typeof(TFilter3)),
                Filter4 = (TFilter4)Convert.ChangeType(value[3], typeof(TFilter4)),
            };
        }
    }

    public class FilterRule<TFilter1, TFilter2, TFilter3> : IFilterRule<TFilter1, TFilter2, TFilter3>
        where TFilter1 : notnull
        where TFilter2 : notnull
        where TFilter3 : notnull
    {
        public FilterRule()
        { }

        public FilterRule(TFilter1 filter1, TFilter2 filter2, TFilter3 filter3)
        {
            Filter1 = filter1;
            Filter2 = filter2;
            Filter3 = filter3;
        }

        public TFilter1 Filter1 { get; set; }
        public TFilter2 Filter2 { get; set; }
        public TFilter3 Filter3 { get; set; }

        public static implicit operator FilterRule<TFilter1, TFilter2, TFilter3>(object[] value)
        {
            return new FilterRule<TFilter1, TFilter2, TFilter3>()
            {
                Filter1 = (TFilter1)Convert.ChangeType(value[0], typeof(TFilter1)),
                Filter2 = (TFilter2)Convert.ChangeType(value[1], typeof(TFilter2)),
                Filter3 = (TFilter3)Convert.ChangeType(value[2], typeof(TFilter3)),
            };
        }
    }
}