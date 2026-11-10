using Microsoft.EntityFrameworkCore;
using MusicApi.Context.Models;

namespace MusicApi.Context.Repositories
{
    public interface IArtistRepository
    {
        Artist? GetByIdWithTracks(int id);
        void Add(Artist artist);
    }

    public class ArtistRepository : IArtistRepository
    {
        private readonly MusicdbContext _context;

        public ArtistRepository(MusicdbContext context)
        {
            _context = context;
        }

        public Artist? GetByIdWithTracks(int id) =>
            _context.Artists
                .Include(a => a.Tracks)
                    .ThenInclude(t => t.Genre)
                .FirstOrDefault(a => a.Id == id);

        public void Add(Artist artist)
        {
            _context.Artists.Add(artist);
            _context.SaveChanges();
        }
    }
}
