using Microsoft.AspNetCore.Mvc;
using MusicApi.Services;
using MusicApi.Services.Models;

namespace MusicApi.WebApi.Controllers
{
    [ApiController]
    [Route("tracks")]
    public class TracksController : ControllerBase
    {
        private readonly ITrackService _tracks;
        private readonly IWebHostEnvironment _env;

        public TracksController(ITrackService tracks, IWebHostEnvironment env)
        {
            _tracks = tracks;
            _env = env;
        }

        [HttpGet]
        [EndpointSummary("get all tracks")]
        public ActionResult<List<TrackDto>> GetAll() => _tracks.GetAll();

        [HttpGet("{id:int}")]
        [EndpointSummary("get track by id")]
        public ActionResult<TrackDto> GetById(int id)
        {
            var track = _tracks.GetById(id);
            if (track == null)
                return Problem(title: "track is not found", statusCode: StatusCodes.Status404NotFound);
            return track;
        }

        [HttpGet("search")]
        [EndpointSummary("search track by name")]
        public ActionResult<List<TrackDto>> Search([FromQuery] string query)
        {
            var results = _tracks.Search(query);
            if (results.Count == 0)
                return NotFound("song is not found");
            return results;
        }

        [HttpPost]
        [EndpointSummary("add a new track")]
        public ActionResult<TrackDto> Create(TrackRequest request)
        {
            var track = _tracks.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = track.Id }, track);
        }

        [HttpPut("{id:int}")]
        [EndpointSummary("update track")]
        public IActionResult Update(int id, TrackRequest request)
        {
            if (!_tracks.Update(id, request))
                return NotFound($"track {id} is not found");
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [EndpointSummary("delete track")]
        public IActionResult Delete(int id)
        {
            if (!_tracks.Delete(id))
                return NotFound($"track id={id} is not found");
            return NoContent();
        }

        [HttpPost("{id:int}/upload")]
        [EndpointSummary("upload mp3 for track")]
        public async Task<ActionResult<TrackDto>> Upload(int id, IFormFile file)
        {
            if (_tracks.GetById(id) == null)
                return NotFound("track is not found");

            var audioDir = Path.Combine(_env.WebRootPath, "audio");
            Directory.CreateDirectory(audioDir);
            var filePath = Path.Combine(audioDir, $"{id}.mp3");
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return _tracks.SetAudioUrl(id, $"/audio/{id}.mp3")!;
        }
    }
}
