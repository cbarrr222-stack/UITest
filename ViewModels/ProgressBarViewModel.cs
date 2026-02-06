using System;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WidgetDashboard.ViewModels;

public partial class ProgressBarViewModel : ObservableObject
{
    private readonly DispatcherTimer _timer;
    private readonly Random _random = new();
    private int _colorIndex;

    private static readonly Color[] _colors =
    {
        Color.Parse("#FF6B6B"),  // Red
        Color.Parse("#4ECDC4"),  // Teal
        Color.Parse("#45B7D1"),  // Sky Blue
        Color.Parse("#96CEB4"),  // Sage Green
        Color.Parse("#FFEAA7"),  // Pale Yellow
        Color.Parse("#DDA0DD"),  // Plum
        Color.Parse("#FF8A65"),  // Deep Orange
        Color.Parse("#81C784"),  // Light Green
        Color.Parse("#64B5F6"),  // Blue
        Color.Parse("#BA68C8"),  // Purple
    };

    [ObservableProperty] private double _progressValue;
    [ObservableProperty] private IBrush _progressColor;
    [ObservableProperty] private string _progressText = "0%";
    [ObservableProperty] private int _cycleCount;
    [ObservableProperty] private string _cycleText = "Cycle: 1";
    [ObservableProperty] private double _speed = 1.0;
    [ObservableProperty] private bool _isPaused;
    [ObservableProperty] private IBrush _nextColor;

    public ProgressBarViewModel()
    {
        _colorIndex = 0;
        ProgressColor = new SolidColorBrush(_colors[0]);
        NextColor = new SolidColorBrush(_colors[1]);
        CycleCount = 1;

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(50),
        };
        _timer.Tick += OnTimerTick;
        _timer.Start();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        if (IsPaused) return;

        ProgressValue += 0.5 * Speed;
        ProgressText = $"{Math.Min(ProgressValue, 100):F0}%";

        if (ProgressValue >= 100)
        {
            ProgressValue = 0;
            _colorIndex = (_colorIndex + 1) % _colors.Length;
            ProgressColor = new SolidColorBrush(_colors[_colorIndex]);
            NextColor = new SolidColorBrush(_colors[(_colorIndex + 1) % _colors.Length]);
            CycleCount++;
            CycleText = $"Cycle: {CycleCount}";
        }
    }

    [RelayCommand]
    private void TogglePause()
    {
        IsPaused = !IsPaused;
    }

    [RelayCommand]
    private void IncreaseSpeed()
    {
        if (Speed < 5.0)
            Speed = Math.Round(Speed + 0.5, 1);
    }

    [RelayCommand]
    private void DecreaseSpeed()
    {
        if (Speed > 0.5)
            Speed = Math.Round(Speed - 0.5, 1);
    }

    [RelayCommand]
    private void Reset()
    {
        ProgressValue = 0;
        _colorIndex = 0;
        ProgressColor = new SolidColorBrush(_colors[0]);
        NextColor = new SolidColorBrush(_colors[1]);
        CycleCount = 1;
        CycleText = "Cycle: 1";
        Speed = 1.0;
        IsPaused = false;
    }
}
