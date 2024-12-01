using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class Director : Person
    {
        public Director() { }

        public Director(string name, string externalId, ICollection<TitleDirectors> titles) :
            base(externalId, name)
        {
            Titles = titles;
        }

        public ICollection<TitleDirectors> Titles { get; set; } = new List<TitleDirectors>();
    }
}
