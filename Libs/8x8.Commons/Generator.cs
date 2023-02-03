using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8x8.Commons
{
    public static class Generator
    {
        public static readonly string ANY = "<ANY>";

        public static int CreateHash(params object[] filter)
        {
            int hash = 0;
            List<int> segments = new List<int>();
            Type type;

            for(int n = 0; n < filter.Length; n++)
            {
                if (filter[n] == null)
                {
                    continue;
                }

                if((type = Nullable.GetUnderlyingType(filter[n].GetType())) == null)
                {
                    type = filter[n].GetType();
                }

                if (type.Equals(typeof(string)))
                {
                    if (ANY.Equals((string)filter[n], StringComparison.OrdinalIgnoreCase))
                        continue;

                    hash += ((string)filter[n]).Select(x => ((int)x) * (n + 1)).Sum();
                }
                else
                {
                    hash += filter[n].GetHashCode();
                }
            }

            return hash;
        }
    }
}
