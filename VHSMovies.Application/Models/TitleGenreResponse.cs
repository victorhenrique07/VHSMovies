using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Application.Models
{
    public class TitleGenreResponse
    {
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int TitleId { get; set; }
    }
}
