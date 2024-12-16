using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.BiDi.Modules.Log;
using VHSMovies.Domain.Domain.Entity;

namespace LiveChat.Infraestructure
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Cast> Casts { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<TVShow> TVShows { get; set; }
        public DbSet<TVShowSeason> TVShowSeasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Title>().ToTable("Titles");
            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<TVShow>().ToTable("TVShows");
            modelBuilder.Entity<TVShowSeason>().ToTable("TVShowSeasons");
            modelBuilder.Entity<Cast>().ToTable("Casts");
            modelBuilder.Entity<Person>().ToTable("People");
            modelBuilder.Entity<Review>().ToTable("Reviews");

            modelBuilder.Entity<Title>()
                .HasMany(t => t.Ratings)
                .WithOne()
                .HasForeignKey(r => r.Id)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cast>()
                .HasKey(tp => tp.Id);

            modelBuilder.Entity<Cast>()
                .HasOne(tp => tp.Title)
                .WithMany(t => t.Cast)
                .HasForeignKey(tp => tp.TitleId);

            modelBuilder.Entity<Cast>()
                .HasOne(tp => tp.Person)
                .WithMany(p => p.Titles)
                .HasForeignKey(tp => tp.PersonId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Title)
                .WithMany(t => t.Ratings)
                .HasForeignKey(r => r.TitleId);

            modelBuilder.Entity<Person>()
                .Property(p => p.Role)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
