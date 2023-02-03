using _8x8.Interfaces;
using System;
using System.Collections.Generic;

namespace _8x8.Impls
{
    public class CsvLoader : IDataLoader
    {
        private readonly string separator;

        public CsvLoader(string separator)
        {
            this.separator = separator;
        }

        public IEnumerable<IDictionary<string, string>> Load(string path)
        {
            Microsoft.VisualBasic.FileIO.TextFieldParser tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(path)
            {
                TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited,
                HasFieldsEnclosedInQuotes = true,
                TrimWhiteSpace = true,
            };
            tfp.SetDelimiters(separator);

            var headers = tfp.ReadFields();
            
            while(!tfp.EndOfData)
            {
                var rows = tfp.ReadFields();
                IDictionary<string, string> dic = new Dictionary<string, string>();
                for(int n = 0; n < headers.Length;n++)
                {
                    dic.Add(headers[n], rows[n]);
                }
                yield return dic;
            }
        }
    }
}
