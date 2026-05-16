using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ResolutionMatcher.Core;
using ResolutionMatcher.Display;
using ResolutionMatcher.Settings;
using System.Windows;
using Microsoft.Extensions.Options;
namespace ResolutionMatcher;

public partial class App : Application
{
    private IHost? _host;
    private TaskbarIcon? _trayIcon;


  protected override void OnStartup(StartupEventArgs e)
  {
    base.OnStartup(e);
    _trayIcon = (TaskbarIcon)FindResource("TrayIcon");


    _host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
          services.Configure<AppSettings>(
                context.Configuration.GetSection("AppSettings"));

          services.AddSingleton<IDisplayService, DisplayService>();
          services.AddSingleton<IProcessWatcher, ProcessWatcher>();
          services.AddHostedService<ResolutionWatcherService>();
        })
            .Build();

    _host.Start();
  }
    
    private void OnSettingsClicked(object sender, RoutedEventArgs e)
  { 
    if (_host == null) return;
    var settings = _host.Services.GetRequiredService<IOptions<AppSettings>>().Value;
    var window = new SettingsWindow(settings);
    window.Show();
  }
    private void OnQuitClicked(object sender, RoutedEventArgs e)
    {
        _host?.StopAsync().Wait();
        Shutdown();
    }


    protected override void OnExit(ExitEventArgs e)
    {
        _trayIcon?.Dispose();
        _host?.StopAsync().Wait();
        base.OnExit(e);
    }
}