﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VHSMovies.Infraestructure;

#nullable disable

namespace VHSMovies.Infraestructure.Migrations
{
    [DbContext(typeof(DbContextClass))]
    [Migration("20250201183714_mudando nome das colunas para AverageRating e TotalReviews")]
    partial class mudandonomedascolunasparaAverageRatingeTotalReviews
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Cast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("PersonId")
                        .HasColumnType("integer");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<int>("TitleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PersonId");

                    b.HasIndex("TitleId");

                    b.ToTable("Casts", (string)null);
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Genres", (string)null);
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("People", (string)null);
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.RecommendedTitle", b =>
                {
                    b.Property<decimal>("AverageRating")
                        .HasColumnType("numeric");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Genres")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PosterImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PrincipalImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Relevance")
                        .HasColumnType("numeric");

                    b.Property<int>("TotalReviews")
                        .HasColumnType("integer");

                    b.ToTable((string)null);

                    b.ToView("recommended_titles", (string)null);
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Rating")
                        .HasColumnType("numeric");

                    b.Property<int>("ReviewCount")
                        .HasColumnType("integer");

                    b.Property<string>("Reviewer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TitleExternalId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TitleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TitleId");

                    b.ToTable("Reviews", (string)null);
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.TVShowSeason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EpisodesQuantity")
                        .HasColumnType("integer");

                    b.Property<int?>("TVShowId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TVShowId");

                    b.ToTable("TVShowSeasons", (string)null);
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Title", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PosterImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PrincipalImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Titles", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.TitleGenre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GenreId")
                        .HasColumnType("integer");

                    b.Property<int>("TitleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("TitleId");

                    b.ToTable("TitlesGenres", (string)null);
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Movie", b =>
                {
                    b.HasBaseType("VHSMovies.Domain.Domain.Entity.Title");

                    b.Property<decimal?>("Duration")
                        .HasColumnType("numeric");

                    b.ToTable("Movies", (string)null);
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.TVShow", b =>
                {
                    b.HasBaseType("VHSMovies.Domain.Domain.Entity.Title");

                    b.ToTable("TVShows", (string)null);
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Cast", b =>
                {
                    b.HasOne("VHSMovies.Domain.Domain.Entity.Person", "Person")
                        .WithMany("Titles")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VHSMovies.Domain.Domain.Entity.Title", "Title")
                        .WithMany()
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Title");
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Review", b =>
                {
                    b.HasOne("VHSMovies.Domain.Domain.Entity.Title", "Title")
                        .WithMany("Ratings")
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Title");
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.TVShowSeason", b =>
                {
                    b.HasOne("VHSMovies.Domain.Domain.Entity.TVShow", null)
                        .WithMany("Seasons")
                        .HasForeignKey("TVShowId");
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.TitleGenre", b =>
                {
                    b.HasOne("VHSMovies.Domain.Domain.Entity.Genre", "Genre")
                        .WithMany("Titles")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VHSMovies.Domain.Domain.Entity.Title", "Title")
                        .WithMany("Genres")
                        .HasForeignKey("TitleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Title");
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Movie", b =>
                {
                    b.HasOne("VHSMovies.Domain.Domain.Entity.Title", null)
                        .WithOne()
                        .HasForeignKey("VHSMovies.Domain.Domain.Entity.Movie", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.TVShow", b =>
                {
                    b.HasOne("VHSMovies.Domain.Domain.Entity.Title", null)
                        .WithOne()
                        .HasForeignKey("VHSMovies.Domain.Domain.Entity.TVShow", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Genre", b =>
                {
                    b.Navigation("Titles");
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Person", b =>
                {
                    b.Navigation("Titles");
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.Title", b =>
                {
                    b.Navigation("Genres");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("VHSMovies.Domain.Domain.Entity.TVShow", b =>
                {
                    b.Navigation("Seasons");
                });
#pragma warning restore 612, 618
        }
    }
}
