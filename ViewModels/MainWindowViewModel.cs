using CommunityToolkit.Mvvm.ComponentModel;

namespace WidgetDashboard.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private WeatherViewModel _weather = new();
    [ObservableProperty] private ProgressBarViewModel _progressBar = new();
    [ObservableProperty] private SoundCloudViewModel _soundCloud = new();
}
