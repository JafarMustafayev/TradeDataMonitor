using System.IO;
using TradeMonitor.Models;

namespace TradeMonitor.Loaders
{
    public class CsvTradeLoader : ITradeLoader
    {
        public string FileExtension => ".csv";

        public IEnumerable<Trade> LoadTrades(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                string line;
                bool isFirstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var values = line.Split(',');
                    yield return new Trade
                    {
                        Date = DateTime.Parse(values[0]),
                        Open = decimal.Parse(values[1]),
                        High = decimal.Parse(values[2]),
                        Low = decimal.Parse(values[3]),
                        Close = decimal.Parse(values[4]),
                        Volume = int.Parse(values[5])
                    };
                }
            }
        }
    }
}