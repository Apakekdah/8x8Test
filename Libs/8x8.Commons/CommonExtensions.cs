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

        public static IEnumerable<T[]> Combinations<T>(this T[] source, int dimension)
        {
            int sizeOf = source.Count();
            T[] result = new T[dimension];
            Stack<int> stack = new Stack<int>();
            stack.Push(0);

            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();

                while (value < sizeOf)
                {
                    result[index++] = source[(++value) - 1];
                    stack.Push(value);

                    if (index == dimension)
                    {
                        yield return (T[])result.Clone();
                        break;
                    }
                }
            }
        }
    }
}
