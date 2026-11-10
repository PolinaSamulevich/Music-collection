namespace MusicApi.Context.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Track> Tracks { get; set; } = new();
    }
}
