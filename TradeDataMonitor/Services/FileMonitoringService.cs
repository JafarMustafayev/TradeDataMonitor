using System.Timers;

public class FileMonitoringService
{
    private readonly AppConfig _config;
    private readonly Dictionary<string, ITradeLoader> _loaders;
    private System.Timers.Timer _timer;
    private Dictionary<string, DateTime> _fileLastWriteTimes;
    private FileSystemWatcher _watcher;

    public event EventHandler<IEnumerable<Trade>> NewTradesLoaded;

    public FileMonitoringService(AppConfig config, IEnumerable<ITradeLoader> loaders)
    {
        _config = config;
        _loaders = loaders.ToDictionary(l => l.FileExtension, l => l);
        _fileLastWriteTimes = new Dictionary<string, DateTime>();
        InitializeFileWatcher();
        InitializeTimer();
    }

    private void InitializeFileWatcher()
    {
        _watcher = new FileSystemWatcher(_config.InputDirectory)
        {
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            Filter = "*.*"
        };
        _watcher.EnableRaisingEvents = false;
    }

    private void InitializeTimer()
    {
        _timer = new System.Timers.Timer(_config.MonitoringFrequencySeconds * 1000);
        _timer.Elapsed += OnTimedEvent;
        _timer.AutoReset = true;
        _timer.Enabled = true;
    }

    private void OnTimedEvent(object sender, ElapsedEventArgs e)
    {
        CheckForChanges();
    }

    private void CheckForChanges()
    {
        var directoryInfo = new DirectoryInfo(_config.InputDirectory);
        foreach (var file in directoryInfo.GetFiles())
        {
            var lastWriteTime = file.LastWriteTime;
            if (!_fileLastWriteTimes.TryGetValue(file.FullName, out var storedLastWriteTime) ||
                storedLastWriteTime != lastWriteTime)
            {
                _fileLastWriteTimes[file.FullName] = lastWriteTime;
                OnFileCreated(file.FullName);
            }
        }
    }

    private void OnFileCreated(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLower();
        if (_loaders.TryGetValue(extension, out var loader))
        {
            Task.Run(() =>
            {
                try
                {
                    var trades = loader.LoadTrades(filePath);
                    NewTradesLoaded?.Invoke(this, trades);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading file {filePath}: {ex.Message}");
                }
            });
        }
    }

    public void UpdateMonitoringFrequency(int seconds)
    {
        _config.MonitoringFrequencySeconds = seconds;
        _config.Save("appsettings.json");
        _timer.Interval = seconds * 1000;
    }

    public void UpdateInputDirectory(string path)
    {
        _config.InputDirectory = path;
        _config.Save("appsettings.json");
        _watcher.Path = path;
        _fileLastWriteTimes.Clear();
    }

    public string GetInputDirectory() => _config.InputDirectory;

    public int GetRefreshFrequency() => _config.MonitoringFrequencySeconds;
}