using _8x8.Interfaces;
using System.Collections.Generic;

namespace _8x8.Impls
{
    public class MicrosoftCsvReader : ICsvReader
    {
        public static readonly string DefaultSeparator = ",";

        public string Separator { get; set; } = DefaultSeparator;

        public IEnumerable<IDictionary<string, string>> Reader(string path)
        {
            Microsoft.VisualBasic.FileIO.TextFieldParser tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(path)
            {
                TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited,
                HasFieldsEnclosedInQuotes = true,
                TrimWhiteSpace = true,
            };
            tfp.SetDelimiters(Separator);

            var headers = tfp.ReadFields();

            while (!tfp.EndOfData)
            {
                var rows = tfp.ReadFields();
                IDictionary<string, string> dic = new Dictionary<string, string>();
                for (int n = 0; n < headers.Length; n++)
                {
                    dic.Add(headers[n], rows[n]);
                }
                yield return dic;
            }
        }
    }
}
