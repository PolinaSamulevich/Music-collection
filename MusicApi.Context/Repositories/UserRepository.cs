using MusicApi.Context.Models;

namespace MusicApi.Context.Repositories
{
    public interface IUserRepository
    {
        User? GetByEmail(string email);
        void Add(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly MusicdbContext _context;

        public UserRepository(MusicdbContext context)
        {
            _context = context;
        }

        public User? GetByEmail(string email) =>
            _context.Users.FirstOrDefault(u => u.Email == email);

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
