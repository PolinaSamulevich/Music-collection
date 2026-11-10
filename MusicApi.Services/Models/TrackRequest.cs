using System.ComponentModel.DataAnnotations;

namespace MusicApi.Services.Models
{
    /// <summary>Тело запроса для создания и обновления трека.</summary>
    public record TrackRequest(
        [Required(ErrorMessage = "the track name is required")] string Title,
        int ArtistId,
        int GenreId,
        [Range(1, 3600, ErrorMessage = "incorrect duration")] int DurationSeconds);
}
