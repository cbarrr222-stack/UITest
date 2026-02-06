namespace WidgetDashboard.Models;

public class SoundCloudTrack
{
    public string Title { get; set; } = "";
    public string Artist { get; set; } = "";
    public string Genre { get; set; } = "";
    public int PlayCount { get; set; }
    public int LikeCount { get; set; }
    public string Duration { get; set; } = "0:00";
    public bool IsPlaying { get; set; }
}
