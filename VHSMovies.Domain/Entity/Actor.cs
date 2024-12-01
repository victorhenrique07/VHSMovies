using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class Actor : Person
    {
        public Actor()
        {
        }

        public Actor(string name, string externalId, ICollection<Cast> titles) :
            base(externalId, name)
        {
            Titles = titles;
        }

        public ICollection<Cast> Titles { get; set; } = new List<Cast>();
    }
}
