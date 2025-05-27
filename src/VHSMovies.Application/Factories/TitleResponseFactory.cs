using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;

using MySqlX.XDevAPI.Common;

using RestSharp;

using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Infraestructure.Services.Responses;
using VHSMovies.Infrastructure.Services;
using VHSMovies.Infrastructure.Services.Responses;

namespace VHSMovies.Application.Factories
{
    public class TitleResponseFactory
    {
        private readonly ITMDbService tMDbService;

        public TitleResponseFactory() { }

        public TitleResponseFactory(ITMDbService tMDbService)
        {
            this.tMDbService = tMDbService;
        }

        public TitleResponse CreateTitleResponseByRecommendedTitle(RecommendedTitle title, IReadOnlyCollection<Genre> genres = null)
        {
            List<GenreResponse> titlesGenres = new List<GenreResponse>();
            List<Cast> titleCasts = new List<Cast>();

            if (genres != null && !title.Genres.IsNullOrEmpty())
            {
                foreach (var titleGenre in title.Genres)
                {
                    if (titleGenre == null)
                        continue;

                    Genre genre = genres.Where(g => g.Name.ToLower() == titleGenre.ToLower()).FirstOrDefault();

                    titlesGenres.Add(new GenreResponse(genre.Id, genre.Name));
                }
            }

            var titleDetails = tMDbService.FindTitleDetails(title.IMDB_Id).Result;

            TitleDetailsResult result = titleDetails.Movie_Results.FirstOrDefault()
                                      ?? titleDetails.Tv_Results.FirstOrDefault()
                                      ?? titleDetails.Tv_Season_Results.FirstOrDefault()
                                      ?? new();

            DateOnly? release_date = title.ReleaseDate.Value == null
                ? result?.release_date ?? result?.first_air_date :
                new DateOnly(title.ReleaseDate.Value, 1, 1);

            var overview = result?.overview ?? string.Empty;
            var posterUrl = string.IsNullOrEmpty(result?.poster_path)
                ? string.Empty
                : "https://image.tmdb.org/t/p/original" + result.poster_path;
            var backdropUrl = string.IsNullOrEmpty(result?.backdrop_path)
                ? string.Empty
                : "https://image.tmdb.org/t/p/original" + result.backdrop_path;

            return new TitleResponse(title.Id, title.Name, release_date, overview, title.AverageRating, title.TotalReviews)
            {
                PosterImageUrl = posterUrl,
                BackdropImageUrl = backdropUrl,
                Genres = titlesGenres
            };
        }

        public TitleResponse CreateTitleResponseByTitle(Title title)
        {
            TitleDetailsTMDB titleDetails = tMDbService.FindTitleDetails(title.IMDB_Id).Result;

            TitleDetailsResult result = titleDetails.Movie_Results.FirstOrDefault()
                                      ?? titleDetails.Tv_Results.FirstOrDefault()
                                      ?? titleDetails.Tv_Season_Results.FirstOrDefault()
                                      ?? new();

            DateOnly? release_date = title.ReleaseDate.Value == null
                ? result?.release_date ?? result?.first_air_date :
                new DateOnly(title.ReleaseDate.Value, 1, 1);

            var overview = result?.overview ?? string.Empty;
            var posterUrl = string.IsNullOrEmpty(result?.poster_path)
                ? string.Empty
                : "https://image.tmdb.org/t/p/original" + result.poster_path;
            var backdropUrl = string.IsNullOrEmpty(result?.backdrop_path)
                ? string.Empty
                : "https://image.tmdb.org/t/p/original" + result.backdrop_path;


            List<GenreResponse> titleGenres = new List<GenreResponse>();

            foreach (TitleGenre titleGenre in title.Genres)
            {
                if (titleGenre == null)
                    continue;

                Genre genre = titleGenre.Genre;
                titleGenres.Add(new GenreResponse(genre.Id, genre.Name));
            }

            TitleResponse titleResponse = new TitleResponse(title.Id, title.Name, release_date, overview, title.AverageRating, title.TotalReviews)
            {
                PosterImageUrl = posterUrl,
                BackdropImageUrl = backdropUrl,
                Genres = titleGenres
            };

            foreach (Cast cast in title.Casts)
            {
                if (cast == null)
                    continue;

                CastResponse castResponse = new CastResponse()
                {
                    Id = cast.Person.Id,
                    Name = cast.Person.Name
                };

                if (cast.Role == PersonRole.Director)
                {
                    titleResponse.Directors.Add(castResponse);
                }
                else if (cast.Role == PersonRole.Writer)
                {
                    titleResponse.Writers.Add(castResponse);
                }
                else
                {
                    titleResponse.Actors.Add(castResponse);
                }
            }

            TMDbWatchProvider tmdbWatchProvider = tMDbService.FindTitleWatchProvider(result.id, title.Type).Result;

            if (tmdbWatchProvider.results.ContainsKey("BR"))
            {
                titleResponse.Providers = tmdbWatchProvider.results["BR"].flatrate?
                    .Select(provider => new WatchProvider(provider.provider_name, provider.logo_path, "flatrate"))
                    .ToList() ?? new List<WatchProvider>();

                titleResponse.Providers = tmdbWatchProvider.results["BR"].rent?
                    .Select(provider => new WatchProvider(provider.provider_name, provider.logo_path, "rent"))
                    .ToList() ?? new List<WatchProvider>();
            }

            return titleResponse;
        }
    }
}