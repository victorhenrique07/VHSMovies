using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;
using RestSharp;

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

            TitleDetailsTMDB titleDetails = FindTitleDetails(title.IMDB_Id).Result;

            if (titleDetails.Movie_Results.Count == 0 && titleDetails.Tv_Results.Count == 0 && titleDetails.Tv_Season_Results.Count == 0)
                return new TitleResponse(title.Id, title.Name, title.ReleaseDate, "", title.AverageRating, title.TotalReviews)
                {
                    PosterImageUrl = "",
                    Genres = titlesGenres
                };

            if (title.Type == (int)TitleType.Movie || title.Type == (int)TitleType.TvMovie)
            {
                TitleDetailsResult movieResult = titleDetails.Movie_Results.FirstOrDefault();

                return new TitleResponse(title.Id, title.Name, title.ReleaseDate, movieResult.overview, title.AverageRating, title.TotalReviews)
                {
                    PosterImageUrl = "https://image.tmdb.org/t/p/original" + movieResult.poster_path,
                    Genres = titlesGenres
                };
            }

            TitleDetailsResult tvResult = titleDetails.Tv_Results.FirstOrDefault();

            if (tvResult == null)
            {
                tvResult = titleDetails.Tv_Season_Results.FirstOrDefault();
            }

            return new TitleResponse(title.Id, title.Name, title.ReleaseDate, tvResult.overview, title.AverageRating, title.TotalReviews)
            {
                PosterImageUrl = "https://image.tmdb.org/t/p/original" + tvResult.poster_path ,
                Genres = titlesGenres
            };
        }

        public TitleResponse CreateTitleResponseByTitle(Title title)
        {
            string imageUrl = "";

            return new TitleResponse(title.Id, title.Name, title.ReleaseDate, "title.Description", title.AverageRating, title.TotalReviews)
            {
                PosterImageUrl = imageUrl,
                Genres = title.Genres
                                .Select(gt => new GenreResponse(gt.Genre.Name) { Id = gt.Genre.Id})
                                .ToList(),
            };
        }

        private async Task<TitleDetailsTMDB> FindTitleDetails(string imdb_id)
        {
            var options = new RestClientOptions($"https://api.themoviedb.org/3/find/{imdb_id}?external_source=imdb_id");

            var client = new RestClient(options);
            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIxZDVlNTg2ZWNkODdmY2I2YjFkNmU2Mzg5ZDg0NTUxYiIsIm5iZiI6MTczNDgwMTUwMS42MzMsInN1YiI6IjY3NjZmODVkMzMwYmNlNmVjOTkxMGQ2MSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.SDtBev6hlNPFb-k_N7zSa1U_7S6FM2l7tqnATPGUNH0");
            var response = await client.GetAsync(request);

            var t = JsonSerializer.Deserialize<TitleDetailsTMDB>(response.Content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return t;
        }
    }
}
