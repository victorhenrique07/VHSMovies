using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.BiDi.Modules.Log;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Infraestructure
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
        public DbSet<PersonRole> Roles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
        public DbSet<TitleGenre> TitlesGenres { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<TVShow> TVShows { get; set; }
        public DbSet<TVShowSeason> TVShowSeasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Title>().ToTable("titles");
            modelBuilder.Entity<TitleGenre>().ToTable("titles_genres");
            modelBuilder.Entity<Movie>().ToTable("movies");
            modelBuilder.Entity<TVShow>().ToTable("tvshows");
            modelBuilder.Entity<TVShowSeason>().ToTable("tvshow_seasons");
            modelBuilder.Entity<Cast>().ToTable("casts");
            modelBuilder.Entity<Person>().ToTable("people");
            modelBuilder.Entity<PersonRole>().ToTable("person_role");
            modelBuilder.Entity<Reviewer>().ToTable("reviewers");
            modelBuilder.Entity<Review>().ToTable("reviews");
            modelBuilder.Entity<Genre>().ToTable("genres");

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<PersonRole>(entity =>
            {
                entity.HasKey(pr => new { pr.PersonId, pr.Role });
                entity.HasOne(pr => pr.Person)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(pr => pr.PersonId);
            });

            modelBuilder.Entity<Title>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Description).HasMaxLength(500);
            });

            modelBuilder.Entity<Cast>(entity =>
            {
                entity.HasKey(c => new { c.TitleId, c.PersonId });
                entity.HasOne(c => c.Title)
                    .WithMany(t => t.Cast)
                    .HasForeignKey(c => c.TitleId);
                entity.HasOne(c => c.Person)
                    .WithMany(p => p.Titles)
                    .HasForeignKey(c => c.PersonId);
            });

            modelBuilder.Entity<TitleGenre>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TitleGenre>()
                .HasKey(tg => new { tg.TitleId, tg.GenreId });

            modelBuilder.Entity<TitleGenre>()
                .HasOne(tg => tg.Title)
                .WithMany(t => t.Genres)
                .HasForeignKey(tg => tg.TitleId);

            modelBuilder.Entity<TitleGenre>()
                .HasOne(tg => tg.Genre)
                .WithMany()
                .HasForeignKey(tg => tg.GenreId);

            modelBuilder.Entity<Genre>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<Reviewer>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
                entity.Property(r => r.ShortName).HasMaxLength(50);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.TitleExternalId).HasMaxLength(40);
                entity.Property(r => r.TitleExternalUrl).HasMaxLength(200);
                entity.Property(r => r.Rating).IsRequired().HasPrecision(2, 1);

                entity.HasOne(r => r.Title)
                    .WithMany(t => t.Reviews)
                    .HasForeignKey(r => r.TitleId);

                entity.HasOne(r => r.Reviewer)
                    .WithMany(rv => rv.Reviews)
                    .HasForeignKey(r => r.ReviewerId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
