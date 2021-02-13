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
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public DataAccessContext(DbContextOptions<DataAccessContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MoviesGenres>().HasKey(sc => new { sc.MovieId, sc.GenreId });
            builder.Entity<Cinema>().HasMany<Room>(c => c.Rooms).WithOne(r => r.Cinema).HasForeignKey(r => r.CinemaId);
            builder.Entity<Room>().HasOne<Cinema>(r => r.Cinema).WithMany(c => c.Rooms).HasForeignKey(r => r.CinemaId);
            builder.Entity<Show>().HasOne<Movie>(s => s.Movie).WithMany(m => m.Shows).HasForeignKey(s => s.MovieId);
            builder.Entity<Movie>().HasMany<Show>(m => m.Shows).WithOne(s => s.Movie).HasForeignKey(s => s.MovieId);
            builder.Entity<Show>().HasOne<Room>(s => s.Room).WithMany(r => r.Shows).HasForeignKey(s => s.RoomId);
            builder.Entity<Room>().HasMany<Show>(r => r.Shows).WithOne(s => s.Room).HasForeignKey(s => s.RoomId);
            builder.Entity<Purchase>().HasOne<User>(p => p.User).WithMany(u => u.Purchases).HasForeignKey(p => p.UserId);
            builder.Entity<User>().HasMany<Purchase>(u => u.Purchases).WithOne(p => p.User).HasForeignKey(p => p.UserId);
            builder.Entity<Ticket>().HasOne<Purchase>(t => t.Purchase).WithMany(p => p.Tickets).HasForeignKey(t => t.PurchaseId);
            builder.Entity<Purchase>().HasMany<Ticket>(p => p.Tickets).WithOne(t => t.Purchase).HasForeignKey(t => t.PurchaseId);
            builder.Entity<Ticket>().HasOne<Show>(t => t.Show).WithMany(s => s.Tickets).HasForeignKey(t => t.ShowId);
            builder.Entity<Show>().HasMany<Ticket>(s => s.Tickets).WithOne(t => t.Show).HasForeignKey(t => t.ShowId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("MoviePass"));
        }
    }
}
