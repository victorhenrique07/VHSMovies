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

        protected ITitleRepository titleRepository;

        protected IHtmlReader htmlReader;

        protected DataReader(
            IPersonRepository personRepository,
            IHtmlReader htmlReader,
            ITitleRepository titleRepository)
        {
            this.personRepository = personRepository;
            this.htmlReader = htmlReader;
            this.titleRepository = titleRepository;
        }

        public Title ReadTitle(string url)
        {
            HtmlDocument document = htmlReader.Read(url);

            Title title = ReadTitlePage(document);

            return title;
        }

        public void AddOrUpdateTitle(Title title, List<Title> titles)
        {
            Title existingTitle = titleRepository.GetByExternalIdAsync(title.ExternalId).Result;

            if (existingTitle != null)
            {
                titleRepository.UpdateByExternalIdAsync(title);

                return;
            }

            titles.Add(title);
            titleRepository.RegisterAsync(title);
        }

        public abstract Title ReadTitlePage(HtmlDocument document);

        public abstract IReadOnlyCollection<Title> ReadTitles();

        public abstract string GetSourceName();
    }
}
