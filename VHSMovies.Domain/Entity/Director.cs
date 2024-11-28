using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class Director : Person
    {
        public Director(string name, string externalId, IReadOnlyCollection<Title> titles) :
            base(externalId, name, titles)
        {
        }
    }
}
