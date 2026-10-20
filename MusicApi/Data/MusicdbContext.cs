using Microsoft.EntityFrameworkCore;
using MusicApi.Models;
namespace MusicApi.Data
{
    public class MusicdbContext:DbContext
    {
            public MusicdbContext(DbContextOptions<MusicdbContext> options) : base(options) { }
            public DbSet<Artist> Artists { get; set; }
            public DbSet<Genre> Genres { get; set; }
            public DbSet<Track> Tracks { get; set; }
            public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
