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

        protected DataReader(IHtmlReader htmlReader, IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
            this.htmlReader = htmlReader;
        }

        public Title ReadTitle(string url)
        {
            HtmlDocument document = htmlReader.Read(url);

            Title title = ReadTitlePage(document);

            return title;
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

        public abstract Title ReadTitlePage(HtmlDocument document);

        public abstract IReadOnlyCollection<Title> ReadTitles();

        public abstract string GetSourceName();
    }
}
