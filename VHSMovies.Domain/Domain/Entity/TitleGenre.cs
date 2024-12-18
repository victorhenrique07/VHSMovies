using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class TitleGenre
    {
        public int Id { get; set; }

        public int TitleId { get; set; }
        public Title Title { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public TitleGenre()
        {
        }

        public TitleGenre(Title title, Genre genre)
        {
            Title = title;
            Genre = genre;
        }
    }
}
