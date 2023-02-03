namespace _8x8.Impls
{
    //public class CsvLoaderX
    //{
    //    private readonly string separator;

    //    public CsvLoader(string separator)
    //    {
    //        this.separator = separator;
    //    }

    //    public IEnumerable<IDictionary<string, string>> Load(string path)
    //    {
    //        Microsoft.VisualBasic.FileIO.TextFieldParser tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(path)
    //        {
    //            TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited,
    //            HasFieldsEnclosedInQuotes = true,
    //            TrimWhiteSpace = true,
    //        };
    //        tfp.SetDelimiters(separator);

    //        var headers = tfp.ReadFields();

    //        while (!tfp.EndOfData)
    //        {
    //            var rows = tfp.ReadFields();
    //            IDictionary<string, string> dic = new Dictionary<string, string>();
    //            for (int n = 0; n < headers.Length; n++)
    //            {
    //                dic.Add(headers[n], rows[n]);
    //            }
    //            yield return dic;
    //        }
    //    }

    //    private IEnumerable<IDictionary<string, string>> LoadCsv(string path)
    //    {
    //        Microsoft.VisualBasic.FileIO.TextFieldParser tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(path)
    //        {
    //            TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited,
    //            HasFieldsEnclosedInQuotes = true,
    //            TrimWhiteSpace = true,
    //        };
    //        tfp.SetDelimiters(separator);

    //        var headers = tfp.ReadFields();

    //        while (!tfp.EndOfData)
    //        {
    //            var rows = tfp.ReadFields();
    //            IDictionary<string, string> dic = new Dictionary<string, string>();
    //            for (int n = 0; n < headers.Length; n++)
    //            {
    //                dic.Add(headers[n], rows[n]);
    //            }
    //            yield return dic;
    //        }
    //    }

    //    public IEnumerable<TStrategy> ParseToStrategy<TStrategy>(IEnumerable<IDictionary<string, string>> dataSetStrategy)
    //        where TStrategy : IStrategy, new()
    //    {
    //        var typeT = typeof(TStrategy);
    //        PropertyInfo pi;

    //        TStrategy strategy;
    //        Type nullType;

    //        TypeAccessor accessor = TypeAccessor.Create(typeof(TStrategy));
    //        foreach (var item in dataSetStrategy)
    //        {
    //            strategy = new TStrategy();
    //            foreach (var kvp in item)
    //            {
    //                pi = typeT.GetProperty(kvp.Key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
    //                if (pi != null)
    //                {
    //                    if (!string.IsNullOrEmpty(kvp.Value))
    //                    {
    //                        if ((nullType = Nullable.GetUnderlyingType(pi.PropertyType)) == null)
    //                        {
    //                            nullType = pi.PropertyType;
    //                        }
    //                        accessor[strategy, pi.Name] = Convert.ChangeType(kvp.Value, nullType);
    //                    }
    //                }
    //            }
    //            yield return strategy;
    //        }
    //    }

    //    public IEnumerable<IStrategyWrapper> CreateRuleStrategy<TStrategy>(IEnumerable<TStrategy> strategies)
    //        where TStrategy : IStrategy
    //    {
    //        foreach (var strategy in strategies)
    //        {
    //            yield return new HashStrategyWrapper(strategy);
    //        }
    //    }

    //    public IEnumerable<IStrategyWrapper> Load(string path)
    //    {
    //        var rawData = LoadCsv(path);

    //        var strategies = ParseToStrategy()

    //        rawData.AsParallel().WithDegreeOfParallelism(3).ForAll(d => d.Clear());
    //    }
    //}
}
