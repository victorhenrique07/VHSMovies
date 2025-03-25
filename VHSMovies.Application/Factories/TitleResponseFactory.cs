using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Application.Models;
using VHSMovies.Domain.Domain.Entity;

namespace VHSMovies.Application.Factories
{
    public class TitleResponseFactory
    {
        public TitleResponse CreateTitleResponseByRecommendedTitle(RecommendedTitle title, IReadOnlyCollection<Genre> genres = null)
        {
            List<GenreResponse> titlesGenres = new List<GenreResponse>();

            if (genres != null)
            {
                foreach (var titleGenre in title.Genres)
                {
                    Genre genre = genres.Where(g => g.Name.ToLower() == titleGenre.ToLower()).FirstOrDefault();

                    titlesGenres.Add(new GenreResponse(genre.Id, genre.Name));
                }
            }

            return new TitleResponse(title.Id, title.Name, title.ReleaseDate, title.Description, title.AverageRating, title.TotalReviews)
            {
                PrincipalImageUrl = title.PrincipalImageUrl,
                PosterImageUrl = title.PosterImageUrl,
                Genres = titlesGenres
            };
        }

        public TitleResponse CreateTitleResponseByTitle(Title title)
        {
            int rankPosition = 0;

            return new TitleResponse(title.Id, title.Name, title.ReleaseDate.Value, title.Description, title.AverageRating, title.TotalReviews)
            {
                PrincipalImageUrl = title.PrincipalImageUrl,
                PosterImageUrl = title.PosterImageUrl,
                Genres = title.Genres
                                .Select(gt => new GenreResponse(gt.Genre.Name) { Id = gt.Genre.Id})
                                .ToList(),
                RankPosition = rankPosition++
            };
        }
    }
}
