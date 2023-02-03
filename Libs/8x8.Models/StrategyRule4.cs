using _8x8.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace _8x8.Models
{
    public class StrategyRule4<TFilter1, TFilter2, TFilter3, TFilter4> : BaseRule, IFilterRule<TFilter1, TFilter2, TFilter3, TFilter4>, IStrategy
    {
        public TFilter1 Filter1 { get; set; }
        public TFilter2 Filter2 { get; set; }
        public TFilter3 Filter3 { get; set; }
        public TFilter4 Filter4 { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            IDictionary<string, string> dic = new Dictionary<string, string>();

            var header = props.Select(p => p.Name).OrderBy(c => c).ToArray();

            object val;
            foreach (var hdr in header)
            {
                var pi = GetType().GetProperty(hdr);
                val = pi.GetValue(this);
                if (val == null)
                {
                    dic.Add(hdr, "<null>");
                }
                else
                {
                    dic.Add(hdr, val.ToString());
                }
            }
            foreach (var kvp in dic)
            {
                builder.AppendLine($"{kvp.Key} = {kvp.Value}");
            }

            dic.Clear();

            return builder.ToString();
        }
    }
}
