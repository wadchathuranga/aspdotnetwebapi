using authentication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace authentication.DbConnections
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private readonly IConfiguration _config;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) : base(options) 
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> Users { get; set; }
    }
}
