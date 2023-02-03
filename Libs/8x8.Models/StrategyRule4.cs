using _8x8.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace _8x8.Models
{
    public class StrategyRule4<TFilter1, TFilter2, TFilter3, TFilter4> : BaseRule, IFilterRule<TFilter1, TFilter2, TFilter3, TFilter4>, IStrategy
    {
        public TFilter1 Filter1 { get; set; }
        public TFilter2 Filter2 { get;set; }
        public TFilter3 Filter3 { get;set; }    
        public TFilter4 Filter4 { get;set; }
    }
}
