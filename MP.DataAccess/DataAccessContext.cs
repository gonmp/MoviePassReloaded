using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MP.DataAccess.EntityModels;
using System;

namespace MP.DataAccess
{
    public class DataAccessContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRol> UsersRols { get; set; }

        public DataAccessContext(DbContextOptions<DataAccessContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MoviesGenres>().HasKey(sc => new { sc.MovieId, sc.GenreId });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MoviePass"));
        }
    }
}
