using HtmlAgilityPack;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Domain.Infraestructure
{
    public abstract class DataReader : IDataReader
    {
        protected IGenreRepository genreRepository;
        protected IHtmlReader htmlReader;
        protected IReviewRepository reviewRepository;

        protected DataReader(
            IHtmlReader htmlReader,
            IGenreRepository genreRepository,
            IReviewRepository reviewRepository)
        {
            this.genreRepository = genreRepository;
            this.htmlReader = htmlReader;
            this.reviewRepository = reviewRepository;
        }

        public Review ReadReview(string url)
        {
            HtmlDocument document = htmlReader.Read(url);

            Review review = ReadTitlePage(document);

            return review;
        }

        public List<TitleGenre> ReadGenres(string url, int titleId)
        {
            HtmlDocument document = htmlReader.Read(url);

            List<TitleGenre> genres = ReadTitleGenres(document, titleId);

            return genres;
        }

        public List<Genre> CheckValidGenres(HtmlNodeCollection nodes)
        {
            List<Genre> genres = new List<Genre>();

            List<Genre> validGenres = genreRepository
                .GetAll().Result
                .ToList();

            foreach (HtmlNode genresNode in nodes)
            {
                string genreText = genresNode.InnerText.Replace("-", "").Replace(" ", "").ToLower();

                if (validGenres.Any(validGenre => genreText.Contains(validGenre.Name.ToLower())))
                {
                    var matchingGenre = validGenres
                        .FirstOrDefault(g => g.Name.ToLower().Equals(genresNode.InnerText.Replace("-", "").Replace(" ", ""), StringComparison.OrdinalIgnoreCase));

                    if (matchingGenre != null)
                    {
                        genres.Add(matchingGenre);
                    }
                }
            }

            return genres;
        }

        public abstract Review ReadTitlePage(HtmlDocument document);

        public abstract string GetSourceName();

        public abstract string GetFullUrl(string externalId);

        public abstract List<TitleGenre> ReadTitleGenres(HtmlDocument document, int titleId);
    }
}
