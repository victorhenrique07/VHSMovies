using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Entity
{
    public class TitleWriters
    {
        public int WriterId { get; set; }

        public virtual Writer Writer { get; set; }

        public int TitleId { get; set; }

        public virtual Title Title { get; set; }
    }
}
