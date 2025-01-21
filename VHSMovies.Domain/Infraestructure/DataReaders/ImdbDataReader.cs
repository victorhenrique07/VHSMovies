using HtmlAgilityPack;
using OpenQA.Selenium.DevTools.V129.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Domain.Infraestructure.DataReaders
{
    public class ImdbDataReader : DataReader
    {
        public ImdbDataReader(IGenreRepository genreRepository, IHtmlReader htmlReader, IReviewRepository reviewRepository) : base(htmlReader, genreRepository, reviewRepository)
        {
        }

        public override Review ReadTitlePage(HtmlDocument document)
        {
            HtmlNode titleUrlNode = document.DocumentNode.SelectSingleNode("//div[contains(@data-testid, 'aggregate-rating')]/a");
            decimal rating = 0m;

            if (titleUrlNode != null)
            {
                HtmlNode ratingNode = titleUrlNode.SelectSingleNode("span/div/div[2]/div[1]/span");
                if (ratingNode != null)
                {
                    rating = decimal.Parse(ratingNode.InnerText);
                }
            }

            Review review = new Review(GetSourceName(), rating);

            return review;
        }

        public override List<TitleGenre> ReadTitleGenres(HtmlDocument document, int titleId)
        {
            HtmlNodeCollection genresCollection = document.DocumentNode.SelectNodes("//div[@class = 'ipc-chip-list__scroller']/a");

            List<Genre> genres = CheckValidGenres(genresCollection);

            List<TitleGenre> titleGenres = new List<TitleGenre>();

            titleGenres.AddRange(genres.Select(g => new TitleGenre() { GenreId = g.Id, TitleId = titleId }));

            return titleGenres;
        }

        public override string GetSourceName()
        {
            return "imdb";
        }

        public override string GetFullUrl(string externalId)
        {
            return $"https://www.imdb.com/title/{externalId}";
        }
    }
}
