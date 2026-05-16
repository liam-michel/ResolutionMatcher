using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.IO;
namespace ResolutionMatcher.Settings;

public partial class SettingsWindow : Window
{
    public SettingsWindow(AppSettings settings)
    {
        InitializeComponent();
        ProcessNameTextBox.Text = settings.GameProcessName;
        //find the matching resolution in the dropdown and select it
        var target = $"{settings.TargetWidth}x{settings.TargetHeight}";
        foreach (ComboBoxItem item in ResolutionComboBox.Items)
        {
            if (item.Content.ToString() == target)
            {
                ResolutionComboBox.SelectedItem = item;
                break;
            }
        }
    }

    private void OnSaveClicked(object sender, RoutedEventArgs e)
    {
        //read the values from the UI and save them to the settings
        var settings = new AppSettings
        {
            GameProcessName = ProcessNameTextBox.Text,
        };
        if (ResolutionComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            var resolution = selectedItem.Content.ToString()?.Split('x');
            if (resolution != null && resolution.Length == 2)
            {
                settings.TargetWidth = int.Parse(resolution[0]);
                settings.TargetHeight = int.Parse(resolution[1]);
            }
        }
        var json = JsonSerializer.Serialize(new { AppSettings = settings },
        new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("appsettings.json", json);
        Close();
    }
}