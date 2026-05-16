using System.Configuration;
using System.Data;
using System.Windows;
using ResolutionMatcher.Display;

namespace ResolutionMatcher;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    try
    {
        var service = new DisplayService();
        var resolution = service.GetCurrentResolution();
      MessageBox.Show($"Current resolution: {resolution.Width}x{resolution.Height}");
      Shutdown();
    }
    catch (Exception ex)
    {
      MessageBox.Show($"Error: {ex.Message}");
        
    }
}
}

