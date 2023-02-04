using System.Collections.Generic;
using System.Linq;

namespace _8x8
{
    public static class CommonExtensions
    {
        public static bool IsEmptyArray<T>(this IEnumerable<T> array)
        {
            if (array == null) return true;
            else if (!array.Any()) return true;
            return false;
        }

    }
}
