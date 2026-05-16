using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.IO;
namespace ResolutionMatcher.Settings;

public partial class SettingsWindow : Window
{
    private readonly AppSettings _settings;
    public SettingsWindow(AppSettings settings)
    {
        try
        {

            InitializeComponent();
            _settings = settings;
            ProcessNameTextBox.Text = settings.GameProcessName;
            //find the matching resolution in the dropdown and select it
            foreach (var ratio in settings.Resolutions.Keys)
            {
                AspectRatioComboBox.Items.Add(new ComboBoxItem { Content = ratio.Replace("_", ":"), Tag = ratio });

            }
            //select the aspect ratio that matches the target resolution
            var targetRatio = settings.TargetAspectRatio;
            var aspectRatioItem = AspectRatioComboBox.Items.Cast<ComboBoxItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == targetRatio);
            if (aspectRatioItem != null)
            {
                AspectRatioComboBox.SelectedItem = aspectRatioItem;
                OnAspectRatioChanged(this, null!); // manually populate resolutions
            }
        }
        catch (Exception ex)
        {
            File.AppendAllText("error.log", $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n\n");
            MessageBox.Show($"Error logged to error.log", "Error", MessageBoxButton.OK);
            Close();
        }
    }

    private void OnAspectRatioChanged(object sender, SelectionChangedEventArgs e)
    {
        if (AspectRatioComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            var ratio = (AspectRatioComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();

            if (ratio != null && _settings.Resolutions.ContainsKey(ratio))
            {
                ResolutionComboBox.Items.Clear();
                foreach (var res in _settings.Resolutions[ratio])
                {
                    ResolutionComboBox.Items.Add(new ComboBoxItem { Content = $"{res.Width}x{res.Height}" });
                }
            }
        }
    }
    private void OnSaveClicked(object sender, RoutedEventArgs e)
    {
        _settings.GameProcessName = ProcessNameTextBox.Text;
        _settings.TargetAspectRatio = (AspectRatioComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? _settings.TargetAspectRatio;

        if (ResolutionComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            var resolution = selectedItem.Content.ToString()?.Split('x');
            if (resolution != null && resolution.Length == 2)
            {
                _settings.TargetWidth = int.Parse(resolution[0]);
                _settings.TargetHeight = int.Parse(resolution[1]);
            }
        }

        var json = JsonSerializer.Serialize(new { AppSettings = _settings },
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("appsettings.json", json);
        Close();
    }
}