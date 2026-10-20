using MusicApi.Data;
using MusicApi.Models;
using Microsoft.EntityFrameworkCore;
namespace MusicApi.Repositories
{
    public class UserRepository
    {
        private MusicdbContext _context;
        public UserRepository(MusicdbContext context)
        {
            _context = context;
        }
        public User? GetByEmail(string email)=>
               _context.Users.FirstOrDefault(u => u.Email==email);
        
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
