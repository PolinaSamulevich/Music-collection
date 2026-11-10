using Microsoft.AspNetCore.Mvc;
using MusicApi.Services;
using MusicApi.Services.Models;

namespace MusicApi.WebApi.Controllers
{
    [ApiController]
    [Route("Genres")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genres;

        public GenresController(IGenreService genres)
        {
            _genres = genres;
        }

        [HttpPost]
        [EndpointSummary("add a new genre")]
        public IActionResult Create(GenreRequest request)
        {
            var genre = _genres.Create(request);
            return Ok($"genre {genre.Name} is added");
        }
    }
}
