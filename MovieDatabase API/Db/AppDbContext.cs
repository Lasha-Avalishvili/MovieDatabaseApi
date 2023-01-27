using Microsoft.EntityFrameworkCore;
using MovieDatabase_API.Models;

namespace MovieDatabase_API.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MovieMap());

            base.OnModelCreating(builder);
        }
    }
}
