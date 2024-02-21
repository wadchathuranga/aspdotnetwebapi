using authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace authentication.DbConnections
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

    }
}
