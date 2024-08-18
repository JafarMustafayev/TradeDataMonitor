namespace TradeDataMonitor
{
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        private string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

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
            services.AddSingleton<AppConfig>(sp => AppConfig.Load(configPath));
            services.AddSingleton<FileMonitoringService>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<MainWindow>();

            services.AddTransient<ITradeLoader, XmlTradeLoader>();
            services.AddTransient<ITradeLoader, CsvTradeLoader>();
            services.AddTransient<ITradeLoader, TxtTradeLoader>();
        }
    }
}