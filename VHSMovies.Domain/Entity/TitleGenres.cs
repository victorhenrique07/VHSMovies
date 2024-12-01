using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class TitleGenres
    {
        public int TitleId { get; set; }
        public virtual Title Title { get; set; }

        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
