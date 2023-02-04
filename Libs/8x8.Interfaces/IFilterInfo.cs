using System.Collections.Generic;

namespace _8x8.Interfaces
{
    public interface IFilterInfo<T>
    {
        IEnumerable<string> Segments { get; }
        T Hash { get; }
    }
}