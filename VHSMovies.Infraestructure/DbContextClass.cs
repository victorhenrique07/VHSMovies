﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.BiDi.Modules.Log;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Infraestructure
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(IConfiguration configuration, DbContextOptions<DbContextClass> options = null)
            : base(options)
        {
            Configuration = configuration;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            }
        }

        public DbSet<Cast> Casts { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<TitleGenre> TitlesGenres { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<TVShow> TVShows { get; set; }
        public DbSet<TVShowSeason> TVShowSeasons { get; set; }
        public DbSet<RecommendedTitle> RecommendedTitles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Title>().ToTable("Titles");
            modelBuilder.Entity<TitleGenre>().ToTable("TitlesGenres");
            modelBuilder.Entity<Movie>().ToTable("Movies");
            modelBuilder.Entity<TVShow>().ToTable("TVShows");
            modelBuilder.Entity<TVShowSeason>().ToTable("TVShowSeasons");
            modelBuilder.Entity<Cast>().ToTable("Casts");
            modelBuilder.Entity<Person>().ToTable("People");
            modelBuilder.Entity<Review>().ToTable("Reviews");
            modelBuilder.Entity<Genre>().ToTable("Genres");

            modelBuilder.Entity<RecommendedTitle>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("recommended_titles");
            });

            modelBuilder.Entity<Title>()
                .HasMany(t => t.Ratings)
                .WithOne()
                .HasForeignKey(r => r.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TitleGenre>()
                .Property(tg => tg.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Cast>()
                .HasKey(tp => tp.Id);

            /*modelBuilder.Entity<Cast>()
                .HasOne(tp => tp.Title)
                .WithMany(t => t.Cast)
                .HasForeignKey(tp => tp.TitleId);*/

            modelBuilder.Entity<Cast>()
                .HasOne(tp => tp.Person)
                .WithMany(p => p.Titles)
                .HasForeignKey(tp => tp.PersonId);

            modelBuilder.Entity<Cast>()
                .HasIndex(tp => tp.Id)
                .IsUnique();

            modelBuilder.Entity<Person>()
                .HasIndex(tp => tp.Id)
                .IsUnique();

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Title)
                .WithMany(t => t.Ratings)
                .HasForeignKey(r => r.TitleId)
                .IsRequired();

            modelBuilder.Entity<Title>()
                .HasMany(r => r.Ratings)
                .WithOne(r => r.Title)
                .HasForeignKey(r => r.TitleId)
                .IsRequired();

            modelBuilder.Entity<TitleGenre>()
                .HasOne(tg => tg.Title)
                .WithMany(t => t.Genres)
                .HasForeignKey(tg => tg.TitleId);

            modelBuilder.Entity<TitleGenre>()
                .HasOne(tg => tg.Genre)
                .WithMany(t => t.Titles)
                .HasForeignKey(tg => tg.GenreId);

            modelBuilder.Entity<Genre>()
                .HasKey(g => g.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
