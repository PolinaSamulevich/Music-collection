namespace MusicApi.Context.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string? AudioUrl { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ArtistId { get; set; }
        public int GenreId { get; set; }
        public int DurationSeconds { get; set; }
        public Artist? Artist { get; set; }
        public Genre? Genre { get; set; }
    }
}
