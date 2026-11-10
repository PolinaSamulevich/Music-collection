using Microsoft.EntityFrameworkCore;
using MusicApi.Context.Models;

namespace MusicApi.Context.Repositories
{
    public interface ITrackRepository
    {
        List<Track> GetAll();
        Track? GetById(int id);
        List<Track> Search(string query);
        void Add(Track track);
        void Update(Track track);
        void Delete(Track track);
    }

    public class TrackRepository : ITrackRepository
    {
        private readonly MusicdbContext _context;

        public TrackRepository(MusicdbContext context)
        {
            _context = context;
        }

        public List<Track> GetAll() =>
            _context.Tracks.Include(t => t.Artist).Include(t => t.Genre).ToList();

        public Track? GetById(int id) =>
            _context.Tracks
                .Include(t => t.Artist)
                .Include(t => t.Genre)
                .FirstOrDefault(t => t.Id == id);

        public List<Track> Search(string query) =>
            _context.Tracks
                .Include(t => t.Artist)
                .Include(t => t.Genre)
                .Where(t => t.Title.Contains(query))
                .ToList();

        public void Add(Track track)
        {
            _context.Tracks.Add(track);
            _context.SaveChanges();
        }

        // Track приходит из GetById (отслеживается контекстом) — достаточно сохранить изменения.
        public void Update(Track track)
        {
            _context.SaveChanges();
        }

        public void Delete(Track track)
        {
            _context.Tracks.Remove(track);
            _context.SaveChanges();
        }
    }
}
