using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class TitleDirectors
    {
        public int DirectorId { get; set; }

        public virtual Director Director { get; set; }

        public int TitleId { get; set; }

        public virtual Title Title { get; set; }
    }
}
