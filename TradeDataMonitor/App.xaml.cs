using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TradeMonitor.Configuration;
using TradeMonitor.Loaders;
using TradeMonitor.Services;
using TradeMonitor.ViewModels;

namespace TradeDataMonitor
{
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AppConfig>(sp => AppConfig.Load("C:\\Users\\Jafar Mustafayev\\Desktop\\New folder\\TradeDataMonitor\\TradeDataMonitor\\appsettings.json"));
            services.AddSingleton<FileMonitoringService>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<MainWindow>();

            services.AddTransient<ITradeLoader, XmlTradeLoader>();
            services.AddTransient<ITradeLoader, CsvTradeLoader>();
            services.AddTransient<ITradeLoader, TxtTradeLoader>();
        }
    }
}