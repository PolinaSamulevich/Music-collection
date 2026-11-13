using Microsoft.EntityFrameworkCore;
namespace Auth.Api.Data
{
    public class AuthdbContext:DbContext
    {
        public AuthdbContext(DbContextOptions<AuthdbContext> options) : base(options) { }
        public DbSet<AuthUser> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthUser>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
