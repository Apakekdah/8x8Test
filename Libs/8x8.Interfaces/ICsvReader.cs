using System.Collections.Generic;

namespace _8x8.Interfaces
{
    public interface ICsvReader : IFileReader<IEnumerable<IDictionary<string, string>>>
    {
        string Separator { get; set; }
    }
}
