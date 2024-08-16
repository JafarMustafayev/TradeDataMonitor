using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TradeMonitor.Models;
using TradeMonitor.Services;

namespace TradeMonitor.ViewModels
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
        public ICommand RefreshCommand { get; } // Yeni əlavə edilən komanda

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
            _monitoringService.UpdateMonitoringFrequency(RefreshFrequency);
        }

        private void ApplyInputDirectory()
        {
            _monitoringService.UpdateInputDirectory(InputDirectory);
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
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