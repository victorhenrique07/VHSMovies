using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;

using RestSharp;

using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Application.Factories
{
    public class TitleResponseFactory
    {
        public TitleResponse CreateTitleResponseByRecommendedTitle(RecommendedTitle title, IReadOnlyCollection<Genre> genres = null)
        {
            List<GenreResponse> titlesGenres = new List<GenreResponse>();

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

            var titleDetails = FindTitleDetails(title.IMDB_Id);

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
            string imageUrl = "";

            return new TitleResponse(title.Id, title.Name, new DateOnly(title.ReleaseDate.Value, 1, 1), "title.Description", title.AverageRating, title.TotalReviews)
            {
                PosterImageUrl = imageUrl,
                Genres = title.Genres
                                .Select(gt => new GenreResponse(gt.Genre.Name) { Id = gt.Genre.Id })
                                .ToList(),
            };
        }

        private TitleDetailsTMDB FindTitleDetails(string imdb_id)
        {
            var options = new RestClientOptions($"https://api.themoviedb.org/3/find/{imdb_id}?external_source=imdb_id");

            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxZDVlNTg2ZWNkODdmY2I2YjFkNmU2Mzg5ZDg0NTUxYiIsIm5iZiI6MTczNDgwMTUwMS42MzMsInN1YiI6IjY3NjZmODVkMzMwYmNlNmVjOTkxMGQ2MSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.SDtBev6hlNPFb-k_N7zSa1U_7S6FM2l7tqnATPGUNH0");
            var response = client.GetAsync(request).Result;

            var t = JsonSerializer.Deserialize<TitleDetailsTMDB>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return t;
        }
    }
}
