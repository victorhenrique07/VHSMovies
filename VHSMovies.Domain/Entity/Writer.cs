using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class Writer : Person
    {
        public Writer() { }

        public Writer(string externalId, string name, ICollection<TitleWriters> titles) :
            base(externalId, name)
        {
            Titles = titles;
        }

        public ICollection<TitleWriters> Titles { get; set; }
    }
}
