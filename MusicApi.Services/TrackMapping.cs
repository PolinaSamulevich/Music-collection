using MusicApi.Context.Models;
using MusicApi.Services.Models;

namespace MusicApi.Services
{
    internal static class TrackMapping
    {
        public static TrackDto ToDto(this Track t) => new(
            t.Id,
            t.Title,
            t.Artist?.Name ?? "Untitled",
            t.Genre?.Name ?? "Untitled",
            FormatDuration(t.DurationSeconds),
            t.AudioUrl,
            t.ArtistId,
            t.GenreId,
            t.DurationSeconds);

        private static string FormatDuration(int seconds) => $"{seconds / 60}:{seconds % 60:D2}";
    }
}
