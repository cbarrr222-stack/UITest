using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WidgetDashboard.Models;
using WidgetDashboard.Services;

namespace WidgetDashboard.ViewModels;

public partial class SoundCloudViewModel : ObservableObject
{
    private readonly SoundCloudService _service = new();

    [ObservableProperty] private ObservableCollection<TrackItemViewModel> _tracks = new();
    [ObservableProperty] private TrackItemViewModel? _selectedTrack;
    [ObservableProperty] private string _filterGenre = "All";

    public string[] Genres { get; } = { "All", "Pop", "Hip Hop", "Electronic", "Indie", "Dance Pop" };

    public SoundCloudViewModel()
    {
        LoadTracks();
    }

    private void LoadTracks()
    {
        var tracks = _service.GetTrendingTracks();
        Tracks = new ObservableCollection<TrackItemViewModel>(
            tracks.Select((t, i) => new TrackItemViewModel
            {
                Rank = i + 1,
                Title = t.Title,
                Artist = t.Artist,
                Genre = t.Genre,
                PlayCount = SoundCloudService.FormatPlayCount(t.PlayCount),
                LikeCount = SoundCloudService.FormatPlayCount(t.LikeCount),
                Duration = t.Duration,
                RawPlayCount = t.PlayCount,
            })
        );
    }

    [RelayCommand]
    private void FilterByGenre(string genre)
    {
        FilterGenre = genre;
        var allTracks = _service.GetTrendingTracks();

        var filtered = genre == "All"
            ? allTracks
            : allTracks.Where(t => t.Genre == genre).ToList();

        Tracks = new ObservableCollection<TrackItemViewModel>(
            filtered.Select((t, i) => new TrackItemViewModel
            {
                Rank = i + 1,
                Title = t.Title,
                Artist = t.Artist,
                Genre = t.Genre,
                PlayCount = SoundCloudService.FormatPlayCount(t.PlayCount),
                LikeCount = SoundCloudService.FormatPlayCount(t.LikeCount),
                Duration = t.Duration,
                RawPlayCount = t.PlayCount,
            })
        );
    }

    [RelayCommand]
    private void TogglePlay(TrackItemViewModel? track)
    {
        if (track == null) return;

        foreach (var t in Tracks)
        {
            if (t != track) t.IsPlaying = false;
        }

        track.IsPlaying = !track.IsPlaying;
        SelectedTrack = track.IsPlaying ? track : null;
    }
}

public partial class TrackItemViewModel : ObservableObject
{
    [ObservableProperty] private int _rank;
    [ObservableProperty] private string _title = "";
    [ObservableProperty] private string _artist = "";
    [ObservableProperty] private string _genre = "";
    [ObservableProperty] private string _playCount = "";
    [ObservableProperty] private string _likeCount = "";
    [ObservableProperty] private string _duration = "";
    [ObservableProperty] private bool _isPlaying;
    [ObservableProperty] private int _rawPlayCount;

    public string PlayIcon => IsPlaying ? "\u23f8" : "\u25b6";
}
