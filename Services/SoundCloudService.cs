using System;
using System.Collections.Generic;
using WidgetDashboard.Models;

namespace WidgetDashboard.Services;

public class SoundCloudService
{
    // SoundCloud's public API requires OAuth and is heavily restricted.
    // We simulate trending tracks to demonstrate the widget UI.
    // In production, you would integrate with the SoundCloud API using a registered client ID.
    private static readonly Random _random = new();

    public List<SoundCloudTrack> GetTrendingTracks()
    {
        return new List<SoundCloudTrack>
        {
            new()
            {
                Title = "Blinding Lights",
                Artist = "The Weeknd",
                Genre = "Pop",
                PlayCount = 2_845_000,
                LikeCount = 485_000,
                Duration = "3:20",
            },
            new()
            {
                Title = "Levitating",
                Artist = "Dua Lipa",
                Genre = "Dance Pop",
                PlayCount = 1_923_000,
                LikeCount = 312_000,
                Duration = "3:23",
            },
            new()
            {
                Title = "Heat Waves",
                Artist = "Glass Animals",
                Genre = "Indie",
                PlayCount = 3_102_000,
                LikeCount = 567_000,
                Duration = "3:58",
            },
            new()
            {
                Title = "Stay",
                Artist = "The Kid LAROI & Justin Bieber",
                Genre = "Pop",
                PlayCount = 2_456_000,
                LikeCount = 398_000,
                Duration = "2:21",
            },
            new()
            {
                Title = "Industry Baby",
                Artist = "Lil Nas X",
                Genre = "Hip Hop",
                PlayCount = 1_789_000,
                LikeCount = 278_000,
                Duration = "3:32",
            },
            new()
            {
                Title = "Midnight City",
                Artist = "M83",
                Genre = "Electronic",
                PlayCount = 1_456_000,
                LikeCount = 234_000,
                Duration = "4:03",
            },
            new()
            {
                Title = "Flowers",
                Artist = "Miley Cyrus",
                Genre = "Pop",
                PlayCount = 4_210_000,
                LikeCount = 720_000,
                Duration = "3:20",
            },
            new()
            {
                Title = "Creepin'",
                Artist = "Metro Boomin ft. The Weeknd",
                Genre = "Hip Hop",
                PlayCount = 1_980_000,
                LikeCount = 310_000,
                Duration = "3:41",
            },
        };
    }

    public static string FormatPlayCount(int count)
    {
        return count switch
        {
            >= 1_000_000 => $"{count / 1_000_000.0:F1}M",
            >= 1_000 => $"{count / 1_000.0:F1}K",
            _ => count.ToString(),
        };
    }
}
