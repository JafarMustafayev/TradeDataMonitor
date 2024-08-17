using System.Globalization;

namespace TradeMonitor.Loaders
{
    public class TxtTradeLoader : ITradeLoader
    {
        public string FileExtension => ".txt";

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

                    var numberFormat = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = ".",
                        NumberGroupSeparator = ","
                    };

                    var values = line.Split(';');

                    yield return new Trade
                    {
                        Date = DateTime.Parse(values[0]),
                        Open = decimal.Parse(values[1], numberFormat),
                        High = decimal.Parse(values[2], numberFormat),
                        Low = decimal.Parse(values[3], numberFormat),
                        Close = decimal.Parse(values[4], numberFormat),
                        Volume = int.Parse(values[5])
                    };
                }
            }
        }
    }
}