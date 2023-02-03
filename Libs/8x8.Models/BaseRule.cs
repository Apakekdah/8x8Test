using _8x8.Interfaces;
using System;

namespace _8x8.Models
{
    public class BaseRule : IBaseRule, IStrategy
    {
        public int RuleId { get;set; }
        public int Priority { get;set; }
        public int? OutputValue { get; set; }
    }
}