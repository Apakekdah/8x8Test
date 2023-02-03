using _8x8.Impls;
using _8x8.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace _8x8
{
    public static class CommonExtensions
    {
        public static IFilterRule<TFilter1, TFilter2, TFilter3, TFilter4> ParseToFilterRule<TFilter1, TFilter2, TFilter3, TFilter4>(TFilter1 filter1, TFilter2 filter2, TFilter3 filter3, TFilter4 filter4)
        {
            return new FilterRule4<TFilter1, TFilter2, TFilter3, TFilter4>(filter1, filter2, filter3, filter4);
        }
    }
}
