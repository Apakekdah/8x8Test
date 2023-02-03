using System;
using System.Collections.Generic;
using System.Text;

namespace _8x8.Interfaces
{
    public interface IDataLoader
    {
        IEnumerable<IDictionary<string, string>> Load(string path);
    }
}
