using Microsoft.EntityFrameworkCore;
using MusicApi.Models;
using MusicApi.Data;
namespace MusicApi.Repositories
{
    public class TrackRepository
    {
        private MusicdbContext _context;
        private ILogger<TrackRepository> _logger;
        public TrackRepository(MusicdbContext context, ILogger<TrackRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<Track> GetAll() =>
            _context.Tracks.Include(t => t.Artist).Include(t => t.Genre).ToList();
        public Track? GetById(int id)
        {
            var track = _context.Tracks
                .Include(t => t.Artist)
                .Include(t => t.Genre)
                .FirstOrDefault(t => t.Id == id);
            if (track == null)
                _logger.LogWarning("The track with id={Id} was not found", id);
            return track;
        }
        public void Add(Track track)
        {
            _context.Tracks.Add(track);
            _context.SaveChanges();
            _logger.LogInformation("added track {Title}", track.Title);
        }
        public void Delete(int id)
        {
            var track = GetById(id);
            if (track != null)
            {
                _context.Tracks.Remove(track);
                _context.SaveChanges();
                _logger.LogInformation("deleted track {Id}. {Title}", id, track.Title);
            }
        }
        public void Update(int id, Track UpdatedTrack)
        {
            var track = GetById(id);
            if (track != null)
            {
                track.Title = UpdatedTrack.Title;
                track.DurationSeconds = UpdatedTrack.DurationSeconds;
                track.ArtistId = UpdatedTrack.ArtistId;
                track.GenreId = UpdatedTrack.GenreId;
                _context.SaveChanges();
            }
        }
    }
}
