namespace TradeMonitor.Loaders
{
    public class XmlTradeLoader : ITradeLoader
    {
        public string FileExtension => ".xml";

        public IEnumerable<Trade> LoadTrades(string filePath)
        {
            var doc = XDocument.Load(filePath);
            foreach (var element in doc.Root.Elements("value"))
            {
                yield return new Trade
                {
                    Date = DateTime.Parse(element.Attribute("date").Value),
                    Open = decimal.Parse(element.Attribute("open").Value),
                    High = decimal.Parse(element.Attribute("high").Value),
                    Low = decimal.Parse(element.Attribute("low").Value),
                    Close = decimal.Parse(element.Attribute("close").Value),
                    Volume = int.Parse(element.Attribute("volume").Value)
                };
            }
        }
    }
}