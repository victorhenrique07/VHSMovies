using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class Writer : Person
    {
        public Writer(string externalId, string name, IReadOnlyCollection<Title> titles) : 
            base(externalId, name, titles)
        {
        }
    }
}
