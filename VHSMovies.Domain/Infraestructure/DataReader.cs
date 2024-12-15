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
        protected IPersonRepository personRepository;

        protected ITitleRepository<Movie> movieRepository;

        protected ITitleRepository<TVShow> tvshowRepository;

        protected IHtmlReader htmlReader;

        protected DataReader(
            IPersonRepository personRepository,
            IHtmlReader htmlReader,
            ITitleRepository<Movie> movieRepository,
            ITitleRepository<TVShow> tvshowRepository)
        {
            this.personRepository = personRepository;
            this.htmlReader = htmlReader;
            this.movieRepository = movieRepository;
            this.tvshowRepository = tvshowRepository;
        }

        public Title ReadTitle(string url)
        {
            HtmlDocument document = htmlReader.Read(url);

            Title title = ReadTitlePage(document);

            return title;
        }

        public void AddOrUpdateTitle(Title title, List<Title> titles)
        {
         /*   if (title is Movie movie)
            {
                Movie existingTitle = movieRepository.GetByExternalIdAsync(title.ExternalId).Result;

                if (existingTitle != null)
                {
                    movieRepository.UpdateAsync(movie);

                    return;
                }

                titles.Add(movie);
                movieRepository.RegisterAsync(movie);
            }
            else if (title is TVShow tvshow)
            {
                TVShow existingTitle = tvshowRepository.GetByExternalIdAsync(title.ExternalId).Result;

                if (existingTitle != null)
                {
                    tvshowRepository.UpdateAsync(tvshow);

                    return;
                }

                titles.Add(tvshow);
                tvshowRepository.RegisterAsync(tvshow);
            }*/

        }

        public abstract Title ReadTitlePage(HtmlDocument document);

        public abstract IReadOnlyCollection<Title> ReadTitles();

        public abstract string GetSourceName();
    }
}
