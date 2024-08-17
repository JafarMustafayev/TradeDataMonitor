namespace TradeMonitor.Services
{
    public class FileMonitoringService
    {
        private readonly AppConfig _config;
        private readonly Dictionary<string, ITradeLoader> _loaders;
        private FileSystemWatcher _watcher;

        public event EventHandler<IEnumerable<Trade>> NewTradesLoaded;

        public FileMonitoringService(AppConfig config, IEnumerable<ITradeLoader> loaders)
        {
            _config = config;
            _loaders = loaders.ToDictionary(l => l.FileExtension, l => l);
            InitializeFileWatcher();
        }

        private void InitializeFileWatcher()
        {
            _watcher = new FileSystemWatcher(_config.InputDirectory)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                Filter = "*.*"
            };
            _watcher.Created += OnFileCreated;
            _watcher.EnableRaisingEvents = true;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            var extension = Path.GetExtension(e.FullPath).ToLower();
            if (_loaders.TryGetValue(extension, out var loader))
            {
                Task.Run(() =>
                {
                    try
                    {
                        var trades = loader.LoadTrades(e.FullPath);
                        NewTradesLoaded?.Invoke(this, trades);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading file {e.FullPath}: {ex.Message}");
                    }
                });
            }
        }

        public void UpdateMonitoringFrequency(int seconds)
        {
            _config.MonitoringFrequencySeconds = seconds;
            _config.Save("appsettings.json");
        }

        public void UpdateInputDirectory(string path)
        {
            _config.InputDirectory = path;
            _config.Save("appsettings.json");
            _watcher.Path = path;
        }

        public string GetInputDirectory() => _config.InputDirectory;

        public int GetRefreshFrequency() => _config.MonitoringFrequencySeconds;
    }
}