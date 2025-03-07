﻿namespace TradeMonitor.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly FileMonitoringService _monitoringService;
        private ObservableCollection<Trade> _trades;
        private string _inputDirectory;
        private int _refreshFrequency;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Trade> Trades
        {
            get => _trades;
            set
            {
                _trades = value;
                OnPropertyChanged();
            }
        }

        public string InputDirectory
        {
            get => _inputDirectory;
            set
            {
                if (_inputDirectory != value)
                {
                    _inputDirectory = value;
                    OnPropertyChanged();
                }
            }
        }

        public int RefreshFrequency
        {
            get => _refreshFrequency;
            set
            {
                if (_refreshFrequency != value)
                {
                    _refreshFrequency = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ApplyRefreshFrequencyCommand { get; }
        public ICommand ApplyInputDirectoryCommand { get; }

        public MainViewModel(FileMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
            _trades = new ObservableCollection<Trade>();
            _monitoringService.NewTradesLoaded += OnNewTradesLoaded;
            ApplyRefreshFrequencyCommand = new RelayCommand(ApplyRefreshFrequency);
            ApplyInputDirectoryCommand = new RelayCommand(ApplyInputDirectory);
            InputDirectory = _monitoringService.GetInputDirectory();
            RefreshFrequency = _monitoringService.GetRefreshFrequency();
        }

        private void ApplyRefreshFrequency()
        {
            if (RefreshFrequency >= 1)
            {
                _monitoringService.UpdateMonitoringFrequency(RefreshFrequency);
                MessageBox.Show($"Refresh Frequency updated successfully to {RefreshFrequency} seconds", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Refresh Frequency should be at least 1 seconds", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyInputDirectory()
        {
            if (Directory.Exists(InputDirectory))
            {
                _monitoringService.UpdateInputDirectory(InputDirectory);
                MessageBox.Show($"Input Directory updated successfully to {InputDirectory}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Directory does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnNewTradesLoaded(object sender, IEnumerable<Trade> newTrades)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var trade in newTrades)
                {
                    Trades.Add(trade);
                }
            });
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();
    }
}