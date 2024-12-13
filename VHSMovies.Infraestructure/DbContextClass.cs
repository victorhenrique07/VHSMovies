using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<TitleDirectors> TitleDirectors { get; set; }
        public DbSet<TitleWriters> TitleWriters { get; set; }
        public DbSet<TVShow> TVShows { get; set; }
        public DbSet<TVShowSeason> TVShowSeasons { get; set; }
        public DbSet<Writer> Writers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cast>()
                .HasKey(c => new { c.ActorId, c.TitleId });

            modelBuilder.Entity<TitleDirectors>()
                .HasKey(td => new { td.TitleId, td.DirectorId });

            modelBuilder.Entity<TitleDirectors>()
                .HasOne(td => td.Title)
                .WithMany(t => t.Directors)
                .HasForeignKey(td => td.TitleId);

            modelBuilder.Entity<TitleDirectors>()
                .HasOne(td => td.Director)
                .WithMany(d => d.Titles)
                .HasForeignKey(td => td.DirectorId);

            modelBuilder.Entity<TitleWriters>()
                .HasKey(tw => new { tw.TitleId, tw.WriterId });

            modelBuilder.Entity<TitleWriters>()
                .HasOne(tw => tw.Title)
                .WithMany(t => t.Writers)
                .HasForeignKey(tw => tw.TitleId);

            modelBuilder.Entity<TitleWriters>()
                .HasOne(tw => tw.Writer)
                .WithMany(w => w.Titles)
                .HasForeignKey(tw => tw.WriterId);

            modelBuilder.Entity<TVShow>()
                .HasMany(tv => tv.Seasons)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
