using MusicApi.Context.Models;
using MusicApi.Context.Repositories;
using MusicApi.Services.Models;

namespace MusicApi.Services
{
    public interface IGenreService
    {
        GenreDto Create(GenreRequest request);
    }

    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genres;

        public GenreService(IGenreRepository genres)
        {
            _genres = genres;
        }

        public GenreDto Create(GenreRequest request)
        {
            var genre = new Genre { Name = request.Name };
            _genres.Add(genre);
            return new GenreDto(genre.Id, genre.Name);
        }
    }
}
