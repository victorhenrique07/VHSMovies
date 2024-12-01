using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class Genre
    {
        public Genre() { }

        public Genre(string name, ICollection<TitleGenres> titles)
        {
            Name = name;
            Titles = titles;
        }

        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string Name { get; set; }

        public ICollection<TitleGenres> Titles { get; set; } = new List<TitleGenres>();
    }
}
