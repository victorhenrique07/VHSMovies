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
        public TitleResponse CreateTitleResponseByRecommendedTitle(RecommendedTitle title)
        {
            return new TitleResponse(title.Id, title.Name, title.ReleaseDate, title.Description, title.AverageRating, title.TotalReviews)
            {
                PrincipalImageUrl = title.PrincipalImageUrl,
                PosterImageUrl = title.PosterImageUrl,
                Genres = title.Genres.Split(new[] { ", " }, StringSplitOptions.None)
                                .ToList()
                                .Select(gt => new GenreResponse(gt)).ToList()
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
                                .Select(gt => new GenreResponse(gt.Genre.Name))
                                .ToList(),
                RankPosition = rankPosition++
            };
        }
    }
}
