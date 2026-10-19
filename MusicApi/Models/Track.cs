using System.ComponentModel.DataAnnotations;
namespace MusicApi.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string? AudioUrl { get; set; }
        [Required(ErrorMessage = "the track name is required")]
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public int GenreId { get; set; }
        [Range(1, 3600, ErrorMessage = "incorrect duration")]
        public int DurationSeconds { get; set; }
        public Artist? Artist { get; set; }
        public Genre? Genre { get; set; }
        public string DurationFormatted => $"{DurationSeconds / 60}:{DurationSeconds % 60:D2}";
    }
}
