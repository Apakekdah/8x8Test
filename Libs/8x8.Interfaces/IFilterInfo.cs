using System;
using System.Collections.Generic;
using System.Text;

namespace _8x8.Interfaces
{
    public interface IFilterInfo
    {
        IEnumerable<string> Segments { get; }
        int Hash { get; }
    }
}