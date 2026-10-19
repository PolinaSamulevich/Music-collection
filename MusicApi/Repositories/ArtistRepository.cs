using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Models;
namespace MusicApi.Repositories
{
    public class ArtistRepository
    {
        private MusicdbContext _context;
        public ArtistRepository(MusicdbContext context)
        {
            _context = context;
        }
        public List<Artist> GetAll() => _context.Artists.ToList();
        public Artist? GetByIdWithTracks(int id)
        {
            var artist = _context.Artists
                .Include(a => a.Tracks)
                .FirstOrDefault(a => a.Id == id);
            if (artist != null)
            {
                foreach (var track in artist.Tracks)
                    track.Artist = null;
            }
            return artist;
           
        }
    }
}
