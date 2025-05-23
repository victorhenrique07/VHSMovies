using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using OpenQA.Selenium.BiDi.Modules.Log;

using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infrastructure;

namespace VHSMovies.Infraestructure
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;
        private readonly IWebHostEnvironment env;

        public DbContextClass(IConfiguration configuration, IWebHostEnvironment env, DbContextOptions<DbContextClass> options = null)
            : base(options)
        {
            this.env = env;
            Configuration = configuration;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                DbConfigurationManager manager = DbConfigurationManager.Instance;

                string DATABASE_HOST = manager.GetConfigurationValue("DATABASE_HOST");
                string DATABASE_USERNAME = manager.GetConfigurationValue("DATABASE_USERNAME");
                string DATABASE_PASSWORD = manager.GetConfigurationValue("DATABASE_PASSWORD");
                string DATABASE_NAME = manager.GetConfigurationValue("DATABASE_NAME");
                string DATABASE_PORT = manager.GetConfigurationValue("DATABASE_PORT");

                string conectionString = $"Server={DATABASE_HOST};Port={DATABASE_PORT};Database={DATABASE_NAME};Uid={DATABASE_USERNAME};Pwd={DATABASE_PASSWORD};";

                options.UseNpgsql(conectionString);
                options.EnableSensitiveDataLogging();
            }
        }

        public DbSet<Cast> Casts { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<TitleGenre> TitlesGenres { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<RecommendedTitle> RecommendedTitles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Title>().ToTable("titles");
            modelBuilder.Entity<TitleGenre>().ToTable("titles_genres");
            modelBuilder.Entity<Cast>().ToTable("casts");
            modelBuilder.Entity<Person>().ToTable("people");
            modelBuilder.Entity<Review>().ToTable("reviews");
            modelBuilder.Entity<Genre>().ToTable("genres");

            if (env.IsDevelopment())
            {
                modelBuilder.Entity<RecommendedTitle>(entity =>
                {
                    entity.HasNoKey();
                    entity.ToTable("recommended_titles");
                });
            }
            else
            {
                modelBuilder.Entity<RecommendedTitle>(entity =>
                {
                    entity.HasNoKey();
                    entity.ToView("recommended_titles");
                });
            }

                modelBuilder.Entity<Title>()
                    .Property(t => t.Relevance)
                    .HasColumnType("numeric(5,2)");

            modelBuilder.Entity<TitleGenre>()
                .Property(tg => tg.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Cast>()
                .HasIndex(tp => tp.Id)
                .IsUnique();

            modelBuilder.Entity<Person>()
                .HasIndex(tp => tp.Id)
                .IsUnique();

            modelBuilder.Entity<Person>()
                .Property(tp => tp.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Title)
                .WithMany(t => t.Ratings)
                .HasForeignKey("TitleId")
                .IsRequired();

            modelBuilder.Entity<Title>()
                .HasMany(r => r.Ratings)
                .WithOne(r => r.Title)
                .HasForeignKey("TitleId")
                .IsRequired();

            modelBuilder.Entity<Cast>()
                .HasOne(t => t.Title)
                .WithMany(t => t.Casts)
                .HasForeignKey("TitleId");

            modelBuilder.Entity<Cast>()
                .HasOne(t => t.Person)
                .WithMany(t => t.Titles)
                .HasForeignKey("PersonId");

            modelBuilder.Entity<TitleGenre>()
                .HasOne(tg => tg.Title)
                .WithMany(t => t.Genres)
                .HasForeignKey("TitleId");

            modelBuilder.Entity<TitleGenre>()
                .HasOne(tg => tg.Genre)
                .WithMany(t => t.Titles)
                .HasForeignKey("GenreId");

            modelBuilder.Entity<Genre>()
                .HasKey(g => g.Id);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RecommendedTitle>()
                .HasKey(rt => rt.Id);
        }
    }
}
