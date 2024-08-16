using System.Windows;
using TradeMonitor.ViewModels;

namespace TradeDataMonitor
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}