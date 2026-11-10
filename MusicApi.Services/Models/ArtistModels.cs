using System.ComponentModel.DataAnnotations;

namespace MusicApi.Services.Models
{
    public record ArtistRequest([Required] string Name);

    public record ArtistDto(int Id, string Name, List<TrackDto> Tracks);
}
