namespace MusicApi.Models
{
    public record TrackDto
    (
        int Id,
        string Title,
        string Artist,
        string Genre,
        string Duration,
        string? AudioUrl
    );
}
