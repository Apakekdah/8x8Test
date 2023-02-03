using System;
using System.Collections.Generic;
using System.Text;

namespace _8x8.Interfaces
{
    public interface IBaseRule
    {
        int RuleId { get; set; }
        int Priority { get; set; }
        int? OutputValue { get; set; }
    }
}
