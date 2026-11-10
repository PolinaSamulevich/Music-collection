namespace MusicApi.Services.Models
{
    public record TrackDto
    (
        int Id,
        string Title,
        string Artist,
        string Genre,
        string Duration,
        string? AudioUrl,
        int ArtistId,
        int GenreId,
        int DurationSeconds
    );
}
