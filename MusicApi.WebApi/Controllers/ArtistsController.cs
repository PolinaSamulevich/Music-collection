using Microsoft.AspNetCore.Mvc;
using MusicApi.Services;
using MusicApi.Services.Models;

namespace MusicApi.WebApi.Controllers
{
    [ApiController]
    [Route("artists")]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artists;

        public ArtistsController(IArtistService artists)
        {
            _artists = artists;
        }

        [HttpGet("{id:int}")]
        [EndpointSummary("get artist and tracks")]
        public ActionResult<ArtistDto> GetById(int id)
        {
            var artist = _artists.GetByIdWithTracks(id);
            if (artist == null)
                return NotFound($"artist id={id} is not found");
            return artist;
        }

        [HttpPost]
        [EndpointSummary("add a new artist")]
        public IActionResult Create(ArtistRequest request)
        {
            var artist = _artists.Create(request);
            return Ok($"artist {artist.Name} is added");
        }
    }
}
