using TradeMonitor.Models;

namespace TradeMonitor.Loaders
{
    public interface ITradeLoader
    {
        string FileExtension { get; }

        IEnumerable<Trade> LoadTrades(string filePath);
    }
}