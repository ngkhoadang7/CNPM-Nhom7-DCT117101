using MovieTheater.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieTheater.Data
{
    public class MovieTheaterContext : IdentityDbContext<User>
    {
        public MovieTheaterContext(DbContextOptions<MovieTheaterContext> options) : base(options){}

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<Showtime>().ToTable("Showtime");
            modelBuilder.Entity<Theater>().ToTable("Theater");
            modelBuilder.Entity<Ticket>().ToTable("Ticket");
        }
    }
}