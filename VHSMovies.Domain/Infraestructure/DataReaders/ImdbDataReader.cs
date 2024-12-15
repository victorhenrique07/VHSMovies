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
        public ImdbDataReader(
            IPersonRepository personRepository,
            IHtmlReader htmlReader,
            ITitleRepository<Movie> movieRepository,
            ITitleRepository<TVShow> tvshowRepository
            ) : base(personRepository, htmlReader, movieRepository, tvshowRepository)
        {
        }

        public override IReadOnlyCollection<Title> ReadTitles()
        {
            List<string> urls = new List<string>()
            {
                "https://www.imdb.com/search/title/?title_type=feature,tv_movie,tv_series,tv_miniseries"
            };

            List<Title> titles = new List<Title>();

            foreach (string url in urls)
            {
                int nodeNumber = 1;


                List<HtmlNode> titleNode = new List<HtmlNode>();

                HtmlDocument document = htmlReader.Read(url);

                IList<HtmlNode> titleNodes = document.DocumentNode.SelectNodes("//div[@class = 'ipc-metadata-list-summary-item__tc']");

                foreach (HtmlNode node in titleNodes)
                {
                    Title title = ReadTitlesList(node, nodeNumber);

                    titles.Add(title);

                    nodeNumber++;
                }
            }

            return titles;
        }

        // Lê a pagina do filme/série.
        public override Title ReadTitlePage(HtmlDocument document)
        {
            string name = document.DocumentNode.SelectSingleNode("//h1[contains(@data-testid, 'pageTitle')]/span[contains(@class, 'primary-text')]").InnerText;

            HtmlNodeCollection genresCollection = document.DocumentNode.SelectNodes("//div[@class = 'ipc-chip-list__scroller']/a");

            List<Genre> genres = new List<Genre>();

            foreach (HtmlNode genresNode in genresCollection)
            {
                 genres = genres
                    .Where(g => Enum.TryParse(typeof(Genre), genresNode.InnerText.Replace("-", "").Replace(" ", ""), true, out _))
                    .ToList();
            }

            HtmlNode titleUrlNode = document.DocumentNode.SelectSingleNode("//div[contains(@data-testid, 'aggregate-rating')]/a");

            string titleExternalId = titleUrlNode.GetAttributeValue("href", "");

            titleExternalId = titleExternalId.Replace("/title/", string.Empty);

            string[] urlList = titleExternalId.Split("/");

            titleExternalId = urlList[0];

            string titleUrl = $"https://www.imdb.com/title/{titleExternalId}";

            string description = document.DocumentNode.SelectSingleNode("//p[@data-testid='plot']/span[3]").InnerText;

            HtmlNode ratingNode = titleUrlNode.SelectSingleNode("span/div/div[2]/div[1]/span");

            decimal rating = decimal.Parse(ratingNode.InnerText);

            HtmlNode professionalsNode = document.DocumentNode.SelectSingleNode("//div[@role = 'presentation']/ul[contains(@class, 'ipc-metadata-list')]");

            HtmlNode directorsNode = professionalsNode.SelectSingleNode("li[1]");
            HtmlNode writersNode = professionalsNode.SelectSingleNode("li[2]");
            HtmlNode actorsNode = professionalsNode.SelectSingleNode("li[3]");

            string castNode = "div[@class = 'ipc-metadata-list-item__content-container']/ul";

            HtmlNodeCollection directorsNodes = directorsNode.SelectNodes(castNode);
            HtmlNodeCollection writersNodes = writersNode.SelectNodes(castNode);
            HtmlNodeCollection actorsNodes = actorsNode.SelectNodes(castNode);

            List<Person> directors = GetPersonData(directorsNodes);
            List<Person> writers = GetPersonData(writersNodes);
            List<Person> actors = GetPersonData(actorsNodes);

            Cast cast = new Cast() { Actors = actors, Directors = directors, Writers = writers };

            List<Review> reviews = new List<Review>()
            {
                new Review(GetSourceName(), rating)
            };

            return new Title(titleExternalId, name, description, cast, genres, reviews);
        }

        public Title ReadTitlesList(HtmlNode titleNode, int nodeNumber)
        {
            HtmlNode nameNode = titleNode.SelectSingleNode("div/div/div[2]/div[contains(@class, 'ipc-title')]/a/h3");

            string name = nameNode.InnerText.Replace($"{nodeNumber}. ", string.Empty);

            string titleUrl = nameNode.ParentNode.GetAttributeValue("href", "");

            string[] externalIdList = titleUrl.Replace("/title/", string.Empty).Split("/");

            string externalId = externalIdList[0];

            HtmlNode descriptionNode = titleNode.SelectSingleNode("div/div[2]/div/div");

            HtmlNode ratingNode = titleNode.SelectSingleNode("div/div[1]/div[2]/span/div/span/span[@class = 'ipc-rating-star--rating']");

            decimal rating = 0m;

            if (ratingNode != null)
            {
                rating = decimal.Parse(ratingNode.InnerText);
            }
            else if (ratingNode == null)
            {
                ratingNode = titleNode.SelectSingleNode("div/div[1]/div[2]/span/span/span[1]");

                if (ratingNode != null)
                    rating = decimal.Parse(ratingNode.InnerText) / 10;
            }


            List<Review> reviews = new List<Review>()
            {
                new Review(GetSourceName(), rating)
            };

            HtmlNode checkType = titleNode.SelectSingleNode("div/div[1]/div[2]/span/span");

            bool isTVShow = (checkType != null) && (checkType.InnerText == "TV Series" || checkType.InnerText == "Série de TV");

            if (!isTVShow)
            {
                return new Movie(
                externalId,
                name,
                descriptionNode.InnerText,
                new Cast(),
                new List<Genre>(),
                reviews, 0m)
                { Url = $"https://www.imdb.com{titleUrl}" };
            }



            return new TVShow(
                externalId,
                name,
                descriptionNode.InnerText,
                new Cast(),
                new List<Genre>(),
                reviews)
            { Url = $"https://www.imdb.com{titleUrl}" };
        }

        private List<Person> GetPersonData(HtmlNodeCollection nodes)
        {
            List<Person> people = new List<Person>();

            foreach (HtmlNode node in nodes)
            {
                HtmlNode nameNode = node.SelectSingleNode("li/a");

                string[] externalIdList = nameNode.GetAttributeValue("href", "").Replace("/name/", string.Empty).Split("/");

                string externalId = externalIdList[0];

                string name = nameNode.InnerText;

                people.Add(new Person(name, externalId, $"https://www.imdb.com/name/{externalId}"));
            }

            return people;
        }

        public override string GetSourceName()
        {
            return "imdb";
        }

    }
}
