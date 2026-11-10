using System.ComponentModel.DataAnnotations;

namespace MusicApi.Services.Models
{
    public record GenreRequest([Required] string Name);

    public record GenreDto(int Id, string Name);
}
