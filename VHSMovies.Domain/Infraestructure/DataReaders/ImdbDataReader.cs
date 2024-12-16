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

        public override Title ReadTitlePage(HtmlDocument document)
        {
            string name = document.DocumentNode.SelectSingleNode("//h1[contains(@data-testid, 'pageTitle')]/span[contains(@class, 'primary-text')]").InnerText;

            HtmlNodeCollection genresCollection = document.DocumentNode.SelectNodes("//div[@class = 'ipc-chip-list__scroller']/a");

            List<Genre> genres = new List<Genre>();

            var validGenres = Enum.GetValues(typeof(Genre)).Cast<Genre>().Select(g => g.ToString().ToLower()).ToList();

            foreach (HtmlNode genresNode in genresCollection)
            {
                string genreText = genresNode.InnerText.Replace("-", "").Replace(" ", "").ToLower();

                if (validGenres.Any(validGenre => genreText.Contains(validGenre)))
                {
                    if (Enum.TryParse(typeof(Genre), genresNode.InnerText.Replace("-", "").Replace(" ", ""), true, out var genre))
                    {
                        genres.Add((Genre)genre);
                    }
                }
            }

            HtmlNode titleUrlNode = document.DocumentNode.SelectSingleNode("//div[contains(@data-testid, 'aggregate-rating')]/a");
            string titleExternalId = "";
            string titleUrl = "";
            decimal rating = 0m;

            if (titleUrlNode != null)
            {
                titleExternalId = titleUrlNode.GetAttributeValue("href", "").Replace("/title/", "").Split("/")[0];
                titleUrl = $"https://www.imdb.com/title/{titleExternalId}";

                HtmlNode ratingNode = titleUrlNode.SelectSingleNode("span/div/div[2]/div[1]/span");
                if (ratingNode != null)
                {
                    rating = decimal.Parse(ratingNode.InnerText);
                }
            }

            string description = document.DocumentNode.SelectSingleNode("//p[@data-testid='plot']/span[3]").InnerText;

            HtmlNodeCollection professionalsNode = document.DocumentNode.SelectNodes("//div[@role = 'presentation']/ul[contains(@class, 'ipc-metadata-list')]/li");

            List<Person> directors = new List<Person>();
            List<Person> writers = new List<Person>();
            List<Person> actors = new List<Person>();

            foreach (HtmlNode professionalNode in professionalsNode)
            {
                HtmlNode role = professionalNode.SelectSingleNode("span");

                if (role != null)
                {
                    if (role.InnerText == "Directors" || role.InnerText == "Diretores")
                    {
                        directors.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Director));
                    }
                    else if (role.InnerText == "Writers" || role.InnerText == "Roteiristas")
                    {
                        writers.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Writer));
                    }
                    else if (role.InnerText == "Stars" || role.InnerText == "Artistas")
                    {
                        actors.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Actor));
                    }
                    else if (role.InnerText.Contains("Director") || role.InnerText.Contains("Direção"))
                    {
                        directors.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Director));
                    }
                    else if (role.InnerText.Contains("Writer") || role.InnerText.Contains("Roteirista"))
                    {
                        writers.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Writer));
                    }
                    else if (role.InnerText.Contains("Star") || role.InnerText.Contains("Artista") || role.InnerText.Contains("Ator"))
                    {
                        actors.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Actor));
                    }
                }
                else
                {
                    role = professionalNode.SelectSingleNode("a[1]");

                    if (role != null)
                    {
                        if (role.InnerText == "Directors" || role.InnerText == "Diretores")
                        {
                            directors.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Director));
                        }
                        else if (role.InnerText == "Writers" || role.InnerText == "Roteiristas")
                        {
                            writers.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Writer));
                        }
                        else if (role.InnerText == "Stars" || role.InnerText == "Artistas")
                        {
                            actors.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Actor));
                        }
                        else if (role.InnerText.Contains("Director") || role.InnerText.Contains("Direção"))
                        {
                            directors.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Director));
                        }
                        else if (role.InnerText.Contains("Writer") || role.InnerText.Contains("Roteirista"))
                        {
                            writers.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Writer));
                        }
                        else if (role.InnerText.Contains("Star") || role.InnerText.Contains("Artista") || role.InnerText.Contains("Ator"))
                        {
                            actors.AddRange(GetPersonData(professionalNode.SelectNodes("div[@class = 'ipc-metadata-list-item__content-container']/ul"), PersonRole.Actor));
                        }
                    }
                }
            }

            List<Review> reviews = new List<Review>
            {
                new Review(GetSourceName(), rating)
            };

            Title title = new Title(titleExternalId, name, description, reviews)
            {
                Genres = genres,
                Url = titleUrl
            };

            List<Cast> cast = new List<Cast>();
            cast.AddRange(directors.Select(d => new Cast(d, title)));
            cast.AddRange(writers.Select(w => new Cast(w, title)));
            cast.AddRange(actors.Select(a => new Cast(a, title)));

            title.Cast = cast;

            return title;
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

        private List<Person> GetPersonData(HtmlNodeCollection nodes, PersonRole role)
        {
            List<Person> people = new List<Person>();

            foreach (HtmlNode node in nodes)
            {
                HtmlNode nameNode = node.SelectSingleNode("li/a");

                string[] externalIdList = nameNode.GetAttributeValue("href", "").Replace("/name/", string.Empty).Split("/");

                string externalId = externalIdList[1];

                string name = nameNode.InnerText;

                people.Add(new Person(externalId, name, $"https://www.imdb.com/name/{externalId}", role));
            }

            return people;
        }

        public override string GetSourceName()
        {
            return "imdb";
        }

    }
}
