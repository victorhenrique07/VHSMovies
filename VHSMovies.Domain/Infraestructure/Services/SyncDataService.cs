using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VHSMovies.Domain.Domain.Entity;
using VHSMovies.Domain.Domain.Repository;

namespace VHSMovies.Domain.Infraestructure.Services
{
    public class SyncDataService
    {
        private readonly IEnumerable<IDataReader> dataReaders;
        private readonly IPersonRepository personRepository;
        private readonly ITitleRepository titleRepository;

        public SyncDataService(IPersonRepository personRepository, ITitleRepository titleRepository, IEnumerable<IDataReader> dataReaders)
        {
            this.dataReaders = dataReaders;
            this.personRepository = personRepository;
            this.titleRepository = titleRepository;
        }

        public void RegisterNewData(string reviewerName)
        {
            var titles = titleRepository.GetAll(reviewerName).Result;

            IReadOnlyCollection<Title> titlesList = ReadTitles(reviewerName);

            foreach (Title title in titlesList)
            {
                bool existingTitle = titles.Select(p => p.ExternalId == title.ExternalId).Any();

                if (!existingTitle)
                {
                    titleRepository.RegisterAsync(title);
                    return;
                }

                return;
            }
        }

        public void UpdateTitles(string reviewerName)
        {

        }

        private IReadOnlyCollection<Title> ReadTitles(string sourceName)
        {
            IDataReader dataReader = dataReaders.Single(r => r.GetSourceName() == sourceName);

            IReadOnlyCollection<Title> titles = dataReader.ReadTitles();

            return titles;
        }
    }
}
