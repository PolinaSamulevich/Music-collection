using MusicApi.Context.Models;

namespace MusicApi.Context.Repositories
{
    public interface IGenreRepository
    {
        void Add(Genre genre);
    }

    public class GenreRepository : IGenreRepository
    {
        private readonly MusicdbContext _context;

        public GenreRepository(MusicdbContext context)
        {
            _context = context;
        }

        public void Add(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
        }
    }
}
