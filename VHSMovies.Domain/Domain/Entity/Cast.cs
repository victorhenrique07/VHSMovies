using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Domain.Domain.Entity
{
    public class Cast
    {
        public int Id { get; set; }

        public int TitleId { get; set; }
        public Title Title { get; set; }

        public int PersonId { get; set; }
        public PersonRole Role { get; set; }

        public Cast() { }
    }
}
