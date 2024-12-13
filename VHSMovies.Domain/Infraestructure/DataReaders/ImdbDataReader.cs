using HtmlAgilityPack;
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
            ITitleRepository titleRepository
            ) : base(personRepository, htmlReader, titleRepository)
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
                    Title title = ReadTitlePage(node, nodeNumber);

                    AddOrUpdateTitle(title, titles);

                    nodeNumber++;
                }
            }

            return titles;
        }

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

            string titleUrl = titleUrlNode.GetAttributeValue("href", "");

            titleUrl = titleUrl.Replace("/title/", string.Empty);

            string[] urlList = titleUrl.Split("/");

            titleUrl = urlList[0];

            HtmlNode ratingNode = titleUrlNode.SelectSingleNode("span/div/div[2]/div[1]/span");

            decimal rating = decimal.Parse(ratingNode.InnerText);

            HtmlNode professionalsNode = document.DocumentNode.SelectSingleNode("//div[@role = 'presentation']/ul[contains(@class, 'ipc-metadata-list')]");

            HtmlNode directorsNode = professionalsNode.SelectSingleNode("li[1]");

            HtmlNodeCollection directorsNodes = directorsNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']");

            List<Director> directors = new List<Director>();

            foreach (HtmlNode directorNode in directorsNodes)
            {
                HtmlNode nameNode = directorNode.SelectSingleNode("ul/li/a");

                string[] externalIdList = nameNode.GetAttributeValue("href", "").Replace("/name/", string.Empty).Split("/");

                string externalId = externalIdList[0];

                string directorName = nameNode.InnerText;

                directors.Add(new Director(directorName, externalId));
            }



            return ;
        }

        public Title ReadTitlePage(HtmlNode titleNode, int nodeNumber)
        {
            HtmlNode nameNode = titleNode.SelectSingleNode("div/div/div[2]/div[contains(@class, 'ipc-title')]/a/h3");

            string name = nameNode.InnerText.Replace($"{nodeNumber}. ", string.Empty);

            string titleUrl = nameNode.ParentNode.GetAttributeValue("href", "");

            string[] externalIdList = titleUrl.Replace("/title/", string.Empty).Split("/");

            string externalId = externalIdList[0];

            HtmlNode descriptionNode = titleNode.SelectSingleNode("div/div[2]/div/div");

            HtmlNode ratingNode = titleNode.SelectSingleNode("div/div/div[2]/span/div/span/span[@class = 'ipc-rating-start--rating']");

            Review reviewer = new Review("IMDb", decimal.Parse(ratingNode.InnerText));

            return new Title(
                externalId,
                name,
                descriptionNode.InnerText,
                new List<Genre>(),
                new List<Review>() { reviewer });
        }

        public override string GetSourceName()
        {
            return "imdb";
        }

    }
}
